using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;

public class LanGameManager : MonoBehaviour
{
    Transform MissionPanel;
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
    public int difficulty = 0, dungeonStatues;
    AudioSource audioSource;
    public AudioSource correctSound, wrongSound;
    public AudioClip[] backgroundMusic;
    public LanPlayer[] players;
    public bool isPortalFound;
    public AudioClip[] WarriorSoundEffects;
    public AudioClip[] MageSoundEffects;
    public AudioClip playerHitSoundEffect, playerDieSoundEffect;

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
        sliderHealth = GameObject.FindWithTag("PlayerInfoBar").transform.GetChild(1).GetComponent<Slider>();
        healthText = sliderHealth.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        sliderMana = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetComponent<Slider>();
        manaText = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        sliderExp = GameObject.FindWithTag("UI").transform.GetChild(6).GetChild(3).GetComponent<Slider>();
        expText = sliderExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        

        potionText = GameObject.FindWithTag("Controls").transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        usernameSS = GameObject.FindWithTag("PlayerInfoBar").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        usernameWS = player.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        MissionPanel = GameObject.FindWithTag("UI").transform.GetChild(14);
        audioSource = GetComponent<AudioSource>(); //music component
        light2D = GameObject.FindWithTag("Light").GetComponent<Light2D>();

        //SOUNDS
        correctSound = GameObject.FindWithTag("Sounds").transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        wrongSound = GameObject.FindWithTag("Sounds").transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
        //initialize NPC questions and their dialogues
        LanNpc[] npc = FindObjectsOfType<LanNpc>(); //find all NPC
        foreach (LanNpc item in npc)
        {
            item.Initialize();        //Initialize() for each NPC
        }


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
        Debug.Log(sliderHealth);
        sliderHealth.maxValue = player.finalHealth.Value;
        sliderHealth.value = player.currentHealth.Value;
        healthText.SetText(Mathf.FloorToInt(sliderHealth.value).ToString() + " / " + Mathf.FloorToInt(sliderHealth.maxValue).ToString() );
        
        sliderMana.maxValue = player.finalMana;
        sliderMana.value = player.currentMana;
        manaText.SetText(Mathf.FloorToInt(sliderMana.value).ToString()  + " /" +Mathf.FloorToInt(sliderMana.maxValue).ToString() );

        sliderExp.maxValue = player.finalRequiredExp;
        sliderExp.value = player.currentExp;
        expText.SetText(Mathf.FloorToInt(sliderExp.value).ToString()  + "/" + Mathf.FloorToInt(sliderExp.maxValue).ToString() );

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
        player.hint = PlayerPrefs.GetFloat("hint");
        player.finishIntro = PlayerPrefs.GetFloat("finishIntro");
        player.score.Value = PlayerPrefs.GetFloat("score");

        player.updateStats();
        UpdateUI();

    
    //Load Inventory String
    string tempString = PlayerPrefs.GetString("inventory");
    if (tempString == "") {
        tempString = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
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
    Debug.Log("LOAD DATA SUCCESSFULLY");
    } 

    public void SavePlayerData() {

        PlayerPrefs.SetFloat("level", player.level);
        PlayerPrefs.SetFloat("currentExp", player.currentExp);
        PlayerPrefs.SetFloat("finalRequiredExp", player.finalRequiredExp);
        PlayerPrefs.SetFloat("potion", player.potion);
        PlayerPrefs.SetFloat("equipedWeaponIndex", player.equipedWeaponIndex);
        PlayerPrefs.SetFloat("equipedArmorIndex", player.equipedArmorIndex);
        PlayerPrefs.SetFloat("hint", player.hint);
        PlayerPrefs.SetFloat("finishIntro", player.finishIntro);
        PlayerPrefs.SetFloat("score", player.score.Value);
        string tempString = string.Join(",", player.inventory);
        PlayerPrefs.SetString("inventory", tempString);

        Debug.Log("SAVE DATA SUCCESSFULLY");
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

    
    public void StartBackgroundMusic() {
        switch (difficulty)
        {
            case 0:
            audioSource.clip = backgroundMusic[0];
            break;

            case 1:
            audioSource.clip = backgroundMusic[1];
            break;

            case 2:
            audioSource.clip = backgroundMusic[2];
            break;

            case 3:
            audioSource.clip = backgroundMusic[0];
            break;
        }
        audioSource.volume = .5f;
        audioSource.Play();
    }





    ////////////////////////////////////////////////////////MISSION/////////////////////////////////////////////
    public void UpdateMission() {
        TextMeshProUGUI mission1 = MissionPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mission2 = MissionPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mission3 = MissionPanel.GetChild(3).GetComponent<TextMeshProUGUI>();

        mission1.SetText("Dungeon statues completed: " + dungeonStatues + "/ 21");
        if(dungeonStatues == 21) {
            mission1.color = Color.gray;

            //show next mission
            MissionPanel.GetChild(2).gameObject.SetActive(true);
        }
        ////start next mission
        if(isPortalFound) {
            mission2.color = Color.gray;

            //
            MissionPanel.GetChild(3).gameObject.SetActive(true);
        }
    }
}
