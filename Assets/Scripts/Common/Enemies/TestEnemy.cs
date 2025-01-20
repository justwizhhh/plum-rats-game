using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    [Space(10)]
    public float MoveSpeed;

    public override void OnEnable()
    {
        base.OnEnable();
        transform.position = new Vector2(camWidth * 2, cam.transform.position.y + Random.Range(MinSpawnPosY, MaxSpawnPosY));
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.Translate(new Vector2(-MoveSpeed * GameManager.instance.currentGameTime, 0));
    }
}
