using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    public float DisplayTime;

    UIHealthIcon[] heartObjects;
    
    PlayerController player;
    Camera cam;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        heartObjects = new UIHealthIcon[transform.childCount];
        for (int i = 0; i <= transform.childCount - 1; i++)
        {
            heartObjects[i] = transform.GetChild(i).GetComponent<UIHealthIcon>();
            heartObjects[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.WorldToScreenPoint(player.transform.position);
    }

    public IEnumerator ResetIcons()
    {
        for (int i = 0; i <= heartObjects.Length - 1; i++)
        {
            heartObjects[i].gameObject.SetActive(true);
            if (i >= player.health)
            {
                heartObjects[i].TurnOffIcon();
            }
        }

        yield return new WaitForSeconds(DisplayTime);

        foreach (UIHealthIcon child in heartObjects) { child.gameObject.SetActive(false); }
    }
}
