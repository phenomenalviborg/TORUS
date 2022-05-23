using UnityEngine;
using Random = UnityEngine.Random;


public class FloorTiles : MonoBehaviour
{
    public Transform target;
    
    public Color tintColor;
    private Transform cam;
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");
    
    private readonly Vector3Force force = new Vector3Force(200).SetSpeed(7).SetDamp(3);
    
    private static readonly int DistMulti = Shader.PropertyToID("_DistMulti");
    private static readonly int Tint      = Shader.PropertyToID("_Tint");


    private void Start()
    {
        GameObject tile = transform.GetChild(0).gameObject;
        
        for (int x = 0; x < 16; x++)
        for (int z = 0; z < 28; z++)
        {
            GameObject newTile = Instantiate(tile, transform);
          
            newTile.transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 4) * 90, Vector3.up);
            newTile.transform.localPosition = new Vector3(x, Random.Range(-.005f, 0), z) * .6f + new Vector3(-7.5f, 0, -13.5f) * .6f;
            newTile.transform.localScale    = new Vector3(1, .75f, 1);
        }
        
        tile.SetActive(false);

        cam = Camera.main.transform;
        force.SetValue(target != null? target.position : cam.position);
    }


    private void LateUpdate()
    {
        Shader.SetGlobalVector(HeadPos, force.Update(target != null? target.position : cam.position, Time.deltaTime));
        Shader.SetGlobalFloat(DistMulti, 1f + Mathf.Sin(Time.realtimeSinceStartup * .6f) * .06f);
        Shader.SetGlobalColor(Tint, tintColor);
    }
}
