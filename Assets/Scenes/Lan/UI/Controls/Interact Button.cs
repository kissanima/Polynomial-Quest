using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanInteractButton : MonoBehaviour
{
    public GameObject interact;

    public void ButtonPressed() {
        interact.gameObject.SetActive(true);
    }
}
