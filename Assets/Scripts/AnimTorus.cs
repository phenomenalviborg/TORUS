using UnityEngine;

public class AnimTorus : MonoBehaviour
{
    public int ringCount;
    [Space]
    public float radius;
    public float thickness;
    
    protected RingControll[] rings;

    
    protected void CreateRings()
    {
        rings = new RingControll[ringCount];
        
        for (int i = 0; i < ringCount; i++)
            rings[i] = Instantiate(RingControll.Dummy, transform).GetComponent<RingControll>();
    }
}
