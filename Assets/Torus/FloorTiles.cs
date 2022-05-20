using UnityEngine;


public class FloorTiles : MonoBehaviour
{
    private Transform cam;
    private static readonly int HeadPos = Shader.PropertyToID("HeadPos");
    
    private readonly Vector3Force force = new Vector3Force(200).SetSpeed(7).SetDamp(3);
    
    private const int xTiles = 16, zTiles = 32;
    
    
    private Vector3[] position, scale;
    private Quaternion[] rots;
    private Matrix4x4[] mats;
    
    private ComputeBuffer buffer;
    
    private int count;

    
    private void Start()
    {
        GameObject tile = transform.GetChild(0).gameObject;
        
        const int xRange = 4, zRange = 4;
        //const int xRange = 8, zRange = 16;
        count    = (xRange * 2 + 1) * (zRange * 2 + 1);
        position = new    Vector3[count];
        scale    = new    Vector3[count];
        rots     = new Quaternion[count];
        mats     = new  Matrix4x4[count];
        
        buffer = new ComputeBuffer(count, 4 * 4 * 4);
        
        int index = 0;
        for (int x = -xRange; x < xRange + 1; x++)
        for (int z = -zRange; z < zRange + 1; z++)
        {
                rots[index] = Quaternion.AngleAxis(Random.Range(0, 4) * 90, Vector3.up);
            position[index] = new Vector3(x, Random.Range(-.005f, 0), z) * .6f;
               scale[index] = new Vector3(1, .75f, 1);

            GameObject newTile = !(x == 0 && z == 0)? Instantiate(tile, transform) : tile;
          
            newTile.transform.rotation   = rots[index];
            newTile.transform.position   = position[index];
            newTile.transform.localScale = scale[index];
            
            index++;
        }
        
        cam = Camera.main.transform;
        force.SetValue(cam.position);
        
        UpdateMats();
    }


    private void UpdateMats()
    {
        Quaternion r = transform.rotation;
         Matrix4x4 m = transform.localToWorldMatrix;
         for (int i = 0; i < count; i++)
             mats[i] = Matrix4x4.TRS(m.MultiplyPoint(position[i]), r * rots[i], scale[i]);
         
        buffer.SetData(mats);
    }


    private void LateUpdate()
    {
        if(transform.hasChanged)
            UpdateMats();
        
        Shader.SetGlobalVector(HeadPos, force.Update(cam.position, Time.deltaTime));
    }


    private void OnDisable()
    {
        if(buffer != null)
            buffer.Dispose();
    }
}
