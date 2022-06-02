using System;
using TMPro;
using UnityEngine;

public class ShowAnimTime : MonoBehaviour
{
    private AndyAnimator anim;
    private TextMeshProUGUI text;
    
    private void Start()
    {
        anim = FindObjectOfType<AndyAnimator>();
        text = GetComponent<TextMeshProUGUI>();
    }

    
    private void Update()
    {
        TimeSpan ts = TimeSpan.FromSeconds(anim.animTime);

        text.text = anim.animTime.ToString("F5").PadRight(20) + $"{ts.TotalMinutes:00}:{ts.Seconds:00}";
    }
}
