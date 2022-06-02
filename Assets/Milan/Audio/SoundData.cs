using System.Collections;
using System.Collections.Generic;
using atomtwist.AudioNodes;
using UnityEngine;

public class SoundData : Singleton<SoundData>
{
    [Range(0,1)]
    public float doppler =1;
    public bool randoOffsetInClips = false;
    public enum PitchMode {Nothing,Radius, Hoopyness, Height, All}
    public PitchMode pitchMode;
    
    public List<AudioClip> clips;


}