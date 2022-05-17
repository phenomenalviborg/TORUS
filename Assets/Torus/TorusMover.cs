using UnityEngine;

public class TorusMover : MonoBehaviour
{
    public int row;
    public float radius, thickness;
    
    public float speed;
    private float t, t2;
    
    private bool reverse;


    private void Start()
    {
        t = Random.Range(0, 360f);
        speed *= Random.Range(.75f, 1.25f);
        row = Random.Range(0, 44);
        reverse = Random.Range(0, 2) == 1;
    }


    private void Update()
    {
        t  -= Time.deltaTime * speed * .2f;
        t2 -= Time.deltaTime * 1.21f * .01f * 360 * 2;
        Vector3 tP = Quaternion.AngleAxis(360f / 44 * row + t + t2, Vector3.forward) * Vector3.right * thickness * .25f + Vector3.right * radius * .5f;
        transform.position = Quaternion.AngleAxis(t * (reverse? 1 : -1), Vector3.up) * tP * 10;
    }
}
