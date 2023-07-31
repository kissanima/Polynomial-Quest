using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSelectShoulder : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectShoulder() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectShoulder(index);
    }
}
