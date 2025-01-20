using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Main gameplay logic for player
    // I sincerely apologise to anyone who disassembles this and looks through the source code of this game

    [Header("Main Physics")]
    public int MaxHealth;
    public int health;

    [Space(10)]
    public float JumpHeight;
    public float MaxJumpTimer;
    public float StompSpeed;

    [Space(10)]
    public float AttackSpeed;
    public float AttackHeightBoost;
    public int AttackMaxCombo;

    float initialGravity;
    float currentJumpTimer;
    int attackCombo;
    Enemy currentEnemy;

    [Space(10)]
    public float HurtTime;
    public float InvincibilityTime;
    public float InvincibilityAnim;

    bool grounded;
    bool jumpInput;
    bool jumping;
    bool attacking;
    bool stomping;
    bool hurt;
    bool invincible;

    List<Collider2D> collision = new List<Collider2D>();

    BoxCollider2D col;
    Rigidbody2D rb;
    SpriteRenderer sr;
    PlayerInput input;
    Animator animator;

    UICombo comboHud;
    UIPlusScore plusScoreHud;
    UIGameOver gameoverHud;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        comboHud = FindObjectOfType<UICombo>();
        plusScoreHud = FindObjectOfType<UIPlusScore>();
        gameoverHud = FindObjectsByType<UIGameOver>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        initialGravity = rb.gravityScale;
        health = MaxHealth;
        StartCoroutine(Invincibility());
    }

    void OnStartJump(InputValue value)
    {
        if (grounded)
        {
            currentJumpTimer = MaxJumpTimer;
            animator.SetTrigger("Jump");
        }
        else
        {
            if (!attacking)
            {
                rb.velocity = new Vector2(0, -StompSpeed);
                currentJumpTimer = 0;
                jumping = false;
                jumpInput = false;
                stomping = true;
                animator.SetTrigger("Stomp");
            }
        }
    }

    // Input functions
    void OnJump(InputValue value)
    {
        jumpInput = value.isPressed;
    }

    void OnStopJump(InputValue value)
    {
        currentJumpTimer = 0;
        jumping = false;
        if (grounded)
        {
            stomping = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Collision
        //Debug.DrawRay(rb.position - new Vector2(0, col.size.y / 2), Vector2.down);
        RaycastHit2D[] floorCheck = Physics2D.RaycastAll(rb.position - new Vector2(0, col.size.y), Vector2.down);
        foreach (RaycastHit2D hit in floorCheck)
        {
            if (hit.collider.gameObject.CompareTag("Foreground"))
            {
                if (hit.distance <= 0) 
                { 
                    grounded = true;
                    stomping = false;
                    attackCombo = 0;
                }
                else { grounded = false; }
            }
            else { continue; }
        }

        // Jumping
        animator.SetFloat("YVelocity", rb.velocity.y);
        if (jumpInput)
        {
            if (!stomping)
            {
                if (grounded)
                {
                    rb.velocity = new Vector2(0, JumpHeight);
                    jumping = true;
                }
                else
                {
                    if (jumping)
                    {
                        if (currentJumpTimer > 0)
                        {
                            rb.velocity = new Vector2(0, JumpHeight);
                            currentJumpTimer -= Time.deltaTime;
                        }
                        else
                        {
                            jumping = false;
                            jumpInput = false;
                        }
                    }
                }
            }
            else
            {
                jumping = false;
                jumpInput = false;
            }
        }

        // Homing attack
        if (attacking)
        {
            if (currentEnemy != null)
            {
                rb.position = new Vector2(rb.position.x, Mathf.Lerp(rb.position.y, currentEnemy.transform.position.y, AttackSpeed));

                foreach (Collider2D hit in collision)
                {
                    if (hit.gameObject == currentEnemy.gameObject && hit.gameObject.activeInHierarchy)
                    {
                        collision.Remove(hit);
                        EndOfAttack();

                        break;
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(col, hit.GetComponent<Collider2D>());
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!hurt && health > 0)
        {
            collision = Physics2D.OverlapBoxAll(rb.position, col.size, rb.rotation).ToList();

            // Mouse controls
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (!attacking && !grounded && !stomping)
                {
                    Collider2D[] mouseCheck = GameManager.instance.mouseCollision;
                    foreach (Collider2D hit in mouseCheck)
                    {
                        if (hit.gameObject.CompareTag("Enemy"))
                        {
                            if (hit.transform.position.x > transform.position.x)
                            {
                                var enemy = hit.gameObject.GetComponent<Enemy>();
                                currentEnemy = enemy;
                                Physics2D.IgnoreCollision(col, hit);

                                GameManager.instance.currentGameTime = AttackSpeed;
                                rb.gravityScale = 0;
                                rb.velocity = Vector2.zero;

                                attacking = true;
                            }
                            else { continue; }
                        }
                        else { continue; }
                    }
                }
            }

            // Getting hurt
            foreach (Collider2D hit in collision)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    if (!invincible)
                    {
                        if (!attacking) { Hurt(); }
                        if (hurt) { Physics2D.IgnoreCollision(col, hit.GetComponent<Collider2D>()); }
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(col, hit.GetComponent<Collider2D>());
                    }
                }

                if (hit.gameObject.CompareTag("Collectible"))
                {
                    int newScore = hit.GetComponent<Collectible>().Score;
                    GameManager.instance.Score += newScore;
                    plusScoreHud.CurrentNewScore = newScore;
                    plusScoreHud.UpdateTimer();
                    hit.gameObject.SetActive(false);
                }
            }
        }

        // Animation
        animator.SetBool("Attacking",attacking);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Hurt", hurt);
        animator.SetBool("Invincible", invincible);
    }

    void Hurt()
    {
        if (!hurt && !invincible)
        {
            health--;
            StartCoroutine(HurtState());
            if (health <= 0)
            {
                Debug.Log("Dead!");
                rb.simulated = false;
                input.enabled = false;
                GameManager.instance.currentGameTime = 0;

                comboHud.CurrentTimer = 0;
                plusScoreHud.CurrentTimer = 0;

                gameoverHud.gameObject.SetActive(true);
                gameoverHud.GameOver();
            }
            else
            {
                UIHealth healthUI = FindObjectOfType<UIHealth>();
                StartCoroutine(healthUI.ResetIcons());
            }
        }
    }

    void EndOfAttack()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnHurt();

            int newScore = currentEnemy.DefeatScore + (attackCombo * (currentEnemy.DefeatScore / 10));
            GameManager.instance.Score += newScore;
            plusScoreHud.CurrentNewScore = newScore;
            plusScoreHud.UpdateTimer();
            comboHud.CurrentCombo = attackCombo;
            comboHud.UpdateTimer();

            GameManager.instance.currentGameTime = GameManager.instance.DefaultGameTime;
            rb.gravityScale = initialGravity;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, AttackHeightBoost));

            attackCombo++;
            attacking = false;
            currentEnemy = null;
        }
    }

    IEnumerator HurtState()
    {
        hurt = true;
        invincible = true;
        attacking = false;
        rb.velocity = Vector2.zero;
        input.enabled = false;
        rb.AddForce(new Vector2(0, AttackHeightBoost));

        yield return new WaitForSeconds(HurtTime);
        input.enabled = true;
        hurt = false;

        yield return new WaitForSeconds(InvincibilityTime);
        invincible = false;
    }

    IEnumerator Invincibility()
    {
        if (invincible)
        {
            sr.enabled = !sr.enabled;
        }
        else { sr.enabled = true; }
        
        yield return new WaitForSeconds(InvincibilityAnim);
        StopCoroutine(Invincibility());
        StartCoroutine(Invincibility());
    }
}
