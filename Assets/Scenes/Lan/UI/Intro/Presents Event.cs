using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PresentsEvent : MonoBehaviour
{
    Intro intro;
    TextMeshProUGUI textBox, presentsText, emmanueltextBox;
    string introText = "In the realm of Polynomia, a mystical land filled with mathematical wonders, an ancient prophecy has foretold the arrival of a group of chosen ones who will embark on a perilous journey known as the Polynomial Quest.";
    //string[] emmanuelDialogues = new string[] { "", "",};
    Animator presentsAnim, textBoxAnim;
    Transform npcParent, controls, playerInfoBar;
    //Rigidbody2D emmanuelRb;
    //float elapseTime, moveDuration = 1f;

    private void Awake() {
        intro = transform.parent.GetComponent<Intro>(); 
        presentsText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        presentsAnim = GetComponent<Animator>();
        textBox = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        textBoxAnim = textBox.GetComponent<Animator>();

        npcParent = GameObject.Find("Npc").transform;
        emmanueltextBox = npcParent.GetChild(2).GetChild(7).GetComponent<TextMeshProUGUI>();
        //emmanuelRb = npcParent.GetChild(2).GetComponent<Rigidbody2D>();
        controls = GameObject.Find("Controls").transform;
        playerInfoBar = GameObject.Find("UI").transform.GetChild(6);
    }

    void Event() {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(PlayText());
    }

    
    IEnumerator PlayText() {
        yield return new WaitForSeconds(3.5f);
        presentsText.gameObject.SetActive(false);
        textBox.gameObject.SetActive(true);
        foreach (char item in introText)
        {
            textBox.text += item; //fil
            yield return new WaitForSeconds(0.05f); //.1 default value
        }
        transform.parent.parent.GetChild(7).gameObject.SetActive(false); //disable welcome object
        transform.parent.GetChild(0).gameObject.SetActive(true); //enable borders
        controls.gameObject.SetActive(false); //disable controls during introduction
        playerInfoBar.gameObject.SetActive(false);
        presentsAnim.Play("end");
        textBoxAnim.Play("end");
    }

    void EventPresentsEnd() { //disable presents gameobject
        intro.PlayEmmanuelCourotine();
        gameObject.SetActive(false);

        //start emmanuel coroutine
        //StartCoroutine(PlayEmmanuelDialogue());
    }


    /////////////////////////////////PLAYER IS SHOWN NOW//////////////////////////////////
    /////////////////////////////////START EMMANUEL DIALOGUE//////////////////////////////

    /*
    IEnumerator PlayEmmanuelDialogue() {
        yield return new WaitForSeconds(.5f);
        Vector2 position = new(0.205f, 0.276f);
        Vector2 direction = position - emmanuelRb.position;
        while (elapseTime <= moveDuration) {
            elapseTime += Time.fixedDeltaTime;
            emmanuelRb.MovePosition(emmanuelRb.position + direction * Time.deltaTime);


            yield return new WaitForFixedUpdate();
        }
        elapseTime = 0;
    }
    */
}
