using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidTabletPermissions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject =
            unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intentObject = new AndroidJavaObject(
            "android.content.Intent", "android.settings.MANAGE_ALL_FILES_ACCESS_PERMISSION"))
        {
            currentActivityObject.Call("startActivity", intentObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
