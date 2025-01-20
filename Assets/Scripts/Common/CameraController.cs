using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CamZoomMargin;
    public float CamZoomSpeed;
    bool zooming;
    float playerDistance;

    [HideInInspector] public Vector2 startPos;

    Camera cam;
    PlayerController player;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        player = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > (cam.orthographicSize - CamZoomMargin))
        {
            zooming = true;
        }
        else
        {
            zooming = false;
            playerDistance = (cam.transform.position.y + cam.orthographicSize) - player.transform.position.y;
        }

        Vector3 targetPos;
        if (zooming)
        {
            targetPos = new Vector3(0, player.transform.position.y + playerDistance, -10);
        }
        else { targetPos = new Vector3(0, 0, -10); }

        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, CamZoomSpeed);
    }
}
