using System;
using atomtwist.AudioNodes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioBlendBehaviour : PlayableBehaviour
{
    public SoundsSettings soundSettings;
    
    public int soundID { get; set; }
    
}



