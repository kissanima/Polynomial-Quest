using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class LanSkill2 : NetworkBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle, skillImage, inCooldownSkillImage;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, arrow, cone, skillEffectParent, tempSkillIndicator, tempSkill, instantiatedSkill, warriorSkillObject, mageSkillObject,
    AssassinSkillObject;
    bool hasInitialized, hasPressed, hasReleased, isDirectCast;
    float skillRange = .73f, cooldownTimer, tempCooldownTimer, elapseTime, manaCost;
    public float cooldown;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Vector2 joystickDirection;
    Rigidbody2D rb;
    Color outerColor, innerColor;
    Vector3 targetPosition;
    AudioSource audioSource;
    [SerializeField] AudioClip warriorSkill2SoundEffect;
    [SerializeField] AudioClip mageSkill2SoundEffect;

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

        //sound
        audioSource = GetComponent<AudioSource>();

        player = gmScript.player.transform;
        rb = gmScript.player.GetComponent<Rigidbody2D>();
        playerClass = gmScript.player.playerClass;

        target = player.transform.GetChild(1).GetChild(2).GetChild(2);
        range = player.transform.GetChild(1).GetChild(2).GetChild(0);
        cone = player.transform.GetChild(1).GetChild(2).GetChild(1);
        arrow = player.transform.GetChild(1).GetChild(2).GetChild(3);

        warriorSkillObject = skillEffectParent.GetChild(0).GetChild(1);
        AssassinSkillObject = skillEffectParent.GetChild(3).GetChild(1);

        switch (playerClass)
        {
            case "Warrior":
            tempSkillIndicator = arrow;
            manaCost = 20;
            audioSource.clip = warriorSkill2SoundEffect;
            break;

            case "Mage":
            skillImage.sprite = gmScript.mageSkillIcons[1]; //set image 
            inCooldownSkillImage.sprite = gmScript.mageSkillIcons[1]; //set image 
            tempSkillIndicator = target;
            manaCost = 20;
            audioSource.clip = mageSkill2SoundEffect;
            break;

            case "Assassin":
            tempSkillIndicator = target;
            skillImage.sprite = gmScript.assassinSkillIcons[1]; //set image 
            inCooldownSkillImage.sprite = gmScript.assassinSkillIcons[1]; //set image 
            manaCost = 20;
            break;
        }
        hasInitialized = true;

        //initialize joystick transparancy
        outerColor = outerCircle.color; //get color from joystick outer Circle
        innerColor = innerCircle.color;

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
            //set joystick transparancy
            outerColor.a = 1;
            innerColor.a = 1;

            //set scale
            transform.GetChild(0).localScale = new Vector3(2f,2f,1f);

            //start
            switch (playerClass)
            {
                case "Warrior":

                range.localScale = new Vector2(.25f,.25f);
                // Calculate target position
                targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;
                // Move circle towards target position
                tempSkillIndicator.position = targetPosition;

                Vector3 difference = tempSkillIndicator.position - player.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                tempSkillIndicator.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);
                break;

                case "Mage":
                //skill range indicator
                range.localScale = new Vector2(.15f,.15f);
                // Calculate target position     //0,0,-10           //
                targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;
                // Move circle towards target position
                target.position = targetPosition;
                break;

                case "Assassin":
                //skill range indicator
                range.localScale = new Vector2(.15f,.15f);
                break;
            }
            //target.position = Vector3.MoveTowards(player.position, targetPosition, 5f );

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
                switch (playerClass)
                {
                    case "Warrior":
                    if((gmScript.player.currentMana - 20) >= 0) {
                    gmScript.player.currentMana -= 20;
                    gmScript.UpdateUI();
                    StartCoroutine(WarriorSkill2Wait());

                    //start skill cooldown
                    cooldownTimer = cooldown;
                    hasPressed = false;
                    hasReleased = false;
                    }
                    break;
                    
                    case "Mage":
                    if((gmScript.player.currentMana - 20) >= 0) {
                    gmScript.player.currentMana -= 20;
                    gmScript.UpdateUI();

                    player.position = target.position;  //teleport player
                    target.transform.position = Vector3.zero;  //reset target sprite position

                    //start skill cooldown
                    cooldownTimer = cooldown;
                    hasPressed = false;
                    hasReleased = false;
                    }
                    break;

                    case "Assassin":
                    if((gmScript.player.currentMana - 20) >= 0) {
                    gmScript.player.currentMana -= 20;
                    gmScript.UpdateUI();

                    AssassinSkill2ServerRpc(gmScript.player.NetworkObjectId);
                    //start skill cooldown
                    cooldownTimer = cooldown;
                    hasPressed = false;
                    hasReleased = false;
                    }
                    break;
                }
                audioSource.Play(); //play sound effect
                //start skill cooldown
                cooldownTimer = cooldown;
                hasPressed = false;
                hasReleased = false;
            }
        }
    }

    

    
    IEnumerator WarriorSkill2Wait() {
        gmScript.player.isUsingSkill = true;
        warriorSkillObject.SetParent(gmScript.player.transform.GetChild(3));
        warriorSkillObject.gameObject.SetActive(true);
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        while (elapseTime < .5f)
        {
            elapseTime += Time.deltaTime;
            rb.MovePosition(rb.position + joystickDirection * 3 * Time.fixedDeltaTime );
            yield return new WaitForFixedUpdate();
            

        }
        rb.interpolation = RigidbodyInterpolation2D.None;
        elapseTime = 0;
        gmScript.player.isUsingSkill = false;
        warriorSkillObject.SetParent(skillEffectParent.GetChild(0));
        warriorSkillObject.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    void AssassinSkill2ServerRpc(ulong playerID) {
        AssassinSkill2ClientRpc(playerID);
    }
    [ClientRpc]
    void AssassinSkill2ClientRpc(ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                instantiatedSkill = Instantiate(AssassinSkillObject, item.transform.GetChild(3));
                instantiatedSkill.GetComponent<AssassinSkill2>().playerID = playerID;
                instantiatedSkill.gameObject.SetActive(true);
            }
            
        }
    }
}
