using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanFpsToggle : MonoBehaviour
{
    Toggle toggle;
    public CameraController cameraController;

    private void Start() {
        toggle = GetComponent<Toggle>();
        OnToggleChanged();
    }

    public void OnToggleChanged()
{
    if (toggle.isOn)
    {
        Application.targetFrameRate = 60; // Set the target frame rate to 60 FPS
        
    }
    else
    {
        Application.targetFrameRate = -1; // Use the default frame rate (unlimited)
       
    }
}
}
