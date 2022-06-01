using UnityEngine;

public class HornTorus : MonoBehaviour
{
    public RingTorus ring;
    public HoopTorus hoop;
    
    public float radius;
    
    public float speed;
    
    public float t;
    
    
    private void Update()
    {
        t += Time.deltaTime * speed;
        float a = t;
        
        Vector3 v = Quaternion.AngleAxis(a, Vector3.forward) * Vector3.left * (radius - .5f);
        
        transform.localPosition = Vector3.up * v.y;
        
        ring.radius = v.x + (radius - .5f);
        hoop.radius = v.x + (radius - .5f);
        
        
        float l1 = radius * 2 * Mathf.PI;
        float l2 = .5f * 2 * Mathf.PI;
        
        float a2 = t * (l1 / l2);
        
        ring.spin = -a2 * .5f;
    }
}
