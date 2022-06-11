using System;
using TMPro;
using UnityEngine;

public class ShowAnimTime : Singleton<ShowAnimTime>
{
    private static AndyAnimator anim;
    private static TextMeshProUGUI text;
    
    private void Start()
    {
        anim = FindObjectOfType<AndyAnimator>();
        text = GetComponent<TextMeshProUGUI>();
    }

    
    private void Update()
    {
        text.text = GetInfoString();
    }


    public static string GetInfoString()
    {
        TimeSpan ts = TimeSpan.FromSeconds(anim.animTime);
        float perc = anim.animTime / anim.loopTime.y * 100;
        return anim.animTime.ToString("F3").PadLeft(8) + " / " + anim.loopTime.y + "   ---   " + (perc.ToString("F1") + "%").PadLeft(6) + "   ---   " + $"{ts.Minutes :00}:{ts.Seconds:00}";
    }
}
