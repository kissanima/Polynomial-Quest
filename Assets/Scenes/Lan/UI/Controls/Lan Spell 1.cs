using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
public class LanSpell1 : NetworkBehaviour
{
    LanGameManager gmScript;
    Joystick joystick;
    Image outerCircle, innerCircle;
    public float test;
    Transform player, target, range;
    bool hasInitialized, hasPressed = false, hasReleased;
    float skillRange = .73f, cooldown = 15f, cooldownTimer, tempCooldownTimer;
    GameObject cooldownImageObject;
    Image cooldownImage;
    TextMeshProUGUI cooldownText;
    Color outerColor, innerColor;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").transform.GetComponent<LanGameManager>();
        joystick = transform.GetChild(0).GetComponent<Joystick>();
        outerCircle = transform.GetChild(0).GetComponent<Image>();
        innerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        cooldownText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cooldownImageObject = transform.GetChild(1).gameObject;
        cooldownImage = cooldownImageObject.GetComponent<Image>();
        tempCooldownTimer = cooldownTimer / cooldown;

        player = gmScript.player.transform;
        target = player.GetChild(1).GetChild(2).GetChild(2);
        range = player.GetChild(1).GetChild(2).GetChild(0);
        hasInitialized = true;

        //initialize joystick transparancy
        outerColor = outerCircle.color; //get color from joystick outer Circle
        innerColor = innerCircle.color;
    }

    private void Update() {
        if(!hasInitialized) return;
        cooldownTimer -= Time.deltaTime; // 0
        //Debug.Log(cooldownTimer);
        //start here
        if(cooldownTimer >= 0) { //-1
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
            target.gameObject.SetActive(true);


            //initialize color
            Color outerColor = outerCircle.color;
            Color innerColor = innerCircle.color;
            outerColor.a = 1;
            innerColor.a = 1;

            //set transparancy
            outerCircle.color = outerColor;
            innerCircle.color = innerColor;

            //set scale joystick
            transform.GetChild(0).localScale = new Vector3(2f,2f,1f);

            //start

            //skill range indicator
            range.localScale = new Vector2(.15f,.15f);
            // Calculate target position     //0,0,-10           //
            Vector3 targetPosition = player.position + new Vector3(joystick.Horizontal * skillRange, joystick.Vertical * skillRange, 0) * 1;

            // Move circle towards target position
            target.position = targetPosition;
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
                player.position = target.position;  //teleport player
                target.transform.position = Vector3.zero;  //reset target sprite position

                //
                cooldownTimer = cooldown;
                hasPressed = false;
                hasReleased = false;
            }
        }
    }
}
