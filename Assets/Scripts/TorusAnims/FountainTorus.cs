using UnityEngine;


public class FountainTorus : HoopTorus
{
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(Mathf.Clamp01(completion * ringCount - i * .4f), 1);
    }
}
