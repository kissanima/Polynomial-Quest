using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public GameObject inventoryManager, itemInfo;

    public void ButtonPressed() {
        if(inventoryManager.activeSelf) {
            itemInfo.SetActive(false);
            inventoryManager.SetActive(false);
        }
        else {
            inventoryManager.SetActive(true);
        }
    }
}
