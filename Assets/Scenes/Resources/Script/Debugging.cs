using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugging : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public GameObject debugPanel;

    public void ButtonPressed() {
        if(debugPanel.activeSelf) {
            debugPanel.SetActive(false);
        }
        else {
            debugPanel.SetActive(true);
        }
    }
}
