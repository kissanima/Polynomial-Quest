using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LanCharacterPanel : MonoBehaviour
{
    LanGameManager gmScript;
    TextMeshProUGUI infoText, skillsInfo;
    Image skill1, skill2, skill3, skill4;
    public string[] warriorSkillsDescription, mageSkillsDescription, assassinSkillsDescription;

    private void Awake() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        infoText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        skill1 = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        skill2 = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        skill3 = transform.GetChild(1).GetChild(2).GetComponent<Image>();
        skill4 = transform.GetChild(1).GetChild(3).GetComponent<Image>();
        skillsInfo = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        infoText.SetText("Name: " + gmScript.player.username + "   CLASS: " + gmScript.player.playerClass + "\n"
        + "Level: " + gmScript.player.level + "\n"
        + "HP: " + gmScript.player.currentHealth.Value + "/" + gmScript.player.finalHealth.Value + "\n"
        + "Mana: " + gmScript.player.currentMana + "/" + gmScript.player.finalMana + "\n"
        + "Exp: " + gmScript.player.currentExp + "/" + gmScript.player.finalRequiredExp + "\n"
        + "\n"
        + "STATS \n"
        + "BASE DAMAGE: " + gmScript.player.baseDamage + "\n"
        + "WEAPON DAMAGE: " + gmScript.player.weaponDmg + "\n"
        + "DAMAGE REDUCTION: " + gmScript.player.damageReduction + "\n"
        + "DEFENSE: " + gmScript.player.baseArmor);

        switch (gmScript.player.playerClass)
        {
            case "Warrior":
                //skills icons
                skill1.sprite = gmScript.warriorSkillIcons[0];
                skill2.sprite = gmScript.warriorSkillIcons[1];
                skill3.sprite = gmScript.warriorSkillIcons[2];
                skill4.sprite = gmScript.warriorSkillIcons[3];
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }

    public void Skill1Button() {
        switch (gmScript.player.playerClass)
        {
            case "Warrior":
            if(transform.GetChild(2).gameObject.activeSelf) {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else {
                //skills info
                skillsInfo.SetText(warriorSkillsDescription[0]);
                transform.GetChild(2).gameObject.SetActive(true); //show skills info panel
            }
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }


    public void Skill2Button() {
        switch (gmScript.player.playerClass)
        {
            case "Warrior":
            if(transform.GetChild(2).gameObject.activeSelf) {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else {
                //skills info
                skillsInfo.SetText(warriorSkillsDescription[1]);
                transform.GetChild(2).gameObject.SetActive(true); //show skills info panel
            }
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }

    public void Skill3Button() {
        switch (gmScript.player.playerClass)
        {
            case "Warrior":
            if(transform.GetChild(2).gameObject.activeSelf) {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else {
                //skills info
                skillsInfo.SetText(warriorSkillsDescription[2]);
                transform.GetChild(2).gameObject.SetActive(true); //show skills info panel
            }
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }

    public void Skill4Button() {
        switch (gmScript.player.playerClass)
        {
            case "Warrior":
            if(transform.GetChild(2).gameObject.activeSelf) {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            else {
                //skills info
                skillsInfo.SetText(warriorSkillsDescription[3]);
                transform.GetChild(2).gameObject.SetActive(true); //show skills info panel
            }
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }


}
