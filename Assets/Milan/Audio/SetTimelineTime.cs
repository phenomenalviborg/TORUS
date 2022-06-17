using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetTimelineTime : MonoBehaviour
{
    public PlayableDirector director;
    public AndyAnimator andyAnimator;

    void Start()
    {
        Debug.Log(andyAnimator.time);
        director.time = andyAnimator.time;
        director.Play();
        //director.duration = andyAnimator.loopTime.y;
        InvokeRepeating("UpdateTime",0,10);
    }
    
    // Update is called once per frame
    void UpdateTime()
    {
        director.time = andyAnimator.time;
        director.Play();
    }
}
