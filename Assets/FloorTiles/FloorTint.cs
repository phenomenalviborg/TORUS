using UnityEngine;


public class FloorTint : MonoBehaviour
{
    public Transform target;
    
    public Color tintColor;
    
    [Range(0, 1)]
    public float tintAmount;
    
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");
    
    private readonly Vector3Force force = new Vector3Force(200).SetSpeed(7).SetDamp(3);
    
    private static readonly int DistMulti = Shader.PropertyToID("_DistMulti");
    private static readonly int Tint      = Shader.PropertyToID("_Tint");
    private static readonly int TintAmount = Shader.PropertyToID("_TintAmount");
    
    private Transform cam;
    
    
    private void Start()
    {
        cam = Camera.main.transform;
        force.SetValue(target != null? target.position : cam.position);
    }
    
    
    private void LateUpdate()
    {
        Shader.SetGlobalVector(HeadPos, force.Update(target != null? target.position : cam.position, Time.deltaTime));
        Shader.SetGlobalFloat(DistMulti, 1f + Mathf.Sin(Time.realtimeSinceStartup * .6f) * .06f);
        Shader.SetGlobalColor(Tint, tintColor);
        Shader.SetGlobalFloat(TintAmount, tintAmount);
    }
}
