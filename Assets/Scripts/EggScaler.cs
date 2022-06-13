using UnityEngine;

public class EggScaler : MonoBehaviour
{
    public Transform head;
    
    private void LateUpdate()
    {
        transform.position = head.position.SetY(0);
        transform.localScale = new Vector3(1, (head.position.y + .2f) * .5f, 1);
    }
}
