using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletCamUIObject : MonoBehaviour
{
    public enum TabletCamTypes
    {
        LensDown = 0,
        LensUp = 1,
        Fov = 2,
        Smooth = 3,
        Record = 4,
        StopRecord = 5,
        LensText = 6,
        PreviewVideo = 7,
        PreviewVideoText = 8,
        HideUI = 9,
    }

    public TabletCamTypes tabletCamType;

    public Button button;
    public Slider slider;
    public TextMeshProUGUI text;

    public TabletCamUIController controller;

    private void OnValidate()
    {
        if (controller == null)
            controller = GetComponentInParent<TabletCamUIController>();
        var butcom = GetComponent<Button>();
        button = butcom;
        var slicom = GetComponent<Slider>();
        slider = slicom;
        var texcom = GetComponent<TextMeshProUGUI>();
        text = texcom;
    }

    private void OnEnable()
    {
        if (tabletCamType == TabletCamTypes.LensText)
            controller.lensText = this;
        if (tabletCamType == TabletCamTypes.PreviewVideoText)
        {
            controller.videoDebugText = this;
            //controller.screenRecorderExample.savedPathTMP = this.text;
        }


        if (button != null)
            button.onClick.AddListener(OnClick);
        if (slider != null)
            slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnClick()
    {
        switch (tabletCamType)
        {
            case TabletCamTypes.Record:
                controller.StartRecording();
                break;
            case TabletCamTypes.StopRecord:
                controller.StopRecording();
                break;
            case TabletCamTypes.PreviewVideo:
                controller.PreviewVideo();
                break;
            case TabletCamTypes.HideUI:
                controller.HideUI();
                break;
        }
    }

    private void OnValueChanged(float val01)
    {
        switch (tabletCamType)
        {
            case TabletCamTypes.Fov:
                controller.SetFov(val01);
                break;
            case TabletCamTypes.Smooth:
                controller.SetSmooth(val01);
                break;
        }
    }

    public void SetLensText(string lensText)
    {
        text.text = lensText;
    }
}