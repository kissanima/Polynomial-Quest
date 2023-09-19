using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanItemSS : MonoBehaviour
{
    public int itemIndex;
    public float damage = 25;
    public float armor;
    string itemName;
    public string itemClass = "", itemType = "";
    //public GameObject itemInfo;
    Sprite itemImage;
    public Sprite itemImageWS;
    [SerializeField] LanItemInfo itemInfo;
    [SerializeField] LanGameManager gmScript;

    private void Awake() {
        //itemIndex = transform.GetSiblingIndex() + 1;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        itemName = gameObject.name;
        itemImage = GetComponent<Image>().sprite;
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
