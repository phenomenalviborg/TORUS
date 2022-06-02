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
        this.volume = volume;

        if (child != null)
            child.localScale = Vector3.one * .04f * volume * (SoundInfo.ShowSounds ? 1 : 0);
        
        //set stuff on soundsources
        SoundSystem.Instance.SetVolume(soundID, volume * soundData.volume);

        
        switch (soundData.pitchMode)
        {
            case SoundData.PitchMode.Radius :
                var remappedRadius = ring.radius.Remap(8, 0, -1, 2);
                SoundSystem.Instance.SetPitch(soundID,remappedRadius);
                break;
            case SoundData.PitchMode.Hoopyness :
                SoundSystem.Instance.SetPitch(soundID,ring.hoopines+1);
                break;
        }

        return this.volume;

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