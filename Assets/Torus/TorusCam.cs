using UnityEngine;


public class TorusCam : MonoBehaviour
{
    private float angle;
    private readonly FloatForce ff = new FloatForce(10, 6);
    
    
    private void Update()
    {
        angle = Mathf.Clamp(angle + Time.deltaTime * -Input.GetAxis("LR") * 90, -50, 50);
        float a = ff.Update(angle, Time.deltaTime);
        
        transform.localRotation = Quaternion.AngleAxis(a, Vector3.up);
    }
}
