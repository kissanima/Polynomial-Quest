using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanItemInfo : MonoBehaviour
{
    Image itemImage;
    Sprite itemImageWS;
    TextMeshProUGUI itemName, itemDamage, itemClassLabel;
    LanPlayer player;
    LanGameManager gmScript;
    public string itemType, itemClass;
    public int itemIndex;
    public float weaponDmg, armor;
    private void Awake() {
        //initialize components
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDamage = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemClassLabel = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        player = gmScript.player;
    }

    public void SetInfo(Sprite itemImage, Sprite itemImageWS, string itemName, string itemClass, string itemDamage) {
        this.itemClass = itemClass;
        this.itemImage.sprite = itemImage;
        this.itemImageWS = itemImageWS;
        this.itemName.SetText(itemName);
        this.itemDamage.SetText("Damage: " + itemDamage);
        itemClassLabel.SetText("CLASS: " + itemClass);

        if(itemClass != gmScript.player.playerClass) {
            transform.GetChild(3).gameObject.SetActive(false);
            itemClassLabel.color = Color.red;
        }
        else {
            transform.GetChild(3).gameObject.SetActive(true);
            itemClassLabel.color = Color.white;
        }

        UpdateUI();
    }

    public void itemEquip() {
        if(itemIndex == player.equipedWeaponIndex || itemIndex == player.equipedArmorIndex) {
            player.weaponDmg -= weaponDmg;
            player.equipedWeaponIndex = 0;
            Debug.Log("item unequip");
        }
        else {
            Debug.Log("item equiped");

            //set item sprite to player
            gmScript.player.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = itemImageWS;
            
            if(itemType == "sword") {
                player.weaponDmg = weaponDmg;
                player.equipedWeaponIndex = itemIndex;
            }
            else if(itemType == "armor") {
                player.itemArmor = armor;
                player.equipedArmorIndex = itemIndex;
            }
        }
        player.updateStats();
        UpdateUI();
        gmScript.SavePlayerData();
    }

    public void UpdateUI() {
        if(itemIndex == player.equipedWeaponIndex || itemIndex == player.equipedArmorIndex) {
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("UNQUIP");
        }
        else { //it means item clicked is not equiped
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("EQUIP");
        }
    }
}
