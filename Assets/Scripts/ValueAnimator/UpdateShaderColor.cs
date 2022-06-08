using UnityEngine;

public class UpdateShaderColor : MonoBehaviour
{
    public Color color;
    
    public string colorName;

    public void SetColor(Color color)
    {
        Shader.SetGlobalColor(id, color);
    }
    
    private int id;
    private void Start()
    {
        id = Shader.PropertyToID(colorName);
        Shader.SetGlobalColor(id, color);
    }
}
