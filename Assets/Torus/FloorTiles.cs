using UnityEngine;


public class FloorTiles : MonoBehaviour
{
    private Transform cam;
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");

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
            }
        
        cam = Camera.main.transform;
    }


    private void LateUpdate()
    {
        Shader.SetGlobalVector(HeadPos, cam.position);
    }
}
