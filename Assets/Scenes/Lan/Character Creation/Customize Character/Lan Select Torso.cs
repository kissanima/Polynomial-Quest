using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lanSelectTorso : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectTorso() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectTorso(index);
    }
}
