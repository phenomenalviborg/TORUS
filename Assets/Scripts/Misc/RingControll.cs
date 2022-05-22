using UnityEngine;

//[ExecuteInEditMode]
public class RingControll : MonoBehaviour
{
    public float anim;
    public float vis = 1;
    
    private MeshRenderer mR;
    private MaterialPropertyBlock block;
    
    private Transform trans;
    
    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
        mR.GetPropertyBlock(block);
        trans = transform;
    }

    
    private void LateUpdate()
    {
        if (block != null)
        {
            block.SetFloat("_Anim", anim);
            block.SetFloat("_Vis", vis);
            mR.SetPropertyBlock(block);
        }
    }


    public void UpdateRing(Vector3 pos, Quaternion rot, float scale, float anim, float vis, bool local = false)
    {
        if (local)
        {
            trans.localPosition = pos;
            trans.localRotation = rot;
        }
        else
        {
            trans.position = pos;
            trans.rotation = rot;
        }
            
        trans.localScale = Vector3.one * scale;
        this.anim = anim;
        this.vis = vis;
    }


    private static GameObject dummy;
    public static GameObject Dummy
    {
        get
        {
            if(dummy == null)
                dummy = Resources.Load<GameObject>("Ring");
            
            return dummy;
        }
    }
}
