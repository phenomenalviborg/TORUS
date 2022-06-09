using UnityEngine;

public class HoopBabyTorus : HoopTorus
{
    [Space]
    public int seed;
    
    [Range(0, 1)]
    public float toPositions;
    public Transform[] positions;
    
    private float[] offsets;
    
    private float vis;
    private Placement[] placements;
    
    

    protected override void CreateRings()
    {
        base.CreateRings();
        
        System.Random r = new System.Random(seed);
        offsets = new float[ringCount];
        for (int i = 0; i < ringCount; i++)
            offsets[i] = r.Range(-0f, 1f);
        
        placements = new Placement[ringCount];
    }


    protected override void UpdateRings()
    {
        const float o = .2f, pp = 1 - o;
        
        Transform trans = transform;
        Quaternion objectRot = trans.rotation;
        
        
        float step = 360f / ringCount * spread;
        Quaternion tilt = Quaternion.AngleAxis(twirl * 360f , Vector3.forward) * (Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.up));
        Quaternion sp = Quaternion.AngleAxis(spin * 360f, Vector3.up);
        for (int i = 0; i < ringCount; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(step * i, Vector3.up);
            RingState  rS  = states[i];
            TwirlState tS  = twirlStates[i];
            
            Vector3 p = rot * Vector3.forward * (radius + thickness + tS.distance);
                    p = transform.TransformPoint(p);
                    
                    
            float offset = offsets[i] * o;
            float lerp =Mathf.SmoothStep(0, 1,  Mathf.Max(0, toPositions - offset) / pp);
                    
            Transform goal = positions[i];
                    p = Vector3.Lerp(p, goal.position, lerp);
                    
            Quaternion b = goal.rotation * Quaternion.AngleAxis(180, Vector3.forward);
                     //  b = Quaternion.LookRotation(Vector3.forward, goal.up);
            Quaternion worldRot = Quaternion.Slerp(objectRot, b, lerp);
                  //worldRot = trans.rotation;
            Quaternion result = worldRot * (sp * (rot * (tilt * tS.twirl)));
            
            placements[i] = new Placement(p, result);
            rings[i].UpdateRing(p, result, thickness, rS.completion, rS.visibility);
        }
    }

    
    public void ToPositions(float toPositions)
    {
        this.toPositions = toPositions;
    }


    public void SetVis(float vis)
    {
        this.vis = vis;
    }
    
    
    protected override void UpdateRingStates()
    {
        for (int i = 0; i < ringCount; i++)
            states[i] = new RingState(completion, vis);
    }
    

    public Placement GetPlacement(int id)
    {
        return placements[id];
    }
}
