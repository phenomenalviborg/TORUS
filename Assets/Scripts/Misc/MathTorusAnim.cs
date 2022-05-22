using UnityEngine;


public class MathTorusAnim : MonoBehaviour
{
    public float spinSpeed;
    public float animSpeed;
    
    [Space]
    public bool autoLoop;
    
    private Material copyMat;
    private float spin, anim;
    private static readonly int Spin = Shader.PropertyToID("_Spin");
    private static readonly int Radius = Shader.PropertyToID("_Radius");
    private static readonly int Anim = Shader.PropertyToID("_Anim");


    private void Start()
    {
        MeshRenderer mR = GetComponent<MeshRenderer>();
        copyMat = Instantiate(mR.material);
        mR.material = copyMat;
        
        //copyMat.SetFloat(Radius, 0);
    }

    
    private void Update()
    {
        spin += Time.deltaTime * spinSpeed;
        
        
        anim += Time.deltaTime * animSpeed;
        if(autoLoop && anim >= 1.2f)
            anim -= 1.2f;
        
        if(Input.GetKey(KeyCode.LeftControl))
            anim = 0;
        if(Input.GetKey(KeyCode.LeftAlt))
            spin = 0;
        
        copyMat.SetFloat(Spin, spin);
        copyMat.SetFloat(Anim, anim);
    }
}
