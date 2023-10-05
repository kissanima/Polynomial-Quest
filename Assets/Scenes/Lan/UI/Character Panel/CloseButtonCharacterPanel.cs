using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonCharacterPanel : MonoBehaviour
{
    public void ButtonPressed() {
        transform.parent.gameObject.SetActive(false);
    }
}
