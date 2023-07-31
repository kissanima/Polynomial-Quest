using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSelectBelt : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectBelt() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectBelt(index);
    }
}
