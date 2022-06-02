using UnityEngine;


public class RingSoundTransform : MonoBehaviour
{
    public float volume;
    
    public RingControll ring;

    private Transform trans, child;
    

    public RingSoundTransform Setup(RingControll ring, Vector3 p)
    {
        this.ring = ring;
        trans = transform;
        trans.localPosition = p;
        
        if(trans.childCount > 0)
            child = trans.GetChild(0);
        
        return this;
    }


    public void UpdateSound(float volume)
    {
        this.volume = volume;
        
        if(child != null)
            child.localScale = Vector3.one * .04f * volume * (SoundInfo.ShowSounds? 1 : 0);
    }
}
