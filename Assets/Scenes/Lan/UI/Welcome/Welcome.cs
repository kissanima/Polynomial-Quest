using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanWelcome : MonoBehaviour
{
    public GameObject loginPanel;

    public void ButtonPressed() {
        loginPanel.SetActive(true);
    }
}
