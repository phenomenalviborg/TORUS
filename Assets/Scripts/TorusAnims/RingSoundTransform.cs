using System.Collections.Generic;
using atomtwist.AudioNodes;
using UnityEngine;


public class RingSoundTransform : MonoBehaviour
{
    public float volume;

    public RingControll ring;
    public int soundID;


    private Transform trans, child;


    public RingSoundTransform Setup(RingControll ring, Vector3 p)
    {
        this.ring = ring;
        trans = transform;
        trans.localPosition = p;

        if (trans.childCount > 0)
            child = trans.GetChild(0);

        return this;
    }


    public float UpdateSound(float volume)
    {
        this.volume = volume;

        if (child != null)
            child.localScale = Vector3.one * .04f * volume * (SoundInfo.ShowSounds ? 1 : 0);
        
        //set stuff on soundsources
        SoundSystem.Instance.SetVolume(soundID, volume * SoundData.Inst.volume);
        SoundSystem.Instance.SetSpatialBlend(soundID,SoundData.Inst.spatialBlend);
        SoundSystem.Instance.SetDoppler(soundID,SoundData.Inst.doppler);

        //pitch mode
        switch (SoundData.Inst.pitchMode)
        {
            case SoundData.PitchMode.Radius :
                var remappedRadius = ring.radius.Remap(8, 0, 0.1f, 2);
                SoundSystem.Instance.SetPitch(soundID,remappedRadius);
                break;
            case SoundData.PitchMode.Hoopyness :
                SoundSystem.Instance.SetPitch(soundID,ring.hoopines+1);
                break;
            case SoundData.PitchMode.Height :
                var remappedHeight = ring.center.y.Remap(0, SoundData.Inst.maxPitchHeight, 0.1f, 2);
                SoundSystem.Instance.SetPitch(soundID,remappedHeight+ring.hoopines);
                break;

        }

        return this.volume;

    }

    private List<AudioClip> clips = new List<AudioClip>();
    void OnEnable()
    {
        var torus = GetComponentInParent<AnimTorus>();
        var clipID = torus.soundSettings.torusID;
        if (clipID < SoundData.Inst.clips.Count)
            clips.Add(SoundData.Inst.clips[clipID]);
        else
        {
            clips.Add(SoundData.Inst.clips[0]);
        }

        soundID = SoundSystem.Instance.Play(clips, transform, style: SoundStyle.Random, loop: true, startWithRandomOffset: SoundData.Inst.randoOffsetInClips,spatialBlend:SoundData.Inst.spatialBlend);
    }

    void OnDisable()
    {
        if(SoundSystem.Instance != null)
            SoundSystem.Instance.Stop(soundID);
    }
}