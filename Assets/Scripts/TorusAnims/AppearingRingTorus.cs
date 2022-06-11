using UnityEngine;


public class AppearingRingTorus : RingTorus
{
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, Mathf.Clamp01(completion * ringCount - (ringCount - 1 - i)));
    }
}


