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


    [Range(0, 15)] public float fadeIn;
    [Range(0, 15)] public float fadeOut;
    
    public void Play()
    {
        audioSource.Play();
        StartFadeIn();
    }


    public void Stop()
    {
        StartFadeOut();
        audioSource.SetScheduledEndTime(AudioSettings.dspTime + fadeOut);
    }


    private Transform trans, child;


    //fading
    bool fadingIn;

    public void StartFadeIn()
    {
        timeInSamples = AudioSettings.outputSampleRate * 2 * fadeIn;
        gain = 0;
        counter = 0;
        fadingOut = false;
        fadingIn = true;
    }

    bool fadingOut;

    public void StartFadeOut()
    {
        timeInSamples = AudioSettings.outputSampleRate * 2 * fadeOut;
        //gain = 1;
        counter = 0;
        fadingIn = false;
        fadingOut = true;
    }

    float timeInSamples;
    double gain;
    int counter;

    
    public void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; ++i)
        {
            if (fadingIn)
            {
                gain += 1 / timeInSamples;
                counter++;
                if (gain > 1)
                {
                    gain = 1;
                    fadingIn = false;
                }
            }

            if (fadingOut)
            {
                gain -= 1 / timeInSamples;
                counter++;
                if (gain < 0)
                {
                    gain = 0;
                    fadingOut = false;
                }
            }

            if(fadingIn || fadingOut)
                data[i] = Mathf.Clamp(data[i] * (float) gain, -1, 1);
        }
    }


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

        if(SoundInfo.ShowSounds != child.gameObject.activeInHierarchy)
            child.gameObject.SetActive(SoundInfo.ShowSounds); 
        
        if(SoundInfo.ShowSounds)
            child.localScale = Vector3.one * .04f * volume * (SoundInfo.ShowSounds ? 1 : 0);
            
        
        /*bool shouldBeOn = volume >= .0001f;
        
        if(shouldBeOn != audioSource.enabled)
            audioSource.enabled = shouldBeOn;
        
        if(!shouldBeOn)
            return 0;*/

        audioSource.volume = volume * SoundData.Inst.volume;
        audioSource.spatialBlend = SoundData.Inst.spatialBlend;
        audioSource.dopplerLevel = SoundData.Inst.doppler;

        switch (SoundData.Inst.pitchMode)
        {
            case SoundData.PitchMode.Radius:
                var remappedRadius = ring.radius.Remap(8, 0, 0.1f, 2);
                audioSource.pitch = remappedRadius;
                break;
            case SoundData.PitchMode.Hoopyness:
                audioSource.pitch = ring.hoopines + 1;
                break;
            case SoundData.PitchMode.Height:
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
        Play();

        /*soundID = SoundSystem.Instance.Play(settings.clips, transform, style: SoundStyle.Random, 
            loop: true, startWithRandomOffset: SoundData.Inst.randoOffsetInClips,spatialBlend:SoundData.Inst.spatialBlend,mixerGroup:settings.mixerGroup,transpose:settings.transpose);*/
    }

    void OnDisable()
    {
        Stop();
        /*if(SoundSystem.Instance != null)
            SoundSystem.Instance.Stop(soundID);*/
    }
}