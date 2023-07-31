using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public GameObject characterPanel;
    public void ButtonPressed() {
        if(characterPanel.activeSelf) {
            characterPanel.SetActive(false);
        }
        else {
            characterPanel.SetActive(true);
        }

    }
}
