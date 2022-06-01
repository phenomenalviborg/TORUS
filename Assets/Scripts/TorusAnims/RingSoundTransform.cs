using UnityEngine;


public class RingSoundTransform : MonoBehaviour
{
    public float volume;
    
    public RingControll ring;

    private Transform trans, child;
    

    public RingSoundTransform Setup(RingControll ring)
    {
        this.ring = ring;
        trans = transform;
        
        if(trans.childCount > 0)
            child = trans.GetChild(0);
        
        return this;
    }


    public void UpdateSound(Vector3 pos, float volume)
    {
        this.volume = volume;
        
        if(child != null)
            child.localScale = Vector3.one * .04f * volume * (SoundInfo.ShowSounds? 1 : 0);
        
        if(volume > .00001f)
            trans.localPosition = pos;
    }
}
