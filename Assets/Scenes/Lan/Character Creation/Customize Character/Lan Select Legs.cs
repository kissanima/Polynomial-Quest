using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSelectLegs : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectLegs() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectLegs(index);
    }
}
