using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    public GameObject loginPanel;

    public void ButtonPressed() {
        loginPanel.SetActive(true);
    }
}
