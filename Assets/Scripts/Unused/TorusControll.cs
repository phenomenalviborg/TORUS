using UnityEngine;


public class TorusControll : MonoBehaviour
{
    public GameObject[] g;
    
    private int pick;
    
    private Material[] mats;
    private int count;
    private static readonly int radius = Shader.PropertyToID("_Radius");
    private static readonly int thickness = Shader.PropertyToID("_Thickness");
    
    private float thick = .8f, tilt = 0;
    private readonly FloatForce ff = new FloatForce(100, 11).SetValue(.8f);
    private readonly FloatForce tf = new FloatForce(10, 3).SetValue(.8f);
    
    
    private void Start()
    {
        MeshRenderer[] mRs = GetComponentsInChildren<MeshRenderer>();
        
        count = mRs.Length;
        mats = new Material[count];
        for(int i = 0; i < count; i++)
        {
            MeshRenderer mR = mRs[i];
            Material m = Instantiate(mR.material);
            mR.material = m;
            mats[i] = m;
        }
        
        SetVis();
    }

    
    private void Update()
    {
        thick = Mathf.Clamp01(thick + Time.deltaTime * -Input.GetAxis("Horizontal"));
        float t = ff.Update(thick, Time.deltaTime);

        for (int i = 0; i < count; i++)
        {
            Material m = mats[i];
            m.SetFloat(radius,    .5f);
            m.SetFloat(thickness, t);
        }   
        
        tilt = Mathf.Clamp(tilt + Time.deltaTime * -Input.GetAxis("Vertical") * 20, -5, 5);
        float a = tf.Update(tilt, Time.deltaTime);
        
        transform.rotation = Quaternion.AngleAxis(a, Vector3.right);


        if (Input.GetButtonDown("Fire1"))
        {
            pick++;
            SetVis();
        }
    }


    private void SetVis()
    {
        int l = g.Length;
        for (int i = 0; i < l ; i++)
            g[i].SetActive(i == pick % l);
    }
}
