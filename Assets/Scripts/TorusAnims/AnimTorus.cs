using UnityEngine;

public class AnimTorus : MonoBehaviour
{
    public int ringCount;
    [Space]
    public float radius;
    public float thickness;
    
    [Range(0, 1)]
    public float completion = 1;
    
    [Space]
    public SoundSettings soundSettings;
    [Space]
    
    protected RingControll[] rings;
    protected RingState[] states;
    
    public struct RingState
    {
        public float completion;
        public float visibility;

        public RingState(float completion, float visibility)
        {
            this.completion = completion;
            this.visibility = visibility;
        }
    }

    [System.Serializable]
    public class SoundSettings
    {
        public int torusID;
        public float spinSpeed = 90;
    }


    protected virtual void CreateRings()
    {
        rings = new RingControll[ringCount];
        
        for (int i = 0; i < ringCount; i++)
            rings[i] = Instantiate(RingControll.RingRingDummy, transform).GetComponent<RingControll>().Setup(this);

        states = new RingState[ringCount];
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, 1);
        
        SoundInfo.SetTorus(this);
    }


    protected virtual void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, 1);
    }
    
    
    protected virtual void UpdateRings()
    {
    }
    
    
    public void SetRadius(float radius)
    {
        this.radius = radius;
    }
    
    
    public void SetThickness(float thickness)
    {
        this.thickness = thickness;
    }
    
    
    public void SetCompletion(float completion)
    {
        this.completion = completion;
    }
}
