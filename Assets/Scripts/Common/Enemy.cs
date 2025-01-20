using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Base enemy class

    public int Health;
    public int DefeatScore;

    [Space(10)]
    public int MaxAmount;
    public float MinSpawnDelay;
    public float MaxSpawnDelay;
    public float MinSpawnPosY;
    public float MaxSpawnPosY;

    public Vector3 prevPos;
    protected float camWidth;

    protected Camera cam;
    protected CircleCollider2D col;
    protected SpriteRenderer sr;

    private void Awake()
    {
        cam = Camera.main;
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
        camWidth = (cam.pixelWidth / cam.pixelHeight) * cam.orthographicSize;
        OnEnable();
    }

    public virtual void OnEnable()
    {
        // Spawning logic goes here
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        // Movement logic goes here
        if (transform.position.x < ((-camWidth * 2) - sr.bounds.size.x)) { gameObject.SetActive(false); }

        prevPos = transform.position;
    }

    public void OnHurt()
    {
        Health--;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            transform.position = new Vector2(camWidth * 2, cam.transform.position.y);
        }
    }
}
