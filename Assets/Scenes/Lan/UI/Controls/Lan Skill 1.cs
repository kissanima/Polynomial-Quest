using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LanSkill1 : MonoBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle;
    public float baseDamage, damageModifier, finalDamage;
    Transform player, target, range, skillEffectParent, temp, tempSkill;
    bool hasInitialized, hasPressed, hasReleased;
    float skillRange = .73f, cooldown = 6f, cooldownTimer, tempCooldownTimer, isDirectCast;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    string playerClass;
    Color outerColor, innerColor;



    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>(); //joystick
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>(); //joystick
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        skillEffectParent = GameObject.FindWithTag("SkillEffects").transform;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;

        player = gmScript.player.transform;
        playerClass = gmScript.player.playerClass;


        target = player.GetChild(1).GetChild(2).GetChild(2);
        range = player.GetChild(1).GetChild(2).GetChild(0);

        tempSkill = skillEffectParent.GetChild(0).GetChild(0);


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
            range.gameObject.SetActive(true);

            //start
            switch (playerClass)
            {
                case "Warrior":
                target.gameObject.SetActive(false); //disable target indicator
                range.localScale = new Vector2(0.075f, 0.075f); //set range indicator scale

                //set joystick transparancy
                outerColor.a = 0;
                innerColor.a = 0;
                break;
            }
            // Calculate target position         
            //Vector3 targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;

            // Move circle towards target position
            //target.position = targetPosition;
            //target.position = Vector3.MoveTowards(player.position, targetPosition, 5f );
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
            target.gameObject.SetActive(false);


            //start 
            if(cooldownTimer <= 0 && hasPressed == true && hasReleased == true) {
                Debug.Log("Skill 1 executed");
                
                switch (gmScript.player.playerClass)
                {
                    case "Warrior":
                    StartCoroutine(WarriorSkill1Wait());
                    break;
                    
                    
                }

                //start skill cooldown
                cooldownTimer = cooldown;
                hasPressed = false;
                hasReleased = false;
            }
        }
    }

    IEnumerator WarriorSkill1Wait() {
        tempSkill.parent = gmScript.player.transform.GetChild(3);
        Debug.Log(tempSkill);
        tempSkill.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        tempSkill.parent = skillEffectParent.GetChild(0);
        tempSkill.gameObject.SetActive(false);
    }
}
