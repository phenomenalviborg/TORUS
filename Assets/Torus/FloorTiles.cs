using UnityEngine;


public class FloorTiles : MonoBehaviour
{
    private Transform cam;
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");
    
    private readonly Vector3Force force = new Vector3Force(200).SetSpeed(7).SetDamp(3);
    
    private const int xTiles = 16, zTiles = 32;

    private void Start()
    {
        GameObject tile = transform.GetChild(0).gameObject;
        
        const int xRange = 4, zRange = 4;
        //const int xRange = 8, zRange = 16;
        
        for (int x = -xRange; x < xRange + 1; x++)
        for (int z = -zRange; z < zRange + 1; z++)
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
