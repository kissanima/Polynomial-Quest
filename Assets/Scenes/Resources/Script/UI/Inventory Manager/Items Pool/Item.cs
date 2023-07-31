using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public int itemIndex = 1;
    public int damage = 25;
    public int armor;
    public string itemName = "sword Lv1";
    //public GameObject itemInfo;
    LanItemInfo itemInfo;
    Sprite itemImage;
    public string itemType;

    private void Start() {
        itemImage = GetComponent<Image>().sprite;
        itemInfo = GetComponent<LanItemInfo>();
    }
    public void ButtonPressed() {
        itemInfo.gameObject.SetActive(true);
        //itemInfo.SetInfo(itemImage, itemName, damage.ToString());
        itemInfo.itemType = itemType;
        itemInfo.itemIndex = itemIndex;
        if(itemType == "sword") {
            itemInfo.weaponDmg = damage;
        }
        else if(itemType == "armor") {
            itemInfo.armor = armor;
        }
    }
}
