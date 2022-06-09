using UnityEngine;

public class BabyHoops : HoopTorus
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
            rings[i].UpdateRing(rot * Vector3.forward * (radius + thickness + tS.distance), sp * (rot * (tilt * (tS.twirl * turnRot))), thickness, rS.completion, rS.visibility, true);
        }
    }
    
    
    protected override void UpdateRingStates()
    {
        const float o = .1f, p = 1 - o;
        
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, 1);
            //states[i] = new RingState(Mathf.Clamp01(completion * ringCount - i * .4f), 1);
    }
}
