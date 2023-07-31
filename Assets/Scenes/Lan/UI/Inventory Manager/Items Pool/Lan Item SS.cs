using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanItemSS : MonoBehaviour
{
    int itemIndex;
    public float damage = 25;
    public float armor;
    string itemName;
    public string itemClass = "", itemType = "";
    //public GameObject itemInfo;
    LanItemInfo itemInfo;
    Sprite itemImage;
    public Sprite itemImageWS;
    LanGameManager gmScript;

    private void Awake() {
        itemIndex = transform.GetSiblingIndex() + 1;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        itemName = gameObject.name;
        itemImage = GetComponent<Image>().sprite;
        itemInfo = GameObject.FindWithTag("InventoryManager").transform.GetChild(2).GetComponent<LanItemInfo>();
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }

    private void OnEnable() {
        if(itemIndex == gmScript.player.equipedWeaponIndex) {
            transform.GetChild(0).gameObject.SetActive(true);

            //set stats
            gmScript.player.weaponDmg = damage;
            gmScript.player.updateStats();
        }
        else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void ButtonPressed() {
            itemInfo.gameObject.SetActive(true);
            itemInfo.itemType = itemType;
            itemInfo.itemIndex = itemIndex;

            itemInfo.SetInfo(itemImage, itemImageWS, itemName, itemClass, damage.ToString());

            if(itemType == "sword") {
                itemInfo.weaponDmg = damage;
            }
            else if(itemType == "armor") {
                itemInfo.armor = armor;
            }
    }
}
