using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingToHoop : MonoBehaviour
{
    public CopyRingTorus target;
    
    private RingControll rC;
    
    private Vector3 pos, mV;
    
    private void Start()
    {
        rC = GetComponent<RingControll>();
    }

    private void Update()
    {
        if (target.copyThis != null)
        {
            Vector3 newPos = target.transform.position;
            mV = Vector3.Lerp(mV, (newPos - pos) / Time.deltaTime, Time.deltaTime * 6);
            pos = newPos;
            
            transform.position   = pos;
            transform.rotation   = target.transform.rotation;
            transform.localScale = target.transform.localScale;
            
            //rC.s
        }
    }
}
