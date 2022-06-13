using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtTimeStamp : MonoBehaviour
{

    public AudioSource audioSource;
    public AndyAnimator andyAnimator;
    public float time;

    public bool shouldPlay;
    private bool hasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (andyAnimator.animTime < time)
        {
            shouldPlay = false;
            hasPlayed = false;
        }
        else if (andyAnimator.animTime > time)
        {
            shouldPlay = true;
        }
        
        
        if(shouldPlay && !hasPlayed)
        {
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("I should have played");
            hasPlayed = true;
        }

    }
}
