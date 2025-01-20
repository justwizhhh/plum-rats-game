using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Script for managing game speed, enemy spawning, score, etc.

    [Header("On-screen Display")]
    public int Score;

    [Space(10)]
    [Header("Scrolling")]
    public float DefaultGameTime;
    public float currentGameTime;

    public Vector2 mousePosition;
    public Collider2D[] mouseCollision;

    [HideInInspector] public PlayerController player;
    Camera cam;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<PlayerController>();
        cam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentGameTime = DefaultGameTime;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic("MainLevelTheme");
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseCollision = Physics2D.OverlapPointAll(mousePosition);
    }

    void OnPause(InputValue value)
    {
        if (Time.timeScale == 0) { Time.timeScale = 1; }
        else { Time.timeScale = 0; }
    }
}
