using System.Collections.Generic;
using UnityEngine;

public class MutatedHoopTorus : HoopTorus
{
    public float turn;
    protected override void UpdateRings()
    {
        float step = 360f / ringCount * spread;
        Quaternion tilt = Quaternion.AngleAxis(twirl * 360f , Vector3.forward) * (Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.up));
        Quaternion sp = Quaternion.AngleAxis(spin * 360f, Vector3.up);
        Quaternion turnRot = Quaternion.AngleAxis(turn * 360f, Vector3.up);
        for (int i = 0; i < ringCount; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(step * i, Vector3.up);
            RingState  rS  = states[i];
            TwirlState tS  = twirlStates[i];
            
            rings[i].SaveIt(rot * Vector3.forward * (radius + thickness + tS.distance), sp * (rot * (tilt * (tS.twirl * turnRot))), thickness, rS.completion, rS.visibility);
        }
        
        
        int mutationCount = mutations.Length;
        for (int i = 0; i < mutationCount; i++)
        {
            RingMutation mutation = mutations[i];
            int indexA = Mathf.FloorToInt(mutation.lerpID) % ringCount;
            int indexB = Mathf.CeilToInt(mutation.lerpID)  % ringCount;
            float mutationLerp = mutation.lerpID % 1;
            
            Quaternion rot = Quaternion.AngleAxis(step * mutation.lerpID, Vector3.up);
            RingState rSA = states[indexA];
            RingState rSB = states[indexB];
            RingState  rS  = new RingState(Mathf.Lerp(rSA.completion, rSB.completion, mutationLerp), 
                                           Mathf.Lerp(rSA.visibility, rSB.visibility, mutationLerp) * mutation.vis);
            
            TwirlState tSA  = twirlStates[indexA];
            TwirlState tSB  = twirlStates[indexB];
            TwirlState tS  = new TwirlState(Mathf.Lerp(tSA.distance, tSB.distance, mutationLerp), 
                Quaternion.Slerp(tSA.twirl, tSB.twirl, mutationLerp));
            
            float thick = thickness * mutation.thicknessMulti;
            float r = radius + mutation.radiusMulti;
            
            mutation.ring.SaveIt(rot * Vector3.forward * (r + thick + tS.distance) + mutation.posOffset, 
                (sp * (rot * (tilt * (tS.twirl * turnRot)))) * Quaternion.Euler(mutation.rotOffset), 
                thick, 
                rS.completion, 
                rS.visibility);
        }
        
    }
    
    
    protected override void UpdateRingStates()
    {
        const float o = .1f, p = 1 - o;
        
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, 1);
            //states[i] = new RingState(Mathf.Clamp01(completion * ringCount - i * .4f), 1);
    }
    
    
    public void GetRings(List<RingControll> secondary, ref RingControll mutation)
    {
        for (int i = 0; i < ringCount; i++)
            secondary.Add(rings[i]);
        
        if(mutations.Length != 0)
            mutation = mutations[0].ring;
    }
}
