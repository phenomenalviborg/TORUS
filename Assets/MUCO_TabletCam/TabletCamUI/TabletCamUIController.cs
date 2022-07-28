using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletCamUIController : MonoBehaviour
{
    public TabletCamUIObject lensText { get; set; }
    public TabletCamUIObject videoDebugText { get; set; }

    public TextMeshProUGUI feedbackText => videoDebugText != null ? videoDebugText.text : null;

    public GameObject hideUIWhileRecording;


    public Vector2 fovRange = new Vector2(5, 80);

    public Vector2 smoothnessRange = new Vector2(1, 10f);

    public bool isRecording { get; private set; }

    void Start()
    {
    }

    public void StartRecording()
    {
        if (isRecording)
            return;

        isRecording = true;
        //screenRecorderExample.StartRecording();
        hideUIWhileRecording.SetActive(false);
    }

    public void HideUI()
    {
        hideUIWhileRecording.SetActive(false);
    }

    public void StopRecording()
    {
        hideUIWhileRecording.SetActive(true);

        if (!isRecording)
            return;

        isRecording = false;
        //screenRecorderExample.StopRecording();
    }


    public void SetFov(float val01)
    {
        var valueInRange = Mathf.Lerp(fovRange.x, fovRange.y, val01);
        TabletCam.Inst.camera.fieldOfView = valueInRange;
        SetFeedbackText("FOV: " + TabletCam.Inst.camera.fieldOfView);
    }

    private void SetFeedbackText(string msg)
    {
        if (feedbackText != null)
            feedbackText.text = msg;

        StopAllCoroutines();
        StartCoroutine(ClearVideoFeedbackText());
    }

    private IEnumerator ClearVideoFeedbackText()
    {
        yield return new WaitForSeconds(3f);
        if (feedbackText != null)
            feedbackText.text = "";
    }


    public void SetSmooth(float val01)
    {
        var valueInRange = Mathf.Lerp(smoothnessRange.x, smoothnessRange.y, val01);

        TabletCam.Inst.smoothiness = 1f / (valueInRange * valueInRange);

        SetFeedbackText("Smooth: " + TabletCam.Inst.smoothiness);
    }

    public void PreviewVideo()
    {
        //screenRecorderExample.PreviewVideo();
    }
}