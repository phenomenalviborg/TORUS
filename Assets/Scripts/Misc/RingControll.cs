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
        mR.enabled = false;
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
        bool shouldBeVisible = anim * vis > .0001f;

        if (shouldBeVisible && CameraPosition.Distance(local ? trans.parent.TransformPoint(pos) : pos) - scale * 2 > 28)
            shouldBeVisible = false;
        
        if (mR.enabled != shouldBeVisible)
        {
            mR.enabled = shouldBeVisible;
            
            if(shouldBeVisible)
                ActiveRings.Add(this);
            else
                ActiveRings.Remove(this);
        }
            
        
        if(!shouldBeVisible)
            return;
        
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
        this.vis  = vis;
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
