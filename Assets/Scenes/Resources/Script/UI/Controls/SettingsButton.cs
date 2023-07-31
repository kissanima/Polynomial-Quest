using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingPanel;

    public void ButtonPressed() {
        if(settingPanel.activeSelf) {
            settingPanel.SetActive(false);
        }
        else {
            settingPanel.SetActive(true);
        }
    }
}
