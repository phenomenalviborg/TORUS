using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private Transform trans;
    private static Vector3 pos;
    
    
    private void Start()
    {
        trans = transform;
        pos = trans.position;
    }

   
    private void LateUpdate()
    {
        pos = trans.position;
    }


    public static float Distance(Vector3 p)
    {
        return (p - pos).magnitude;
    }
}
