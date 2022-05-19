using UnityEngine;


public class MathTorusAnim : MonoBehaviour
{
    public float spinSpeed;
    public float animSpeed;
    
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
        
        copyMat.SetFloat(Radius, 0);
    }

    
    private void Update()
    {
        spin += Time.deltaTime * spinSpeed;
        copyMat.SetFloat(Spin, spin);
        
        anim += Time.deltaTime * animSpeed;
        if(anim >= 1.2f)
            anim -= 1.2f;
        
        copyMat.SetFloat(Anim, anim);
    }
}
