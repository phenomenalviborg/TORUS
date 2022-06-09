using UnityEngine;

[ExecuteInEditMode]
public class LineThickness : MonoBehaviour
{
    public float width;
    public float growth;

    private static readonly int LineS = Shader.PropertyToID("_LineS");
    private static readonly int LineG = Shader.PropertyToID("_LineG");

    public static float Width;
    public static float Growth;
    
    private void Update()
    {
        Width = width;
        Shader.SetGlobalFloat(LineS, width);
        Growth = growth;
        Shader.SetGlobalFloat(LineG, growth);
    }
}
