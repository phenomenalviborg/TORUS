using UnityEngine;


public class RingTorus : AnimTorus
{
    [Space]
    [Range(0, 1)]
    public float completion;
    public float spinSpeed;
    private float spin;
    
    private void Start()
    {
        CreateRings();
    }

    
    private void Update()
    {
        spin += Time.deltaTime * spinSpeed;
        
        float r = radius + thickness;
        float step = 360f / ringCount;
        for (int i = 0; i < ringCount; i++)
        {
            float a = (i * step + spin) * Mathf.Deg2Rad;
            float s = (r + Mathf.Cos(a) * thickness);
            float h = (Mathf.Sin(a) * thickness);
            
            rings[i].UpdateRing(Vector3.up * h, Quaternion.identity, s, 1.2f * completion, 1, true);
        }
    }
}
