using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float ScrollSpeedX;
    public float ScrollSpeedY;

    Vector2 startPos;
    float time;

    Rigidbody2D rb;
    Camera cam;
    SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += GameManager.instance.currentGameTime;
        float newPos = Mathf.Repeat(time * ScrollSpeedX, sr.sprite.bounds.size.x);
        rb.position = startPos + Vector2.left * newPos;
    }
}
