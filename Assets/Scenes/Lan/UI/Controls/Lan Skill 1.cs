using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class LanSkill1 : NetworkBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle, skillImage, inCooldownSkillImage;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, arrow, cone, skillEffectParent, tempSkillIndicator, instantiatedSkill;
    public Transform tempSkill;
    bool hasInitialized, hasPressed, hasReleased;
    float skillRange = .73f, cooldown = 6f, cooldownTimer, tempCooldownTimer, isDirectCast;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Rigidbody2D rb;
    Vector2 joystickDirection;
    Color outerColor, innerColor;



    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>(); //joystick
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>(); //joystick
        skillImage = GetComponent<Image>();
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        skillEffectParent = GameObject.FindWithTag("SkillEffects").transform;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        inCooldownSkillImage = transform.GetChild(1).GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;

        player = gmScript.player.transform;
        rb = gmScript.player.GetComponent<Rigidbody2D>();
        playerClass = gmScript.player.playerClass;


        target = player.GetChild(1).GetChild(2).GetChild(2);
        range = player.GetChild(1).GetChild(2).GetChild(0);
        cone = player.transform.GetChild(1).GetChild(2).GetChild(1);
        arrow = player.transform.GetChild(1).GetChild(2).GetChild(3);

        switch (playerClass)
        {
            case "Warrior":
            tempSkillIndicator = arrow;
            tempSkill = skillEffectParent.GetChild(0).GetChild(0);
            skillImage.sprite = gmScript.warriorSkillIcons[0];
            break;

            case "Mage":
            tempSkill = skillEffectParent.GetChild(1).GetChild(2).GetChild(0); //
            skillImage.sprite = gmScript.mageSkillIcons[0];
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[0];
            tempSkillIndicator = arrow;
            break;

            case "Assassin":
            break;
        }

        hasInitialized = true;

        //initialize joystick transparancy
        outerColor = outerCircle.color; //get color from joystick outer Circle
        innerColor = innerCircle.color;
    }

    private void Update() {
        if(!hasInitialized) return;
        cooldownTimer -= Time.deltaTime; 
        //Debug.Log(cooldownTimer);
        //start here
        if(cooldownTimer >= 0) { //
            transform.GetChild(0).gameObject.SetActive(false); //disable skillshot
            cooldownImageObject.SetActive(true); //enable cooldown image
            cooldownImage.fillAmount = (cooldownTimer - 0) / (cooldown - 0);
            cooldownText.gameObject.SetActive(true);
            cooldownText.SetText(cooldownTimer.ToString("0.0"));
        }
        else {
            transform.GetChild(0).gameObject.SetActive(true); //enable skillshot
            transform.GetChild(0).gameObject.SetActive(true); //enable cooldown image
            cooldownImageObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);
            cooldownImage.fillAmount = 1;
        }

        if(joystick.Horizontal != 0 || joystick.Vertical != 0) {
            hasPressed = true;
            range.gameObject.SetActive(true); //range indicator
            tempSkillIndicator.gameObject.SetActive(true); //enable skill target indicator

            //set scale
            transform.GetChild(0).localScale = new Vector3(2f,2f,1f);
            
            //start
            switch (playerClass)
            {
                case "Warrior":
                tempSkillIndicator.gameObject.SetActive(false); //disable target indicator
                range.localScale = new Vector2(0.075f, 0.075f); //set range indicator scale

                //set joystick transparancy
                outerColor.a = 0;
                innerColor.a = 0;
                break;

                case "Mage":
                range.localScale = new Vector2(.25f,.25f); //range indicator

                // Calculate target position         
                Vector3 targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;
                // Move circle towards target position
                tempSkillIndicator.position = targetPosition;

                Vector3 difference = tempSkillIndicator.position - player.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                tempSkillIndicator.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);
                break;
            }
            joystickDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        else {
            if(!hasPressed) return;
            hasReleased = true;
            //initialize color
            Color outerColor = outerCircle.color;
            Color innerColor = innerCircle.color;
            outerColor.a = 0;
            innerColor.a = 0;

            //set transparancy
            outerCircle.color = outerColor;
            innerCircle.color = innerColor;

            //set scale
            transform.GetChild(0).localScale = new Vector3(1f,1f,1f);

            //disable skill shot
            range.gameObject.SetActive(false); 
            tempSkillIndicator.gameObject.SetActive(false);


            //start 
            if(cooldownTimer <= 0 && hasPressed == true && hasReleased == true) { 
                switch (gmScript.player.playerClass)
                {
                    case "Warrior":
                    if((gmScript.player.currentMana - 15) >= 0) {  //15
                    gmScript.player.currentMana -= 15;
                    gmScript.UpdateUI();
                    Warriorskill1ServerRpc(gmScript.player.NetworkObjectId);

                    //start skill cooldown
                    cooldownTimer = cooldown;
                    hasPressed = false;
                    hasReleased = false;
                    }
                    break;

                    case "Mage":
                    if((gmScript.player.currentMana - 10) >= 0) {  //15
                    gmScript.player.currentMana -= 10;
                    gmScript.UpdateUI();
                    MageSkill1ServerRpc(gmScript.player.NetworkObjectId);

                    //start skill cooldown
                    cooldownTimer = cooldown;
                    hasPressed = false;
                    hasReleased = false;
                    }
                    break;
                    
                    
                }

                //start skill cooldown
                cooldownTimer = cooldown;
                hasPressed = false;
                hasReleased = false;
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]  //1
    public void Warriorskill1ServerRpc(ulong playerID) { //
        WarriorSkill1ClientRpc(playerID); //1
    }
    [ClientRpc]   //2
    void WarriorSkill1ClientRpc(ulong playerID) { //playerID = the network object id of the player who cast the skill
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(tempSkill, item.transform.GetChild(3));
                StartCoroutine(WarriorSkill1Wait(playerID));
                break;
            }
        }      
    }
    IEnumerator WarriorSkill1Wait(ulong playerID) {
        instantiatedSkill.GetComponent<WarriorSkill1>().ownerID = playerID; //1

        instantiatedSkill.gameObject.SetActive(true);
        yield return new WaitForSeconds(3); //3 seconds
        instantiatedSkill.parent = skillEffectParent.GetChild(0);
        instantiatedSkill.gameObject.SetActive(false);
    }






    [ServerRpc(RequireOwnership = false)]  //1
    public void MageSkill1ServerRpc(ulong playerID) { //
        MageSkill1ClientRpc(playerID); //1
    }
    [ClientRpc]
    void MageSkill1ClientRpc(ulong playerID) { //playerID = the network object id of the player who cast the skill
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(tempSkill, item.transform.GetChild(3));
                StartCoroutine(MageSkill1Duration(playerID));
                break;
            }
        }      
    }
    IEnumerator MageSkill1Duration(ulong playerID) {
        MagicBullet temp = instantiatedSkill.GetComponent<MagicBullet>();
        temp.playerID = playerID; //1
        temp.direction = joystickDirection;

        instantiatedSkill.gameObject.SetActive(true); 
        yield return new WaitForSeconds(3); //3 seconds
        //instantiatedSkill.parent = skillEffectParent.GetChild(0);
        Destroy(instantiatedSkill.gameObject);
    }
}
