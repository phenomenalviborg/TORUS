using System.Collections;
using Shapes;
using UnityEngine;

public class WalkPath : ImmediateModeShapeDrawer 
{
    public Transform target;
    public float stopAt;
    public float walked;
    
    public bool flyingUp;
    
    private Vector3 pos;
    private Vector3 all;
    private int count;

    private PolylinePath p;
    
    private const float height = .05f;
    
    private Transform trans;
    private float y;
    
    private const float multi = 1.5f;
    
    
    private void Start()
    {
        p = new PolylinePath();
        trans = transform;
        
        Init();
    }


    private void Init()
    {
        trans.position = Vector3.zero;
        trans.rotation = Quaternion.identity;
        p.ClearAllPoints();
        pos = target.position.SetY(height);
        all = pos;
        count = 1;
        p.AddPoint(pos);
        walked = 0;
        y = 0;
    }
    

    private void OnDestroy()
    {
        p.Dispose();
    }


    private void Update()
    {
        if (!flyingUp)
        {
            Vector3 newPos = target.position.SetY(height);
            float sm = (newPos - pos).sqrMagnitude;
            const float dist = 1f / 100 * 1;
            const float thresh = dist * dist;
            if (sm > thresh)
            {
                walked += Mathf.Sqrt(sm);
                pos = newPos;
                p.AddPoint(pos);
                all += pos;
                count++;
            
                if(walked >= stopAt)
                    StartCoroutine(FlyUp());
            }
        }
        
    }


    private IEnumerator FlyUp()
    {
        flyingUp = true;
        Vector3 center = all / count;
        Vector3 offset = transform.position - center;
        
        Vector3 spinAxis = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * Vector3.forward;
        Quaternion rot = Quaternion.identity;
        
        float speed = 0;
        y = 0;
        while (y < 40)
        {
            speed += Time.deltaTime * .05f;
            y += speed * Time.deltaTime;
            
            Vector3 ps = center + Vector3.up * y;
            
            Vector3 d = rot * offset;
            
            trans.position = ps + d;
            trans.rotation = rot;
            
            rot      = Quaternion.AngleAxis(Time.deltaTime * y * 1.5f, spinAxis) * rot;
            spinAxis = Quaternion.AngleAxis(Time.deltaTime * 5, Vector3.up) * spinAxis;
            
            yield return null;
        }
        
        flyingUp = false;
        Init();
    }

    
    public override void DrawShapes( Camera cam ){
        using( Draw.Command( cam ) ){
            Draw.LineGeometry   = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness      = LineThickness.Width * multi;
            Draw.Matrix = transform.localToWorldMatrix;
            
            Draw.Polyline(p, Color.white.A(Mathf.Clamp01((20f - y) * .05f)));
        }
    }
}
