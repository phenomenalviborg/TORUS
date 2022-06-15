using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWithRandoPitch : MonoBehaviour
{
    public Vector2 randoPitchRange = new Vector2(.8f,1.6f);

    void OnEnable()
    {
        var randoPitch = Random.Range(randoPitchRange.x, randoPitchRange.y);
        var audioo = GetComponent<AudioSource>();
        audioo.pitch = randoPitch;
        audioo.Play();
    }
    
    
}
