using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthIcon : MonoBehaviour
{
    public Sprite Icon1;
    public Sprite Icon2;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = Icon1;
    }

    public void TurnOffIcon()
    {
        image.sprite = Icon2;
    }
}
