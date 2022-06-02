using atomtwist.AudioNodes;
using UnityEngine;


public class RingSoundTransform : MonoBehaviour
{
    public float volume;

    public RingControll ring;
    public int soundID;

    private SoundData soundData;

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
        this.volume = Mathf.Lerp(this.volume, volume, Time.deltaTime * 50);

        if (child != null)
            child.localScale = Vector3.one * .04f * this.volume * (SoundInfo.ShowSounds ? 1 : 0);
        
        //set stuff on soundsources
        SoundSystem.Instance.SetVolume(soundID, this.volume);

        if (soundData.pitchMode == SoundData.PitchMode.Hoopyness)
            SoundSystem.Instance.SetPitch(soundID,ring.hoopines+1);
        
        
        return this.volume;
        /*
        switch (soundData.pitchMode)
        {
            case SoundData.PitchMode.Radius :
                var remappedRadius = ring.radius.
                SoundSystem.Instance.SetPitch(ring.);

        }
        if(soundData.pitchMode == )
            SoundSystem.Instance.SetPitch(ring.);#1#*/
    }

    void Awake()
    {
        soundData = SoundData.Inst;
    }

    void OnEnable()
    {
        soundID = SoundSystem.Instance.Play(soundData.clips, transform, style: SoundStyle.Random,
            doppler: soundData.doppler, loop: true, startWithRandomOffset: soundData.randoOffsetInClips);
    }

    void OnDisable()
    {
        if(SoundSystem.Instance != null)
            SoundSystem.Instance.Stop(soundID);
    }
}