using UnityEngine;

[ExecuteInEditMode]
public class LineThickness : MonoBehaviour
{
    public float width;

    private static readonly int LineS = Shader.PropertyToID("_LineS");

    public static float Width;
    
    private void Update()
    {
        Width = width;
        Shader.SetGlobalFloat(LineS, width);
    }
}
