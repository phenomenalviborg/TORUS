using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletCam : MonoBehaviour
{
    public static TabletCam Inst;
    
    public float smoothiness = 0.3f;
    public Camera camera;

    private void Awake()
    {
        Inst = this;
    }

    void OnEnable()
    {
        Application.targetFrameRate = 120;
    }

    

    // Update is called once per frame
    void LateUpdate()
    {
        var newPos = TabletCamPos.Inst.transform.position;
        transform.position = Vector3.Lerp(transform.position, newPos, smoothiness);

        var newRot = TabletCamPos.Inst.transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, smoothiness);
    }
}
