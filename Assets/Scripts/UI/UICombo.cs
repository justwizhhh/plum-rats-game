using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UICombo : MonoBehaviour
{
    public float DisplayTime;
    public float CurrentTimer;
    public int CurrentCombo;
    
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
            if (CurrentCombo > 0)
            {
                text.enabled = true;
                text.text = "X" + (CurrentCombo + 1) + " Combo!";
            }
        }
        else
        {
            text.enabled = false;
        }
    }
}
