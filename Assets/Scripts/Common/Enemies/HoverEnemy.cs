using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEnemy : Enemy
{
    [Space(10)]
    public float MoveSpeed;
    public float WaveHeight;
    public float WaveLength;

    Vector2 spawnPos;

    public override void OnEnable()
    {
        base.OnEnable();
        transform.position = new Vector2(camWidth * 2, cam.transform.position.y + Random.Range(MinSpawnPosY, MaxSpawnPosY));
        spawnPos = transform.position;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position = new Vector2(
            transform.position.x - (MoveSpeed * GameManager.instance.currentGameTime), 
            spawnPos.y + Mathf.Sin(transform.position.x / WaveLength) * WaveHeight);
    }
}
