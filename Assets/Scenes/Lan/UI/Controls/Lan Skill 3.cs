using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class LanSkill3 : NetworkBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, arrow, cone, skillEffectParent, temp, tempSkill;
    bool hasInitialized, hasPressed, hasReleased, warriorSkillStart, isSkillUseTarget = true;
    float skillRange = .73f, cooldown = 15f, cooldownTimer, tempCooldownTimer, elapseTime, ownerID;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Vector2 joystickDirection;
    Rigidbody2D rb;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>();
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        skillEffectParent = GameObject.FindWithTag("SkillEffects").transform;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;

        player = gmScript.player.transform;
        rb = gmScript.player.GetComponent<Rigidbody2D>();
        playerClass = gmScript.player.playerClass;

        target = player.transform.GetChild(1).GetChild(2).GetChild(2);
        range = player.transform.GetChild(1).GetChild(2).GetChild(0);
        cone = player.transform.GetChild(1).GetChild(2).GetChild(1);
        arrow = player.transform.GetChild(1).GetChild(2).GetChild(3);

        tempSkill = skillEffectParent.GetChild(0).GetChild(2);

        switch (playerClass)
        {
            case "Warrior":
            temp = target;
            range.localScale = new Vector2(range.localScale.x * .5f, range.localScale.y * .5f);
            skillRange *= .5f;

            break;
        }

        hasInitialized = true;
    }

    private void Update() {
        if(!hasInitialized) return;
        cooldownTimer -= Time.deltaTime;
        //Debug.Log(cooldownTimer);
        //start here
        if(cooldownTimer >= 0) {
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
            range.gameObject.SetActive(true); //enable skill range
            temp.gameObject.SetActive(true); //enable skill target

            range.localScale = new Vector2(0.075f, 0.075f); //set range indicator scale

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

            //start
            // Calculate target position
            Vector3 targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;

            // Move circle towards target position
            temp.position = targetPosition;

            if(isSkillUseTarget) {
                // Move circle towards target position
                target.position = targetPosition;
            }
            else {
                Vector3 difference = temp.position - player.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                temp.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);
            }
            


            joystickDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        else {
            if(!hasPressed) return; //if has NOT pressed, do not proceed
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
            temp.gameObject.SetActive(false);


            //start 
            if(cooldownTimer <= 0 && hasPressed == true && hasReleased == true) {
                Debug.Log("Skill 2 executed");
                
                switch (playerClass)
                {
                    case "Warrior":
                    WarriorSkill3ServerRpc(joystickDirection.x, target.position, gmScript.player.NetworkObjectId);
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
    void WarriorSkill3ServerRpc(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        WarriorSkill3ClientRpc(joystickDirectionx, targetPosition, gmScript.player.NetworkObjectId);
    }

    Transform instantiatedSkill;
    [ClientRpc]
    void WarriorSkill3ClientRpc(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        CastSkill3(joystickDirectionx, targetPosition, playerID);
    }
    void CastSkill3(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(tempSkill, item.transform.GetChild(3));
                instantiatedSkill.GetComponent<WarriorSkill3>().ownerID = playerID;
                if(joystickDirectionx < 0) {
                    instantiatedSkill.GetComponent<SpriteRenderer>().flipX = true;
                }
                else {
                    instantiatedSkill.GetComponent<SpriteRenderer>().flipX = false;
                }
                instantiatedSkill.position = targetPosition;
                instantiatedSkill.gameObject.SetActive(true);
                break;
            }
        }
    }

    

    
    

}
