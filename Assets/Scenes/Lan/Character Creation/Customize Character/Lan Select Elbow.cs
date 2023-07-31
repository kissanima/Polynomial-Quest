using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lanSelectElbow : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectElbow() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectElbow(index);
    }
}
