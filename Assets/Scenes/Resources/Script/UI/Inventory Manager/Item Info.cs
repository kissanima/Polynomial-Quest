using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    Image itemImage;
    TextMeshProUGUI itemName, itemDamage;
    public Player player;
    public string itemType;
    public int weaponDmg, itemIndex;
    public int armor;
    private void Awake() {
        //initialize components
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDamage = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetInfo(Sprite itemImage, string itemName, string itemDamage) {
        this.itemImage.sprite = itemImage;
        this.itemName.SetText(itemName);
        this.itemDamage.SetText(itemDamage);
    }

    public void UpdateUI() {
        if(itemIndex == player.equipedSwordIndex || itemIndex == player.equipedArmorIndex) {
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("UNQUIP");
        }
        else { //it means item clicked is not equiped
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("EQUIP");
        }
    }

    public void itemEquip() {
        if(itemIndex == player.equipedSwordIndex || itemIndex == player.equipedArmorIndex) {
            player.weaponDmg -= weaponDmg;
            player.equipedSwordIndex = 0;
            Debug.Log("item unequip");
        }
        else {
            Debug.Log("item equip");
        if(itemType == "sword") {
            player.weaponDmg = weaponDmg;
            player.equipedSwordIndex = itemIndex;
        }
        else if(itemType == "armor") {
            player.itemArmor = armor;
            player.equipedArmorIndex = itemIndex;
        }
        }
        player.updateStats();
        UpdateUI();
    }
}
