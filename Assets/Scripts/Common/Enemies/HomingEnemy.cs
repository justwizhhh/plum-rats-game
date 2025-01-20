using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : Enemy
{
    [Space(10)]
    public float StartPosX;
    public float StartSpeed;
    public float MoveSpeed;
    public float AttackWaitTime;

    Vector2 targetPos;
    Vector3 currentSpeed = Vector3.zero;
    bool attacking;

    public override void OnEnable()
    {
        base.OnEnable();
        transform.position = new Vector2(camWidth * 2, cam.transform.position.y + Random.Range(MinSpawnPosY, MaxSpawnPosY));
        targetPos = new Vector2(StartPosX, transform.position.y);
        currentSpeed = Vector3.zero;
        attacking = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!attacking) 
        {
            //transform.position = Vector3.Lerp(transform.position, (Vector3)targetPos, MoveSpeed); 
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)targetPos, StartSpeed * GameManager.instance.currentGameTime);
            if (transform.position.x == StartPosX)
            {
                Invoke("Attack", AttackWaitTime);
            }
        }
        else
        {
            currentSpeed += new Vector3(MoveSpeed * GameManager.instance.currentGameTime, 0, 0);
            transform.position -= currentSpeed;
        }
    }

    public void Attack()
    {
        attacking = true;
    }
}
