using System;
using System.Collections.Generic;
using ATVR;
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
        if (VRInput.RightHand.GetPressDown(Button.ButtonOne))
        {
            useSystemTime = !useSystemTime;
            speed = 1;
        }
        
        
        if (useSystemTime)
        {
            DateTime n = DateTime.Now;
            
            int hour = (24 + n.Hour - 4) % 24;
            time = (hour * 60 * 60 + n.Minute * 60 + n.Second) % Mathf.FloorToInt(loopTime.y) + n.Millisecond * .001f;
        }
        else
        {
            speed = Mathf.Clamp(speed + VRInput.RightHand.GetJoystick().x * Time.deltaTime * 5, -10, 10);
            if(VRInput.RightHand.GetPressDown(Button.Joystick))
                speed = 1;
            
            time += Time.deltaTime * speed;
        }
            
        
        animTime = time.Wrap(0, loopTime.y) + loopTime.x;
        
        int count = AllAnims.Count;
        for (int i = 0; i < count; i++)
            AllAnims[i].Evaluate(animTime);
    }
}