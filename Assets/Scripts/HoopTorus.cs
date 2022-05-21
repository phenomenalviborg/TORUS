using UnityEngine;

public class HoopTorus : AnimTorus
{
    [Range(0, 1)]
    public float completion;
    [Range(0, 1)]
    public float spread;
    
    [Range(0, 1)]
    public float twirl;
    
    private void Start()
    {
        CreateRings();
    }

    
    private void Update()
    {
        float step = 360f / ringCount * spread;
        Quaternion tilt = Quaternion.AngleAxis(twirl * 360f , Vector3.forward) * (Quaternion.AngleAxis(90, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.up));
        for (int i = 0; i < ringCount; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(step * i, Vector3.up);
            rings[i].UpdateRing(rot * Vector3.forward * (radius + thickness), rot * tilt, thickness, 1.2f * completion, 1, true);
        }
    }
}
