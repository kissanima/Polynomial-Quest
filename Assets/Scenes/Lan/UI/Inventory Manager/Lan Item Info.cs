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
    [SerializeField] LanGameManager gmScript;
    public string itemType, itemClass;
    public int itemIndex;
    public float weaponDmg, armor;
    bool isEquipped;
    
    private void Awake() {
        //initialize components
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDamage = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemClassLabel = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        player = gmScript.player;
    }


    public void SetInfo(Sprite itemImage, Sprite itemImageWS, string itemName, string itemClass, string itemDamage, string armor, bool isEquipped) {
        this.isEquipped = isEquipped;
        this.itemClass = itemClass;
        this.itemImage.sprite = itemImage;
        this.itemImageWS = itemImageWS;
        this.itemName.SetText(itemName);
        itemClassLabel.SetText("CLASS: " + itemClass);

        if(itemClass == "armor") {
            this.itemDamage.SetText("Armor: " + armor);
        }
        else {
            this.itemDamage.SetText("Damage: " + itemDamage);
        }

        if(itemClass != "" && itemClass != gmScript.player.playerClass) {
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
            if(itemType == "sword") {
                player.weaponDmg -= weaponDmg;
                player.equipedWeaponIndex = 0;
                player.EquipItemServerRpc(0, player.NetworkObjectId, true);
            }
            else {
                player.finalArmor -= armor;
                player.equipedArmorIndex = 0;
                player.EquipItemServerRpc(0, player.NetworkObjectId, false);
            }
        }
        else {
            if(itemType == "sword") {
                player.weaponDmg = weaponDmg;
                player.equipedWeaponIndex = itemIndex;
                player.EquipItemServerRpc(itemIndex, player.NetworkObjectId, true);
            }
            else if(itemType == "armor") {
                player.itemArmor = armor;
                player.equipedArmorIndex = itemIndex;
                player.EquipItemServerRpc(itemIndex, player.NetworkObjectId, false);
            }
        }
        player.updateStats();
        UpdateUI();
        gmScript.SavePlayerData();
    }

    public void UpdateUI() {
        if(itemIndex == player.equipedWeaponIndex || itemIndex == player.equipedArmorIndex && isEquipped) {
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("UNQUIP");
        }
        else { //it means item clicked is not equiped
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("EQUIP");
        }
    }
}
