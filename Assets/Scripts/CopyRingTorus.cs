using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRingTorus : RingTorus
{
    public AppearingRingTorus mate;
    public float vis;
    protected override void UpdateRings()
    {
        spin = mate.spin;
        spinOffset = mate.spinOffset;
        
        base.UpdateRings();
    }


    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, vis);
    }
}
