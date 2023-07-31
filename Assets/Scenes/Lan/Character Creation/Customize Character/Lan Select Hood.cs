using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSelectHood : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectHood() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectHood(index);
    }
}
