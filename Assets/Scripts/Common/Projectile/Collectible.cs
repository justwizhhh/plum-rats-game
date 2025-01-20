using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int MaxAmount;
    public Vector2[] SpawnPositions;
    public float MinSpawnDelay;
    public float MaxSpawnDelay;
    public float MinSpawnPosY;
    public float MaxSpawnPosY;

    float camWidth;

    [Space(10)]
    public float MoveSpeed;
    public int Score;

    Camera cam;
    CircleCollider2D col;
    SpriteRenderer sr;

    private void Awake()
    {
        cam = Camera.main;
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        camWidth = (cam.pixelWidth / cam.pixelHeight) * cam.orthographicSize;
        Vector2 currentSpawnPos = SpawnPositions[Random.Range(0, SpawnPositions.Length)];
        transform.position = new Vector2(camWidth * 2, currentSpawnPos.y + Random.Range(MinSpawnPosY, MaxSpawnPosY));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector2(-MoveSpeed * GameManager.instance.currentGameTime, 0));
        if (transform.position.x < ((-camWidth * 2) - sr.bounds.size.x)) { gameObject.SetActive(false); }
    }
}
