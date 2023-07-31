using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanBag : MonoBehaviour
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
