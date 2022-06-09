using UnityEngine;


public class RingMutation : MonoBehaviour
{
    public float vis = 1;
    [Space]
    
    public float lerpID;
    public float radiusMulti = 1;
    public float thicknessMulti = 1;
    
    [Space]
    public Vector3 posOffset;
    public Vector3 rotOffset;
    
    [HideInInspector]
    public RingControll ring;
    
    
    public void Setup(AnimTorus torus)
    {
        ring = Instantiate(RingControll.RingRingDummy, transform).GetComponent<RingControll>().Setup(torus);
    }


    public void SetVis(float vis)
    {
        this.vis = vis;
    }
}
