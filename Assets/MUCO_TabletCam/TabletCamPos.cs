using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antilatency.SDK;
using ATVR;
using UnityEngine;
using UnityEngine.XR;

public class TabletCamPos : MonoBehaviour
{
    public static TabletCamPos Inst;

    public AltTrackingUsbSocket socket;

    private void Awake()
    {
        Inst = this;
    }

    void OnValidate()
    {
        if(socket.Network == null)
        {
            socket.Network = FindObjectOfType<DeviceNetwork>();
            socket.Environment = FindObjectOfType<AltEnvironmentComponent>();
        }
    }
    
    
    public bool IsVREnabled()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
 
        SubsystemManager.GetInstances<XRDisplaySubsystem>(displaySubsystems);

        //return true;
        return displaySubsystems.Any(s => s.running);
    }


    void Start()
    {
       
        if(!IsVREnabled())
            VRInput.Head.parent.gameObject.SetActive(false);
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
