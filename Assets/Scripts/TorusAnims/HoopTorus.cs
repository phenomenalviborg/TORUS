using UnityEngine;

public class HoopTorus : AnimTorus
{
    [Space]
    [Range(0, 1)]
    public float spread;
    
    public float twirl;
    public float spin;
    
    protected TwirlState[] twirlStates;
    
    
    private void Start()
    {
        CreateRings();
    }


    protected override void CreateRings()
    {
        base.CreateRings();
        
        twirlStates = new TwirlState[ringCount];
    }

    
    private void Update()
    {
        UpdateRingStates();
        UpdateTwirlStates();
        UpdateRings();
    }


    protected override void UpdateRings()
    {
        float step = 360f / ringCount * spread;
        Quaternion tilt = Quaternion.AngleAxis(twirl * 360f , Vector3.forward) * (Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.up));
        Quaternion sp = Quaternion.AngleAxis(spin * 360f, Vector3.up);
        for (int i = 0; i < ringCount; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(step * i, Vector3.up);
            RingState  rS  = states[i];
            TwirlState tS  = twirlStates[i];
            rings[i].UpdateRing(rot * Vector3.forward * (radius + thickness + tS.distance), sp * (rot * (tilt * tS.twirl)), thickness, rS.completion, rS.visibility, true);
        }
    }
    
    
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, 1);
    }


    protected virtual void UpdateTwirlStates()
    {
        for (int i = 0; i < ringCount; i++)
            twirlStates[i] = new TwirlState(0, Quaternion.identity);
    }
    
    
    public struct TwirlState
    {
        public readonly float distance;
        public readonly Quaternion twirl;

        public TwirlState(float distance, Quaternion twirl)
        {
            this.distance = distance;
            this.twirl    = twirl;
        }
    }
    
    
    public void SetTwirl(float twirl)
    {
        this.twirl = twirl;
    }
    
    
    public void SetSpin(float spin)
    {
        this.spin = spin;
    }
}
