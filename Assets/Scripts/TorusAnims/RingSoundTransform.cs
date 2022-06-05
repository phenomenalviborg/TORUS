using System.Collections.Generic;
using atomtwist.AudioNodes;
using UnityEngine;
using UnityEngine.Audio;


public class RingSoundTransform : MonoBehaviour
{
    public float volume;

    public RingControll ring;
    public int soundID;
    public AudioSource audioSource;


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

        audioSource.volume = volume * SoundData.Inst.volume;
        audioSource.spatialBlend = SoundData.Inst.spatialBlend;
        audioSource.dopplerLevel = SoundData.Inst.doppler;
        
        switch (SoundData.Inst.pitchMode)
        {
            case SoundData.PitchMode.Radius :
                var remappedRadius = ring.radius.Remap(8, 0, 0.1f, 2);
                audioSource.pitch = remappedRadius;
                break;
            case SoundData.PitchMode.Hoopyness :
                audioSource.pitch = ring.hoopines + 1;
                break;
            case SoundData.PitchMode.Height :
                var clampedY = Mathf.Clamp(ring.center.y, 0, SoundData.Inst.maxPitchHeight);
                var remappedHeight = clampedY.Remap(0, SoundData.Inst.maxPitchHeight, 0.1f, 2);
                audioSource.pitch = remappedHeight + ring.hoopines;
                break;

        }

        /*//set stuff on soundsources
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

        }*/

        return this.volume;

    }

    private List<AudioClip> clips = new List<AudioClip>();
    void OnEnable()
    {
        var torusID = GetComponentInParent<AnimTorus>().soundSettings.torusID;
        var settings = SoundData.Inst.settingsPerTorus[torusID];

        audioSource.clip = settings.clips[0];
        audioSource.loop = true;
        if (SoundData.Inst.randoOffsetInClips)
            audioSource.time = Random.Range(0, audioSource.clip.length);
        audioSource.spatialBlend = SoundData.Inst.spatialBlend;
        audioSource.outputAudioMixerGroup = settings.mixerGroup;
        audioSource.pitch = SoundData.KeyToPitch(72, settings.transpose, 0);
        audioSource.Play();

        /*soundID = SoundSystem.Instance.Play(settings.clips, transform, style: SoundStyle.Random, 
            loop: true, startWithRandomOffset: SoundData.Inst.randoOffsetInClips,spatialBlend:SoundData.Inst.spatialBlend,mixerGroup:settings.mixerGroup,transpose:settings.transpose);*/
    }

    void OnDisable()
    {
        audioSource.Stop();
        /*if(SoundSystem.Instance != null)
            SoundSystem.Instance.Stop(soundID);*/
    }
}