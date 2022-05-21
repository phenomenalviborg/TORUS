using UnityEngine;


public class RingTorus : MonoBehaviour
{
    public int ringCount;
    [Space]
    public float radius;
    public float thickness;
    public float spinSpeed;
    
    private float spin;
    
    private RingControll[] rings;
    
    //private bool showRings;
    
    
    private void Start()
    {
        rings = new RingControll[ringCount];
        
        for (int i = 0; i < ringCount; i++)
            rings[i] = Instantiate(RingControll.Dummy, transform).GetComponent<RingControll>();
    }

    
    private void Update()
    {
        spin += Time.deltaTime * spinSpeed;
        
        const float multi = 1;
        
        float r = radius + thickness;
        float step = 360f / ringCount;
        for (int i = 0; i < ringCount; i++)
        {
            float a = (i * step + spin) * Mathf.Deg2Rad;
            float s = (r + Mathf.Cos(a) * thickness) * multi;
            float h = (Mathf.Sin(a) * thickness) * multi;
            
            rings[i].UpdateRing(Vector3.up * h, Quaternion.identity, s, 2f, 1, true);
        }
    }
}
