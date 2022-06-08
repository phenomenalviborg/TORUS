using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopVisTorus : HoopTorus
{
    public float vis;
    
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, vis);
    }
    
    public void SetVis(float vis)
    {
        this.vis = vis;
    }
}
