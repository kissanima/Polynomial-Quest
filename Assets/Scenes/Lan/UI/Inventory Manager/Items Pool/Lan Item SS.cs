using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanItemSS : MonoBehaviour
{
    public int itemIndex, itemIndexAtInventory;
    public float damage = 25;
    public float armor;
    string itemName;
    public string itemClass = "", itemType = "";
    public bool isEquipped;
    //public GameObject itemInfo;
    Sprite itemImage;
    public Sprite itemImageWS;
    [SerializeField] LanItemInfo itemInfo;
    [SerializeField] LanGameManager gmScript;
    [SerializeField] TextMeshProUGUI damageArmorLabel;

    private void Awake() {
        //itemIndex = transform.GetSiblingIndex() + 1;
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        itemName = gameObject.name;
        itemImage = GetComponent<Image>().sprite;

    }
   
    public void ButtonPressed() {
            itemInfo.gameObject.SetActive(true);
            itemInfo.itemType = itemType;
            //itemInfo.itemIndex = itemIndex;
            itemInfo.SetInfo(itemImage, itemImageWS, itemName, itemClass, damage.ToString(), armor.ToString(), isEquipped, this);

            if(itemType == "sword") {
                itemInfo.weaponDmg = damage;
                damageArmorLabel.SetText("Damage: " + damage);
            }
            else if(itemType == "armor") {
                itemInfo.armor = armor;
                damageArmorLabel.SetText("armor: " + armor);
            }
    }
}
