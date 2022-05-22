using UnityEngine;

[ExecuteInEditMode]
public class Orb : MonoBehaviour
{
    public float multi = 1;
    
    private Transform trans;
    private Transform cam;
    
    private void Start()
    {
        trans = transform;
        cam   = Camera.main.transform;
    }

    
    private void LateUpdate()
    {
        Vector3 pos  = trans.position;
        float   dist = (pos - cam.position).magnitude;
        
        float s = 2f * dist * .0011f * LineThickness.Width * multi;
        trans.localScale = Vector3.one * s;
    }
}
