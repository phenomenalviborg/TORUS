using UnityEngine;


public class FloorTiles : MonoBehaviour
{
    private Transform cam;
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");
    
    private Vector3Force force = new Vector3Force(200).SetSpeed(7).SetDamp(3);

    private void Start()
    {
        GameObject tile = transform.GetChild(0).gameObject;
        
        const int range = 4;
        for (int x = -range; x < range + 1; x++)
        for (int z = -range; z < range + 1; z++)
            if(!(x == 0 && z == 0))
            {
                GameObject newTile = Instantiate(tile, transform);
                newTile.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 4) * 90, Vector3.up);
                newTile.transform.position = new Vector3(x, Random.Range(-.005f, 0), z) * .6f;
                newTile.transform.localScale = new Vector3(1, .75f, 1);
            }
        
        tile.transform.localScale = new Vector3(1, .75f, 1);
        
        cam = Camera.main.transform;
        force.SetValue(cam.position);
    }


    private void LateUpdate()
    {
        Shader.SetGlobalVector(HeadPos, force.Update(cam.position, Time.deltaTime));
    }
}
