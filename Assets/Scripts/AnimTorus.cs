using UnityEngine;

public class AnimTorus : MonoBehaviour
{
    public int ringCount;
    [Space]
    public float radius;
    public float thickness;
    
    protected RingControll[] rings;
    protected RingState[] states;
    
    public class RingState
    {
        public float completion;
        public float visibility;

        public RingState(float completion, float visibility)
        {
            this.completion = completion;
            this.visibility = visibility;
        }
    }


    protected virtual void CreateRings()
    {
        rings = new RingControll[ringCount];
        
        for (int i = 0; i < ringCount; i++)
            rings[i] = Instantiate(RingControll.Dummy, transform).GetComponent<RingControll>();

        states = new RingState[ringCount];
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, 1);
    }


    protected virtual void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, 1);
    }
    
    
    protected virtual void UpdateRings()
    {
    }
}
