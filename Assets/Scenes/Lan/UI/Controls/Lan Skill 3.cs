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
    Image outerCircle, innerCircle, skillImage, inCooldownSkillImage;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, arrow, cone, skillEffectParent, tempSkillIndicator, instantiatedSkill, warriorSkillObject, mageSkillObject,
    AssassinSkillObject;
    public Transform tempSkill;
    bool hasInitialized, hasPressed, hasReleased, warriorSkillStart, isSkillUseTarget = true;
    float skillRange = .73f, cooldown = 15f, cooldownTimer, tempCooldownTimer, elapseTime, ownerID, manaCost;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Vector2 joystickDirection;
    Rigidbody2D rb;
    AudioSource audioSource;
    [SerializeField] AudioClip mageSkill3SoundEffect;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>();
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        skillImage = GetComponent<Image>();
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        skillEffectParent = GameObject.FindWithTag("SkillEffects").transform;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        inCooldownSkillImage = transform.GetChild(1).GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;
        audioSource = GetComponent<AudioSource>();
        

        player = gmScript.player.transform;
        rb = gmScript.player.GetComponent<Rigidbody2D>();
        playerClass = gmScript.player.playerClass;

        target = player.transform.GetChild(1).GetChild(2).GetChild(2);
        range = player.transform.GetChild(1).GetChild(2).GetChild(0);
        cone = player.transform.GetChild(1).GetChild(2).GetChild(1);
        arrow = player.transform.GetChild(1).GetChild(2).GetChild(3);

        warriorSkillObject = skillEffectParent.GetChild(0).GetChild(2);
        mageSkillObject = skillEffectParent.GetChild(1).GetChild(2).GetChild(1);
        AssassinSkillObject = skillEffectParent.GetChild(3).GetChild(2); //skill effect object

        switch (playerClass)
        {
            case "Warrior":
            tempSkillIndicator = target;
            range.localScale = new Vector2(range.localScale.x * .5f, range.localScale.y * .5f);
            skillRange *= .5f;
            manaCost = 20;
            break;

            case "Mage":
            tempSkillIndicator = target;
            skillImage.sprite = gmScript.mageSkillIcons[2];
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[2];
            manaCost = 10;
            audioSource.clip = mageSkill3SoundEffect;
            break;

            case "Assassin":
            tempSkillIndicator = target;
            skillImage.sprite = gmScript.mageSkillIcons[2];
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[2];
            manaCost = 15;
            break;
        }

        hasInitialized = true;

        //StartCoroutine(ManaCheck()); //start mana check
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
            range.gameObject.SetActive(true); //enable skill range
            tempSkillIndicator.gameObject.SetActive(true); //enable skill target

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
            tempSkillIndicator.position = targetPosition;

            if(isSkillUseTarget) {
                // Move circle towards target position
                target.position = targetPosition;
            }
            else {
                Vector3 difference = tempSkillIndicator.position - player.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                tempSkillIndicator.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);
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
            tempSkillIndicator.gameObject.SetActive(false);


            //start 
            if(cooldownTimer <= 0 && hasPressed == true && hasReleased == true) {
                Debug.Log("Skill 2 executed");
                
                switch (playerClass)
                {
                    case "Warrior":
                    if((gmScript.player.currentMana - 20) >= 0) {
                        gmScript.player.currentMana -= 20;
                        gmScript.UpdateUI();
                        WarriorSkill3ServerRpc(joystickDirection.x, target.position, gmScript.player.NetworkObjectId);

                    
                    }
                    break;
                    
                    case "Mage":
                    if((gmScript.player.currentMana - 10) >= 0) {  //15
                        gmScript.player.currentMana -= 10;
                        gmScript.UpdateUI();
                        Mageskill3ServerRpc(gmScript.player.NetworkObjectId);
                    
                    }
                    break;
                    
                    case "Assassin":
                    if((gmScript.player.currentMana - 10) >= 0) {
                        gmScript.player.currentMana -= 10;
                        gmScript.UpdateUI();
                        Assassinskill3ServerRpc(gmScript.player.NetworkObjectId);
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
    void WarriorSkill3ServerRpc(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        WarriorSkill3ClientRpc(joystickDirectionx, targetPosition, gmScript.player.NetworkObjectId);
    }
    [ClientRpc]
    void WarriorSkill3ClientRpc(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        CastSkill3(joystickDirectionx, targetPosition, playerID);
    }
    void CastSkill3(float joystickDirectionx, Vector3 targetPosition, ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(warriorSkillObject, item.transform.GetChild(3));
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

    
    [ServerRpc(RequireOwnership = false)] public void Mageskill3ServerRpc(ulong playerID) {
        MageSkill3ClientRpc(playerID);
    }
    [ClientRpc] void MageSkill3ClientRpc(ulong playerID) { //playerID = the network object id of the player who cast the skill
    foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(mageSkillObject, item.transform.GetChild(3));
                StartCoroutine(MageSkill3Duration(playerID, item));
                break;
            }
        }      
    }

    IEnumerator MageSkill3Duration(ulong playerID, LanPlayer player) {
        player.isManaShieldOn = true;
        instantiatedSkill.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        player.isManaShieldOn = false;
        instantiatedSkill.gameObject.SetActive(false);
    }

    


    ///////////////////////////////////////ASSASSIN/////////////
    [ServerRpc(RequireOwnership = false)] public void Assassinskill3ServerRpc(ulong playerID) {
        AssassinSkill3ClientRpc(playerID);
    }
    [ClientRpc] void AssassinSkill3ClientRpc(ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(AssassinSkillObject, item.transform.GetChild(3));
                instantiatedSkill.GetComponent<AssassinSkill3>().playerID = playerID;
                instantiatedSkill.gameObject.SetActive(true);
            }
        }
    }
}
