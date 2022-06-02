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
        float perc = anim.animTime / anim.loopTime.y * 100;
        text.text = anim.animTime.ToString("F3").PadLeft(8) + " / " + anim.loopTime.y + "   ---   " + (perc.ToString("F1") + "%").PadLeft(6) + "   ---   " + $"{ts.TotalMinutes:00}:{ts.Seconds:00}";
    }
}
