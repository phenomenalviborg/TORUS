using UnityEngine;


public class RingTorus : AnimTorus
{
    [Space]
    [Range(0, 1)]
    public float completion;
    public float spinSpeed;
    public float spin;
    public float spinOffset;
    
    private void Start()
    {
        CreateRings();
    }

    
    private void Update()
    {
        UpdateRingStates();
        UpdateRings();
    }
    
    
    protected override void UpdateRings()
    {
        spin += Time.deltaTime * spinSpeed;
        
        float r = radius + thickness;
        float step = 360f / ringCount;
        for (int i = 0; i < ringCount; i++)
        {
            float a = (i * step + spin + spinOffset) * Mathf.Deg2Rad;
            float s = r + Mathf.Cos(a) * thickness;
            float h = Mathf.Sin(a) * thickness;
            
            RingState rS = states[i];
            rings[i].UpdateRing(Vector3.up * h, Quaternion.identity, s, rS.completion, rS.visibility, true);
        }
    }
    
    
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, 1);
    }
}
