using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class UIScore : MonoBehaviour
{
    public int characterLimit;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var score = GameManager.instance.Score;
        text.text = string.Concat(Enumerable.Repeat("0", (characterLimit - score.ToString().Length))) + GameManager.instance.Score.ToString();
        
    }
}
