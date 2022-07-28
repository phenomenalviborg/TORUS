using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class TabletCamAvatar : MonoBehaviour
{
    public RealtimeView _realtimeView;
    
    void OnEnable()
    {
        if (_realtimeView == null) _realtimeView = GetComponent<RealtimeView>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (_realtimeView.isOwnedLocallySelf)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
            transform.position = TabletCam.Inst.transform.position;
            transform.rotation = TabletCam.Inst.transform.rotation;
        }
    }
}