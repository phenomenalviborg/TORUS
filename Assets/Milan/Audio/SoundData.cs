using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundData : Singleton<SoundData>
{
    [Range(0,1)]
    public float doppler =1;
    [Range(0,1)]
    public float volume =1;
    [Range(0,1)]
    public float spatialBlend =1;
    public float maxPitchHeight = 8; 
    public bool randoOffsetInClips = false;
    public enum PitchMode {Nothing, Radius, Hoopyness, Height, All}
    public PitchMode pitchMode;

    public List<SettingsPerTorus> settingsPerTorus;
    
    [System.Serializable]
    public class SettingsPerTorus
    {
        public int transpose;
        public AudioMixerGroup mixerGroup;
        public List<AudioClip> clips;
        
    }
    
    public static float KeyToPitch(int midiKey, int transpose, int octave)
    {
        int c4Key = midiKey - 72;
        //apply transpose & octave
        c4Key += transpose;
        c4Key += octave * 12;
        float pitch = Mathf.Pow(2, c4Key / 12f);
        return pitch;
    }

}
