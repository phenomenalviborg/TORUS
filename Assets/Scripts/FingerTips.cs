using UnityEngine;

public class FingerTips : MonoBehaviour
{
    public SkinnedMeshRenderer smr;
    private MeshRenderer mR;


    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }

    
    private void LateUpdate()
    {
        if(mR.enabled != smr.enabled)
            mR.enabled = smr.enabled;
    }
}
