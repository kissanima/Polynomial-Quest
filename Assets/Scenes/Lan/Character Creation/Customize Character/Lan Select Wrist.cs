using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanSelectWrist : MonoBehaviour
{
    LanCreateCharacter createCharacter;
    int index;

    private void Start() {
        createCharacter = GameObject.FindWithTag("CharacterCreation").GetComponent<LanCreateCharacter>();
    }
    public void SelectWrist() {
        index = transform.GetSiblingIndex();
        createCharacter.SelectWrist(index);
    }
}
