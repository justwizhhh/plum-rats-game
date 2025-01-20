using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    [Space(10)]
    public float MoveSpeed;

    Vector2 startPos;
    Vector2 direction;
    PlayerController player;

    public override void OnEnable()
    {
        base.OnEnable();
        player = GameManager.instance.player;
        startPos = player.transform.position;
        direction = Vector3.Normalize(startPos - (Vector2)transform.position);
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.Translate(direction * MoveSpeed * GameManager.instance.currentGameTime);
    }
}
