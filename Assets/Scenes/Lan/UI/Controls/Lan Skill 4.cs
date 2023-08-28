using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class LanSkill4 : NetworkBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle, skillImage, inCooldownSkillImage;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, arrow, cone, skillEffectParent, tempSkillIndicator, instantiatedSkill, warriorSkillObject, mageSkillObject,
    AssassinSkillObject;
    public Transform tempSkill;
    bool hasInitialized, hasPressed = false, hasReleased;
    float skillRange = .73f, cooldown = 50f, cooldownTimer, tempCooldownTimer, manaCost;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Vector3 targetPosition;
    Vector2 joystickDirection;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>();
        skillImage = GetComponent<Image>();
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        skillEffectParent = GameObject.FindWithTag("SkillEffects").transform;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        inCooldownSkillImage = transform.GetChild(1).GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;

        player = gmScript.player.transform;
        playerClass = gmScript.player.playerClass;

        target = player.transform.GetChild(1).GetChild(2).GetChild(2);
        range = player.transform.GetChild(1).GetChild(2).GetChild(0);
        cone = player.transform.GetChild(1).GetChild(2).GetChild(1);
        arrow = player.transform.GetChild(1).GetChild(2).GetChild(3);

        warriorSkillObject = skillEffectParent.GetChild(0).GetChild(3); //skill effect object
        mageSkillObject = skillEffectParent.GetChild(1).GetChild(2).GetChild(2); //skill effect object
        AssassinSkillObject = skillEffectParent.GetChild(3).GetChild(3); //skill effect object

        switch (playerClass)
        {   
            case "Warrior":
            tempSkillIndicator = arrow;
            manaCost = 35;
            break;
            
            case "Mage":
            skillImage.sprite = gmScript.mageSkillIcons[3];
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[3];
            tempSkillIndicator = arrow;
            manaCost = 35;
            break;

            case "Assassin":
            skillImage.sprite = gmScript.mageSkillIcons[3];
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[3];
            tempSkillIndicator = arrow;
            manaCost = 35;
            break;
        }
        hasInitialized = true;

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
            cooldownText.fontSize = 120;
            cooldownText.SetText(cooldownTimer.ToString("0.0"));
        }
        else if((gmScript.player.currentMana - manaCost) < 0) {
                cooldownImage.gameObject.SetActive(true);
                cooldownImage.fillAmount = 1;
                cooldownText.gameObject.SetActive(true);
                cooldownText.fontSize = 80;
                cooldownText.SetText("NO MANA");
        }
        else {
            transform.GetChild(0).gameObject.SetActive(true); //enable skillshot
            transform.GetChild(0).gameObject.SetActive(true); //enable cooldown image
            cooldownImageObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);
            cooldownImage.fillAmount = 1;
        }

        if(joystick.Horizontal != 0 || joystick.Vertical != 0 && (gmScript.player.currentMana - manaCost) >= 0) {
            hasPressed = true;
            range.gameObject.SetActive(true);
            tempSkillIndicator.gameObject.SetActive(true);


            //initialize color
            Color outerColor = outerCircle.color;
            Color innerColor = innerCircle.color;
            outerColor.a = 1;
            innerColor.a = 1;

            //set transparancy
            outerCircle.color = outerColor;
            innerCircle.color = innerColor;

            //set scale
            transform.GetChild(0).localScale = new Vector3(2f,2f,1f);

            switch (playerClass)
            {
                case "Warrior":
                tempSkillIndicator.gameObject.SetActive(false); //disable skill indicator
                // Calculate target position
                targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;
                // Move circle towards target position
                target.position = targetPosition;
                break;

                case "Mage":
                range.localScale = new Vector2(.25f,.25f);
                targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;
                tempSkillIndicator.position = targetPosition;

                Vector3 difference = tempSkillIndicator.position - player.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                tempSkillIndicator.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);
                break;

                case "Assassin":
                tempSkillIndicator.gameObject.SetActive(false);

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
                    if((gmScript.player.currentMana - 35) >= 0) {
                    gmScript.player.currentMana -= 35;
                    gmScript.UpdateUI();
                    Warriorskill4ServerRpc(gmScript.player.NetworkObjectId);
                    }
                    break;
                    
                    case "Mage":
                    if((gmScript.player.currentMana - 35) >= 0) {
                    gmScript.player.currentMana -= 35;
                    gmScript.UpdateUI();
                    MageSkill4ServerRpc(joystickDirection, gmScript.player.NetworkObjectId);
                    }
                    break;

                    case "Assassin":
                    if((gmScript.player.currentMana - 35) >= 0) {
                    gmScript.player.currentMana -= 35;
                    gmScript.UpdateUI();
                    AssassinSkill4ServerRpc(gmScript.player.NetworkObjectId);
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


    [ServerRpc(RequireOwnership = false)]
    public void Warriorskill4ServerRpc(ulong playerID) {
        WarriorSkill4ClientRpc( playerID);
    }
    [ClientRpc]
    void WarriorSkill4ClientRpc(ulong playerID) { //playerID = the network object id of the player who cast the skill
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(warriorSkillObject, item.transform.GetChild(3));
                StartCoroutine(WarriorSkill4Wait(playerID));
                break;
            }
        }
            
    }

    IEnumerator WarriorSkill4Wait( ulong playerID) {
        WarriorSkill4 temp = instantiatedSkill.GetComponent<WarriorSkill4>();
        temp.ownerID = playerID;
        //temp.direction = direction;
        
        //tempSkill.SetParent(player);
        //tempSkill.localPosition = new Vector2(0, 0.084f);
        instantiatedSkill.gameObject.SetActive(true);


        yield return new WaitForSeconds(10); 
        instantiatedSkill.SetParent(skillEffectParent.GetChild(0));
        instantiatedSkill.gameObject.SetActive(false);
    }


    [ServerRpc(RequireOwnership = false)] 
    void MageSkill4ServerRpc(Vector2 direction, ulong playerID) {
        MageSkill4ClientRpc(direction, playerID);
    }
    [ClientRpc]
    void MageSkill4ClientRpc(Vector2 direction, ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(mageSkillObject, item.transform.GetChild(3));
                StartCoroutine(MageSkill4Duration(direction, playerID));
                break;
            }
        }
    }

    IEnumerator MageSkill4Duration(Vector2 direction, ulong playerID) {
        MageSkill4 temp = instantiatedSkill.GetComponent<MageSkill4>();
        temp.playerID = playerID; //1
        temp.direction = direction;

        instantiatedSkill.gameObject.SetActive(true);
        yield return new WaitForSeconds(3); //3 seconds
        //instantiatedSkill.parent = skillEffectParent.GetChild(0);
        Destroy(instantiatedSkill.gameObject);
    }




    [ServerRpc(RequireOwnership = false)] void AssassinSkill4ServerRpc(ulong playerID) {
        AssassinSkill4ClientRpc(playerID);
    }
    [ClientRpc] void AssassinSkill4ClientRpc(ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(AssassinSkillObject, item.transform.GetChild(3));
                instantiatedSkill.gameObject.SetActive(true);
                StartCoroutine(AssassinSkill4Duration(playerID));
                break;
            }
        }
    }

    IEnumerator AssassinSkill4Duration(ulong playerID) {
        if(gmScript.player.NetworkObjectId == playerID) {
            gmScript.player.moveSpeed += 1f;
            gmScript.player.attackSpeed += 1;

            yield return new WaitForSeconds(10);
            instantiatedSkill.gameObject.SetActive(false); 
            gmScript.player.moveSpeed -= 1f;
            gmScript.player.attackSpeed -= 1;
        }
    }
}
