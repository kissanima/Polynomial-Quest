using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;

public class LanGameManager : MonoBehaviour
{
    public Slider sliderExp, sliderHealth, sliderMana, sliderHealthWS; //sliderHealthWS worlds space slider

    //initialize variables
    TextMeshProUGUI healthText, manaText, expText;
    public TextMeshProUGUI usernameSS, usernameWS; //SS = screen Space, WS = world space
    public TextMeshProUGUI potionText, console;
    public LanPlayer player;
    public float enemyStatsModifier = 100f;
    public GameObject inventoryManager, itemPool, characterCreationObject;
    LanCreateCharacter characterCreation;
    LanCameraController playerCamera;
    public int difficulty = 0;
    AudioSource audioSource;
    public AudioClip background;
    public LanPlayer[] players;

    Light2D light2D;
    public Sprite[] warriorSkillIcons, mageSkillIcons, assassinSkillIcons;

    public void Initialize() {
        Application.targetFrameRate = 60;
        players = FindObjectsOfType<LanPlayer>();
        foreach (LanPlayer p in players)
        {
            if (p.IsLocalPlayer)
            {
                player = p;
                break;
            }
        }

        
        characterCreation = characterCreationObject.GetComponent<LanCreateCharacter>();
        sliderHealthWS = player.transform.GetChild(1).GetChild(0).GetComponent<Slider>(); //get world space healthbar
        sliderHealth = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetComponent<Slider>();
        healthText = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        sliderMana = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetComponent<Slider>();
        manaText = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        sliderExp = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(3).GetComponent<Slider>();
        expText = sliderExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        

        potionText = GameObject.FindWithTag("Controls").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        usernameSS = GameObject.FindWithTag("PlayerInfoBar").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        usernameWS = player.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        //light
        light2D = GameObject.FindWithTag("Light").GetComponent<Light2D>();


        //initialize NPC questions and their dialogues
        LanNpc[] npc = FindObjectsOfType<LanNpc>(); //find all NPC
        foreach (LanNpc item in npc)
        {
            item.Initialize();        //Initialize() for each NPC
        }

        //music
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = background;
        audioSource.volume = 0;
        audioSource.Play();

        //LoadPlayerData();

        

        //initialize camera
        playerCamera = GameObject.FindWithTag("PlayerCamera").GetComponent<LanCameraController>();
        playerCamera.Initialize();

        LoadPlayerData();


        //initialize skills
        //initialize spell and skills
        GameObject.FindWithTag("Controls").transform.GetChild(7).GetComponent<LanSpell1>().Initialize(); //spell flicker
        GameObject.FindWithTag("Controls").transform.GetChild(8).GetComponent<LanSkill1>().Initialize(); //first skill
        GameObject.FindWithTag("Controls").transform.GetChild(9).GetComponent<LanSkill2>().Initialize(); //second skill
        GameObject.FindWithTag("Controls").transform.GetChild(10).GetComponent<LanSkill3>().Initialize(); //third skill
        GameObject.FindWithTag("Controls").transform.GetChild(11).GetComponent<LanSkill4>().Initialize(); //fourth skill

        //initialize skills warrior
        //GameObject.FindWithTag("SkillEffects").transform.GetChild(0).GetChild(0).GetComponent<WarriorSkill1>().Initialize();
        //GameObject.FindWithTag("SkillEffects").transform.GetChild(0).GetChild(1).GetComponent<WarriorSKill2>().Initialize();
        //GameObject.FindWithTag("SkillEffects").transform.GetChild(0).GetChild(2).GetComponent<WarriorSkill3>().Initialize();
        //GameObject.FindWithTag("SkillEffects").transform.GetChild(0).GetChild(3).GetComponent<WarriorSkill4>().Initialize();


        //initialize skills icon
        switch (player.playerClass)
        {
            case "Warrior":
            //skill 1
            GameObject.FindWithTag("Controls").transform.GetChild(8).GetComponent<Image>().sprite = warriorSkillIcons[0];
            GameObject.FindWithTag("Controls").transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = warriorSkillIcons[0];

            //skill 2
            GameObject.FindWithTag("Controls").transform.GetChild(9).GetComponent<Image>().sprite = warriorSkillIcons[1];
            GameObject.FindWithTag("Controls").transform.GetChild(9).GetChild(1).GetComponent<Image>().sprite = warriorSkillIcons[1];

            //skill 3
            GameObject.FindWithTag("Controls").transform.GetChild(10).GetComponent<Image>().sprite = warriorSkillIcons[2];
            GameObject.FindWithTag("Controls").transform.GetChild(10).GetChild(1).GetComponent<Image>().sprite = warriorSkillIcons[2];

            //skill 4
            GameObject.FindWithTag("Controls").transform.GetChild(11).GetComponent<Image>().sprite = warriorSkillIcons[3];
            GameObject.FindWithTag("Controls").transform.GetChild(11).GetChild(1).GetComponent<Image>().sprite = warriorSkillIcons[3];
            break;

            case "Mage":
            break;

            case "Assassin":
            break;
        }
    }

    public void RainWeather() {
            player.transform.GetChild(2).gameObject.SetActive(true);

            player.moveSpeed -= (player.moveSpeed * .10f); //debuffs
            light2D.intensity = .9f;

            StartCoroutine(RandomWeatherWait());
    }

    IEnumerator RandomWeatherWait() {
        yield return new WaitForSeconds(30f);
            player.transform.GetChild(2).gameObject.SetActive(false);
            player.moveSpeed += (player.moveSpeed * .10f);
            light2D.intensity =  1f;

            player.RandomWeatherServerRpc();
    }
    
    public void UpdateUI() {
        sliderHealth.maxValue = player.finalHealth.Value;
        sliderHealth.value = player.currentHealth.Value;
        healthText.SetText(player.currentHealth.Value.ToString() + " / " + player.finalHealth.ToString());
        //sync Player health bars
        //foreach (LanPlayer item in players)
        //{
        //    item.UpdateHealthBar();
        //}
        
        sliderMana.maxValue = player.finalMana;
        sliderMana.value = player.currentMana;
        manaText.SetText(player.currentMana.ToString() + " /" + player.finalMana.ToString());

        sliderExp.maxValue = player.finalRequiredExp;
        sliderExp.value = player.currentExp;
        expText.SetText(player.currentExp + "/" + player.finalRequiredExp);

        potionText.SetText(player.potion.ToString());

        usernameSS.SetText(player.username + "  Lv. " + player.level);
        usernameWS.SetText(player.username + "  Lv. " + player.level);

    }


    public void LoadPlayerData() {
        player.username = PlayerPrefs.GetString("username");
        player.level = PlayerPrefs.GetFloat("level");
        player.playerClass = PlayerPrefs.GetString("playerClass");
        player.currentExp = PlayerPrefs.GetFloat("currentExp");
        player.finalRequiredExp = PlayerPrefs.GetFloat("finalRequiredExp");
        player.potion = PlayerPrefs.GetFloat("potion");
        player.equipedWeaponIndex = PlayerPrefs.GetFloat("equipedWeaponIndex");
        player.equipedArmorIndex = PlayerPrefs.GetFloat("equipedArmorIndex");
        UpdateUI();

    
    //Load Inventory String
    string tempString = PlayerPrefs.GetString("inventory");
    if (tempString == "") {
        tempString = ("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
    }
    player.inventory = tempString.Split(",");

    

    
    //Instantiate items to Inventory Panel
    for(int i =0; i < player.inventory.Length; i++) { //30
        int temp = int.Parse(player.inventory[i]) - 1; //5

        for (int j = 0; j < itemPool.transform.childCount; j++)
        {
            if(temp == itemPool.transform.GetChild(j).GetSiblingIndex() && temp >= 0) { 
                GameObject tempInstance = Instantiate(itemPool.transform.GetChild(j).gameObject, inventoryManager.transform.GetChild(0)); //instantiate
                tempInstance.gameObject.SetActive(true); 
                break;
            }
        }

    }

    //set Player attack range base on player class
    switch (player.playerClass)
    {
        case "Warrior":
        player.attackRange = 0.30f;
        break;

        case "Mage":
        player.attackRange = 0.75f;
        break;

        case "Assassin":
        player.attackRange = 0.30f;
        break;
    }
    //END LOAD PLAYER DATA
    } 

    public void SavePlayerData() {
        PlayerPrefs.SetFloat("level", player.level);
        PlayerPrefs.SetFloat("currentExp", player.currentExp);
        PlayerPrefs.SetFloat("finalRequiredExp", player.finalRequiredExp);
        PlayerPrefs.SetFloat("potion", player.potion);
        PlayerPrefs.SetFloat("equipedWeaponIndex", player.equipedWeaponIndex);
        PlayerPrefs.SetFloat("equipedArmorIndex", player.equipedArmorIndex);

        string tempString = string.Join(",", player.inventory);
        PlayerPrefs.SetString("inventory", tempString);
    } 



    public void LoadPlayerCostumization() {
        //find all player scripts
        LanPlayer[] temp = FindObjectsOfType<LanPlayer>();
        foreach (LanPlayer p in temp)
        {
            //belt
            p.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = characterCreation.belt[p.belt.Value];

            //boots
            p.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = characterCreation.boots_l[p.boots.Value];
            p.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = characterCreation.boots_r[p.boots.Value];

            //elbow
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.elbow_l[p.elbow.Value];
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.elbow_r[p.elbow.Value];

            //face
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.face[p.face.Value];

            //hood
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = characterCreation.hood[p.hood.Value];

            //legs
            p.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.legs_l[p.legs.Value];
            p.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.legs_r[p.legs.Value];

            //shoulder
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.shoulder_l[p.shoulder.Value];
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite = characterCreation.shoulder_r[p.shoulder.Value];

            //Torso
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = characterCreation.torso[p.torso.Value]; 

            //torso
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = characterCreation.wrist_l[p.wrist.Value]; 
            p.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = characterCreation.wrist_r[p.wrist.Value];
        }
    }

    
    
}
