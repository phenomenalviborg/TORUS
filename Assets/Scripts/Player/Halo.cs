using UnityEngine;


public class Halo : MonoBehaviour
{
    public Transform head;
    public float height;
    
    private readonly Vector3Force force = new Vector3Force(200).SetSpeed(220).SetDamp(17);
    
    
    private void Start()
    {
        force.SetValue(head.position + Vector3.up * height * .01f);
    }

    
    private void LateUpdate()
    {
        transform.position = force.Update(head.position + Vector3.up * height * .01f, Time.deltaTime);
    }
}
