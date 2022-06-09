using UnityEngine;


public class AppearingRingTorus : RingTorus
{
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(1, Mathf.Clamp01(completion * ringCount - (ringCount - 1 - i)));
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

        
        int mutationCount = mutations.Length;
        for (int i = 0; i < mutationCount; i++)
        {
            RingMutation mutation = mutations[i];
            
            int indexA = Mathf.FloorToInt(mutation.lerpID) % ringCount;
            int indexB = Mathf.CeilToInt(mutation.lerpID) % ringCount;
            float mutationLerp = mutation.lerpID % 1;
            
            float thick = thickness * mutation.thicknessMulti;
                  r = radius * mutation.radiusMulti + thick;
            
            float a = (mutation.lerpID * step + spin + spinOffset) * Mathf.Deg2Rad;
            float s = r + Mathf.Cos(a) * thick;
            float h = Mathf.Sin(a) * thick;
            
            RingState rSA = states[indexA];
            RingState rSB = states[indexB];
            
            mutation.ring.UpdateRing(Vector3.up * h + mutation.posOffset, 
                Quaternion.identity * Quaternion.Euler(mutation.rotOffset), s,
                Mathf.Lerp(rSA.completion, rSB.completion, mutationLerp), 
                Mathf.Lerp(rSA.visibility, rSB.visibility, mutationLerp) * mutation.vis, 
                true);
        }
    }
}


