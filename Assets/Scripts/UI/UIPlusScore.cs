using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UIPlusScore : MonoBehaviour
{
    public float DisplayTime;
    public float CurrentTimer;
    public float CurrentNewScore;

    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
    }

    public void UpdateTimer()
    {
        CurrentTimer = DisplayTime;
    }

    private void Update()
    {
        if (CurrentTimer > 0)
        {
            CurrentTimer -= Time.deltaTime;
            text.enabled = true;
            text.text = "+" + CurrentNewScore;
        }
        else
        {
            text.enabled = false;
        }
    }
}
