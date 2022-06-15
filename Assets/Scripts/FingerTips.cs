using UnityEngine;

public class FingerTips : MonoBehaviour
{
    public Transform head;
    public SkinnedMeshRenderer smr;
    private MeshRenderer mR;


    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }

    
    private void LateUpdate()
    {
        Vector3 dir = head.position - transform.position;
        float mag = dir.sqrMagnitude;
        const float thresh = .9f * .9f;
        bool showit = smr.enabled && mag < thresh;
        if(mR.enabled != showit)
            mR.enabled = showit;
    }
}
