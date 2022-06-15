using UnityEngine;

public class HandTips : MonoBehaviour
{
    public Transform head;
    public SkinnedMeshRenderer smr;
    
    public Transform[] realTips;
    private Transform[] myTips;
    
    public float palmOffset;
    private MeshRenderer[] mrs;
    
    private int count;
    
    
    private void Start()
    {
        count = realTips.Length;
        myTips = new Transform[count];
        mrs = new MeshRenderer[count];
        for (int i = 0; i < count; i++)
        {
            myTips[i] = transform.GetChild(i);
            mrs[i] = myTips[i].GetComponent<MeshRenderer>();
        } 
    }


    private void LateUpdate()
    {
        Vector3 h = head.position;
        for (int i = 0; i < 6; i++)
        {
            Transform a = realTips[i];
            Transform b = myTips[i];
            
            Vector3 dir = h - a.position;
            float mag = dir.sqrMagnitude;
            const float thresh = .9f * .9f;
            bool showit = smr.enabled && mag < thresh;
            
            MeshRenderer mR = mrs[i];
            
            if(mR.enabled != showit)
                mR.enabled = showit;

            if (showit)
            {
                Quaternion rot = a.rotation;
                b.position = a.position + rot * Vector3.forward * (i < 5? 0 : palmOffset);
                b.rotation = rot;
            }
        }
     
    }
}
