using System;
using System.Collections.Generic;
using UnityEngine;

public class AndyAnimator : MonoBehaviour
{
    public float time;
    public Vector2 loopTime;
    
    [Space]
    public float speed = 1;
    
    [Space]
    public bool useSystemTime;
    
    [Header("AnimTime ReadOut")]
    public float animTime;
    
    public static readonly List<ValueAnimator> AllAnims = new List<ValueAnimator>();


    private void Update()
    {
        if (useSystemTime)
        {
            DateTime n = DateTime.Now;
            
            int hour = (24 + n.Hour - 4) % 24;
            time = hour * 60 * 60 + n.Minute * 60 + n.Second + n.Millisecond * .001f;
        }
        else
            time += Time.deltaTime * speed;
        
        animTime = time % loopTime.y + loopTime.x;
        
        int count = AllAnims.Count;
        for (int i = 0; i < count; i++)
            AllAnims[i].Evaluate(animTime);
    }
}