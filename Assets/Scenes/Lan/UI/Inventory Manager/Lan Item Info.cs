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
    LanItemSS itemClicked;
    [SerializeField] LanGameManager gmScript;
    public string itemType, itemClass;
    public int itemIndex;
    public float weaponDmg, armor;
    bool isEquipped;
    Transform inventoryPanel;
    
    private void Awake() {
        //initialize components
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDamage = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemClassLabel = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        inventoryPanel = transform.parent.GetChild(0);
        player = gmScript.player;
    }


    public void SetInfo(Sprite itemImage, Sprite itemImageWS, string itemName, string itemClass, string itemDamage, string armor, bool isEquipped, LanItemSS itemClicked) {
        this.isEquipped = isEquipped;
        this.itemClass = itemClass;
        this.itemImage.sprite = itemImage;
        this.itemImageWS = itemImageWS;
        this.itemName.SetText(itemName);
        this.itemClicked = itemClicked;
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
        if(itemClicked.transform.GetSiblingIndex() + 1 == player.weaponIndexAtInventory || itemClicked.transform.GetSiblingIndex() + 1 == player.armorIndexAtInventory) {
            if(itemType == "sword") { //unequipped weapon
                itemClicked.isEquipped = false;
                player.weaponDmg -= weaponDmg;
                player.equipedWeaponIndex = 0;
                player.weaponIndexAtInventory = 0;
                player.EquipItemServerRpc(0, player.NetworkObjectId, true);

                //show equipped status
                itemClicked.transform.GetChild(0).gameObject.SetActive(false);
                

            }
            else { //unequipped armor
                itemClicked.isEquipped = false;
                player.finalArmor -= armor;
                player.equipedArmorIndex = 0;
                player.armorIndexAtInventory = 0;
                player.EquipItemServerRpc(0, player.NetworkObjectId, false);

                //show equipped status
                itemClicked.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else { //equip item
            if(itemType == "sword") {
                itemClicked.isEquipped = true;
                player.weaponDmg = weaponDmg;
                player.equipedWeaponIndex = itemIndex;
                player.EquipItemServerRpc(itemIndex, player.NetworkObjectId, true);

                //show equipped status
                itemClicked.transform.GetChild(0).gameObject.SetActive(true);

                //hide equipped status of the old item
                if(player.weaponIndexAtInventory != 0) {
                
                    inventoryPanel.GetChild((int)player.weaponIndexAtInventory - 1).GetChild(0).gameObject.SetActive(false);
                }

                player.weaponIndexAtInventory = itemClicked.transform.GetSiblingIndex() + 1;

            }
            else if(itemType == "armor") {
                itemClicked.isEquipped = true;
                player.itemArmor = armor;
                player.equipedArmorIndex = itemIndex;
                player.EquipItemServerRpc(itemIndex, player.NetworkObjectId, false);

                //show equipped status
                itemClicked.transform.GetChild(0).gameObject.SetActive(true);

                //hide equipped status of the old item
                if(player.armorIndexAtInventory != 0) {
                 
                    inventoryPanel.GetChild((int)player.armorIndexAtInventory - 1).GetChild(0).gameObject.SetActive(false);
                }
                

                player.armorIndexAtInventory = itemClicked.transform.GetSiblingIndex() + 1;
            }
        }
        player.updateStats();
        UpdateUI();
        gmScript.SavePlayerData();
    }

    public void UpdateUI() {
        if(itemClicked.itemType == "weapon") {
            if(itemClicked.transform.GetSiblingIndex() + 1 == player.weaponIndexAtInventory) {
                transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("UNQUIP");
            }
            else {
                transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("EQUIP");
            }
        }
        else { //item clicked is armor
            if(itemClicked.transform.GetSiblingIndex() + 1 == player.armorIndexAtInventory) {
                transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("UNQUIP");
            }
            else {
                transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("EQUIP");
            }
        }


       
    }

    
}
