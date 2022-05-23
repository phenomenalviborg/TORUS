using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public KeyCode screenShotButton;
    
    private int id;
    void Update()
    {
        if (Input.GetKeyDown(screenShotButton))
        {
            ScreenCapture.CaptureScreenshot("screenshot" + id++ + ".png");
            Debug.Log("A screenshot was taken!");
        }
    }
}
