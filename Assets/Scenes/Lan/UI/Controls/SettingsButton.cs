using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSettingsButton : MonoBehaviour
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
