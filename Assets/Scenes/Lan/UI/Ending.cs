using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    [SerializeField] Transform WilsonTransform, emmanuelTransform, controls, playerInforBar, minimap, missionPanel, mapParent, effect104, ultimateWeapon, ultimateWeaponSS, itemPool, endingFade;
    [SerializeField] TextMeshProUGUI wilsonText, emmanuelText, endingFadeText;
    Vector2 wilsonPosition;
    LanGameManager gmScript;
    Animator emmanuelAnim;
    Rigidbody2D emmanuelRb;
    string dialogue1;
    float elapseTime, moveDuration = 1;
    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        wilsonPosition = new(0.588f, -0.072f);
        dialogue1 = "Thank you!! Brave one. The darkness among us has been extinguished by your bravery, determination, and mastery of polynomials.";

        emmanuelAnim = emmanuelTransform.GetComponent<Animator>();
        emmanuelRb = emmanuelTransform.GetComponent<Rigidbody2D>();
    }

    void TeleportPlayersEvent() {
        //change map
        mapParent.GetChild(0).gameObject.SetActive(true);
        mapParent.GetChild(3).gameObject.SetActive(false);

        foreach (var item in gmScript.players) //teleport players
        {
            item.transform.position = Vector2.zero;
        }

        WilsonTransform.position = wilsonPosition;
        wilsonText.transform.parent.gameObject.SetActive(true);

        controls.gameObject.SetActive(false);
        playerInforBar.gameObject.SetActive(false);
        minimap.gameObject.SetActive(false);
        missionPanel.gameObject.SetActive(false);

        StartCoroutine(WilsonDialogue1());
    }

    IEnumerator WilsonDialogue1() {
        wilsonText.text = null;
        foreach (var item in dialogue1)
        {
            wilsonText.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        wilsonText.text = null;
        wilsonText.transform.parent.gameObject.SetActive(false); //disable textbox
        StartCoroutine(EmmanuelDialogue1());
    }

    IEnumerator EmmanuelDialogue1() {
        Vector3 position = new(0.205f, 0.143f); //0-1
        Vector2 direction = (position - emmanuelRb.transform.position).normalized * .5f;

        while (elapseTime <= moveDuration) { //0   //1
            emmanuelAnim.SetBool("isMoving", true);  //running
            elapseTime += Time.fixedDeltaTime;  
            emmanuelRb.MovePosition(emmanuelRb.position + direction * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        emmanuelAnim.SetBool("isMoving", false);  //running
        emmanuelText.transform.parent.gameObject.SetActive(true);

        //start text effect
        string emmanuelDialogue1 = "Witness, " + gmScript.player.username + ", the outcome of your bravery.";
        string emmanuelDialogue2 = "You have rebuilt our sanctuary, brought back the lost artifacts, and given these walls new life through your strong commitment.";
        foreach (var item in emmanuelDialogue1)
        {
            emmanuelText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);

        //text 2
        emmanuelText.text = null;
        foreach (var item in emmanuelDialogue2)
        {
            emmanuelText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);



        emmanuelText.text = null;
        emmanuelText.transform.parent.gameObject.SetActive(false);

        StartCoroutine(WilsonDialogue2());
    }

    IEnumerator WilsonDialogue2() {
        wilsonText.transform.parent.gameObject.SetActive(true); //enable textbox

        string WilsonDialogue2 = "As evidence of your incredible journey, the castle is still standing. We want to express our thanks by giving you the best gifts possible.";
        foreach (var item in WilsonDialogue2)
        {
            wilsonText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
        wilsonText.text = null;

        string WilsonDialogue3 = "Behold!! THE ULTIMATE WEAPON!!";
        foreach (var item in WilsonDialogue3)
        {
            wilsonText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
        wilsonText.text = null;
        wilsonText.transform.parent.gameObject.SetActive(false); //disable textbox

        //SHOW ITEMM!!!!!!!!
        effect104.gameObject.SetActive(true);
        ultimateWeapon.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        gmScript.player.EquipItemServerRpc(35, gmScript.player.NetworkObjectId);

        StartCoroutine(EmmanuelDialogue2());
    }

    IEnumerator EmmanuelDialogue2() {
        emmanuelText.transform.parent.gameObject.SetActive(true);
        string emmanuelDialogue4 = "With the help of these treasures, you will continue to promote knowledge and overcome obstacles";
        string emmanuelDialogue5 = "Your name will always be remembered in the Castle of Wisdom. " + gmScript.player.username;

        //text 4 start
        foreach (var item in emmanuelDialogue4)
        {
            emmanuelText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
        emmanuelText.text = null;

        //text 5 start
        foreach (var item in emmanuelDialogue5)
        {
            emmanuelText.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        emmanuelText.text = null;
        emmanuelText.transform.parent.gameObject.SetActive(false);

        StartCoroutine(WilsonDialogue3());
    }

    IEnumerator WilsonDialogue3() {
        wilsonText.transform.parent.gameObject.SetActive(true); //enable textbox
        string WilsonDialogue4 = "Goodbye, legendary hero. Through the years, your legacy will always be appreciated.";
        foreach (var item in WilsonDialogue4)
        {
            wilsonText.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        wilsonText.text = null;
        wilsonText.transform.parent.gameObject.SetActive(false);
        endingFade.gameObject.SetActive(true);
        

        //ending text
        string endingText = "Congratulations, " + gmScript.player.username + ", on completing the Polynomial Quest, the castle has been fully restored! Math Genius!";
        foreach (var item in endingText)
        {
            endingFadeText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
    }
       
}
