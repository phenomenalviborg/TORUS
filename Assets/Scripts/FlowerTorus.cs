using UnityEngine;

public class FlowerTorus : HoopTorus
{
    [Space]
    public float flyTime;
    public float startDistance;
    public float startAngle;
    public int seed;
    
    
    private Vector3[] rotAxis;
    private float[] offsets;
    
    
    protected override void CreateRings()
    {
        base.CreateRings();
        
        System.Random r = new System.Random(seed);
        rotAxis = new Vector3[ringCount];
        offsets = new float[ringCount];
        for (int i = 0; i < ringCount; i++)
        {
            Vector3 v = new Vector3(r.Range(-1f, 1f), r.Range(-1f, 1f), r.Range(-1f, 1f)).normalized;
            rotAxis[i] = v;
            offsets[i] = r.Range(-0f, 1f);
        }
    }
    
    protected override void UpdateTwirlStates()
    {
        const float o = .1f, p = 1 - o;
        

        for (int i = 0; i < ringCount; i++)
        {
            float offset = offsets[i] * o;
            float lerp = Mathf.Max(0, flyTime - offset) / p;
            
            float d = Mathf.SmoothStep(startDistance, 0, lerp);
            float a = Mathf.SmoothStep(startAngle,    0, lerp);
            
            Quaternion q = Quaternion.AngleAxis(a, rotAxis[i]);
            twirlStates[i] = new TwirlState(d, q);
        }
            
    }
    
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, 1);
    }
}
