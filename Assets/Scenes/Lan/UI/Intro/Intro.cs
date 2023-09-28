using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class Intro : MonoBehaviour
{
    [SerializeField] GameObject skipButton;
    Rigidbody2D emmanuelRb, wilsonRb; //movePosition()
    Transform npcParent, ui, tutorialPanel, wilson;
    TextMeshProUGUI emmanueltextBox, wilsonTextBox, tutorialTextBox;
    [SerializeField]string[] emmanuelDialogues, wilsonDialogues;
    LanGameManager gmScript;
    Animator emmanuelAnim, wilsonAnim, arrowAnim;
    [SerializeField]AudioClip[] audioClips;
    AudioSource audioSource;

    Vector3 rotation1 = new(0,0,-160);
    Vector3 position1 = new(-185,-16,0);

    Vector3 rotation2 = new(0,0,32.258f);
    Vector3 position2 = new(284,186,0);

    Vector3 rotation3 = new(0,0,0);
    Vector3 position3 = new(545,309,0);

    Vector3 rotation4 = new(0,0,0);
    Vector3 position4 = new(529,170,0);

    Vector3 rotation5 = new(0,0,-27.785f);
    Vector3 position5 = new(94,111,0);

    Vector3 rotation6 = new(0,0,-90);
    Vector3 position6 = new(173,64,0);

    Vector3 rotation7 = new(0,0,-90);
    Vector3 position7 = new(34,64,0);

    float elapseTime, moveDuration = 1f, index;
    [SerializeField]LanMobsMelee[] attackingMonsters;

    private void Awake() {
        npcParent = GameObject.Find("Npc").transform;
        emmanueltextBox = npcParent.GetChild(2).GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();
        emmanuelRb = npcParent.GetChild(2).GetComponent<Rigidbody2D>();
        emmanuelAnim = emmanuelRb.GetComponent<Animator>();

        wilsonTextBox = npcParent.GetChild(3).GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();
        wilson = npcParent.GetChild(3).transform;


        ui = GameObject.FindWithTag("UI").transform;
        tutorialPanel = ui.GetChild(12);
        tutorialTextBox = tutorialPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        arrowAnim = tutorialPanel.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void PlayEmmanuelCourotine() {
        StartCoroutine(PlayEmmanuelDialogue());
    }

    IEnumerator PlayEmmanuelDialogue() {
        yield return new WaitForSeconds(.5f);
        audioSource.clip = audioClips[0];
        audioSource.Play();

        Vector3 position = new(0.205f, 0.143f); //0-1
        Vector2 direction = (position - emmanuelRb.transform.position).normalized * .5f;
        while (elapseTime <= moveDuration) { //0   //1
            emmanuelAnim.SetBool("isMoving", true);  //running
            elapseTime += Time.fixedDeltaTime;  
            emmanuelRb.MovePosition(emmanuelRb.position + direction * Time.deltaTime);


            yield return new WaitForFixedUpdate();
        }
        emmanuelAnim.SetBool("isMoving", false); //elapseTime == moveDuration
        elapseTime = 0;
        emmanueltextBox.transform.parent.gameObject.SetActive(true); //show text box

        //start text 1   ///typing effect
        foreach (var item in emmanuelDialogues[0])
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(.5f); // pause
        emmanueltextBox.text = null;

        //start text 2
        foreach (var item in emmanuelDialogues[1])
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        
        ///////////////////DUB 2 start
        yield return new WaitForSeconds(1f); // pause
        audioSource.clip = audioClips[1];
        audioSource.Play();
        emmanueltextBox.text = null;

        //start text 3
        string temp1 = "Wonderful! You are " + gmScript.player.username + " going forward. Remember that your best tool when entering is knowledge.";
        foreach (var item in temp1) //entering is knowledge.
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f); // pause
        skipButton.SetActive(true);
        emmanueltextBox.text = null;

        //start text 3
        foreach (var item in emmanuelDialogues[2]) //this realm works.
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        //start show tutorial screen
        StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial() {
        yield return new WaitForSeconds(1);
        ui.GetChild(2).gameObject.SetActive(true); // show controls
        ui.GetChild(6).gameObject.SetActive(true); // show health bars
        ui.GetChild(12).gameObject.SetActive(true); // show tutorial

        tutorialTextBox.text = "This is your Controls";

        /*
        yield return new WaitForSeconds(1);
        tutorialTextBox.text = null; //clear text box

        // sequence 1
        tutorialTextBox.text = "Use this to move your character";
        tutorialPanel.GetChild(0).eulerAngles = rotation1;
        tutorialPanel.GetChild(0).localPosition = position1;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(2);
  


        //sequence 2
        tutorialTextBox.text = "This is your character name, level, health and mana.";
        tutorialPanel.GetChild(0).eulerAngles = rotation2;
        tutorialPanel.GetChild(0).localPosition = position2;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(2);
        tutorialPanel.GetChild(0).gameObject.SetActive(false); //disable arrow object

        //sequence 3
        tutorialTextBox.text = "This is a button that will show a panel for your character stats";
        tutorialPanel.GetChild(0).eulerAngles = rotation3;
        tutorialPanel.GetChild(0).localPosition = position3;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(4);
        tutorialPanel.GetChild(0).gameObject.SetActive(false); //disable arrow object
        
        //sequence 4
        tutorialTextBox.text = "This is a button that will show a panel for your character inventory, if you get an item, this is where you equip it, but certain items have class restrictions.";
        tutorialPanel.GetChild(0).eulerAngles = rotation4;
        tutorialPanel.GetChild(0).localPosition = position4;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(2);
        tutorialPanel.GetChild(0).gameObject.SetActive(false); //disable arrow object

        //sequence 5
        tutorialTextBox.text = "This are your skills and your basic attack button, This is also where an interact button will show up if you get near an NPC";
        tutorialPanel.GetChild(0).eulerAngles = rotation5;
        tutorialPanel.GetChild(0).localPosition = position5;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(3);
        tutorialPanel.GetChild(0).gameObject.SetActive(false); //disable arrow object

        //sequence 6
        tutorialTextBox.text = "This is a spell that you can use to escape danger and avoid the penalty of dying! you can blink to a short distance but has 25 seconds cooldown so use it wisely.";
        tutorialPanel.GetChild(0).eulerAngles = rotation6;
        tutorialPanel.GetChild(0).localPosition = position6;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object

        yield return new WaitForSeconds(2);
        tutorialPanel.GetChild(0).gameObject.SetActive(false); //disable arrow object

        //sequence 7
        tutorialTextBox.text = "health Potion! use this when your character is low on health, you can get more potions easily!";
        tutorialPanel.GetChild(0).eulerAngles = rotation7;
        tutorialPanel.GetChild(0).localPosition = position7;
        tutorialPanel.GetChild(0).gameObject.SetActive(true); //show arrow object
        */
    }

    public void NextButton() { //index = 1
        if(index == 0) {
            tutorialTextBox.text = null; //clear text box

            // sequence 1
            tutorialTextBox.text = "Use this to move your character";
            tutorialPanel.GetChild(0).eulerAngles = rotation1;
            tutorialPanel.GetChild(0).localPosition = position1;

            index++; //increment
        }
        else if(index == 1) { 
            tutorialTextBox.text = "This is your character name, level, health and mana.";
            tutorialPanel.GetChild(0).eulerAngles = rotation2;
            tutorialPanel.GetChild(0).localPosition = position2;

            index++;
        }
        else if(index == 2) {
            tutorialTextBox.text = "This is a button that will show a panel for your character stats";
            tutorialPanel.GetChild(0).eulerAngles = rotation3;
            tutorialPanel.GetChild(0).localPosition = position3;

            index++;
        }
        else if(index == 3) {
            tutorialTextBox.text = "This is a button that will show a panel for your character inventory, if you get an item, this is where you equip it, but certain items have class restrictions.";
            tutorialPanel.GetChild(0).eulerAngles = rotation4;
            tutorialPanel.GetChild(0).localPosition = position4;

            index++;
        }
        else if(index == 4) {
            tutorialTextBox.text = "These are your skills and your basic attack button, This is also where an interact button will show up if you get near an NPC";
            tutorialPanel.GetChild(0).eulerAngles = rotation5;
            tutorialPanel.GetChild(0).localPosition = position5;

            index++;
        }
        else if(index == 5) {
            tutorialTextBox.text = "This is a spell that you can use to escape danger and avoid the penalty of dying! you can blink to a short distance but has 25 seconds cooldown so use it wisely.";
            tutorialPanel.GetChild(0).eulerAngles = rotation6;
            tutorialPanel.GetChild(0).localPosition = position6;
            index++;
        }
        else if(index == 6) {
            tutorialTextBox.text = "health Potion! use this when your character is low on health, you can get more potions easily!";
            tutorialPanel.GetChild(0).eulerAngles = rotation7;
            tutorialPanel.GetChild(0).localPosition = position7;
            tutorialPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "FINISH";
            index++;
        }

        else if(index == 7) {
            tutorialPanel.gameObject.SetActive(false);

            //close controls and health bar
            ui.GetChild(2).gameObject.SetActive(false); // hide controls
            ui.GetChild(6).gameObject.SetActive(false); // hide health bars
            ui.GetChild(12).gameObject.SetActive(false); // hide tutorial


            StartCoroutine(PlayEmmanuelDialogue2());
        }
    }

    IEnumerator PlayEmmanuelDialogue2() {
        emmanueltextBox.text = null;
        audioSource.clip = audioClips[2];
        audioSource.Play();
        yield return new WaitForSeconds(0.5f); //pause

        //start text 4
        foreach (var item in emmanuelDialogues[3])
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f); // pause
        emmanueltextBox.text = null;

        //start text 5
        foreach (var item in emmanuelDialogues[4])
        {
            emmanueltextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }
 
        yield return new WaitForSeconds(1f); //pause               /////////////////////////////////////WILSON DIALOGUE START HERE////////////////////////////////////
        emmanueltextBox.transform.parent.gameObject.SetActive(false); //hide text box

        //teleport wilson
        wilson.localPosition = new(0.602f, 0.165f);


        audioSource.clip = audioClips[3];
        audioSource.Play();
        yield return new WaitForSeconds(.5f); //pause
        wilsonTextBox.transform.parent.gameObject.SetActive(true);
        string tempwilson = "Good afternoon, " + gmScript.player.username + " Wilson, the Holy Angel, is who I am.";
        //start wilson text 1
        foreach (var item in tempwilson)
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(1f); //pause
        wilsonTextBox.text = null;
        foreach (var item in wilsonDialogues[0])
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(1f); //pause
        wilsonTextBox.text = null;
        foreach (var item in wilsonDialogues[1])
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(1f); //pause
        wilsonTextBox.text = null;
        foreach (var item in wilsonDialogues[2])
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }


        audioSource.clip = audioClips[4];
        audioSource.Play();
        yield return new WaitForSeconds(1f); //pause
        wilsonTextBox.text = null;
        foreach (var item in wilsonDialogues[3])
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(.1f); //pause
        wilsonTextBox.text = null;
        foreach (var item in wilsonDialogues[4])
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }



        yield return new WaitForSeconds(1f); //pause
        wilsonTextBox.text = null;
        string tempwilson1 = "Now, " + gmScript.player.username + " , go forth on your adventure; may it be productive and successful.";
        foreach (var item in tempwilson1)
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        
        yield return new WaitForSeconds(1.5f); //pause
        wilsonTextBox.text = null; 
        foreach (var item in wilsonDialogues[5]) //oh no monsters are attacking
        {
            wilsonTextBox.text += item;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f); //pause
        foreach (var item in attackingMonsters) 
        {
            item.target = item.attackersTarget;
            item.isAttacking = true;
        }





        wilsonTextBox.transform.parent.gameObject.SetActive(false); //textbox disable
        ui.GetChild(2).gameObject.SetActive(true); // true controls
        ui.GetChild(6).gameObject.SetActive(true); // true health bars
        ui.GetChild(10).gameObject.SetActive(true); // show minimap
        ui.GetChild(14).gameObject.SetActive(true);
        skipButton.SetActive(false);
        gmScript.player.finishIntro = 1;
        gmScript.SavePlayerData();
    }


    public void SkipButton() {
        gameObject.SetActive(false);
        skipButton.SetActive(false);
        emmanueltextBox.transform.parent.gameObject.SetActive(false); //disable emmanuel text box
        wilsonTextBox.transform.parent.gameObject.SetActive(false); //textbox disable
        ui.GetChild(2).gameObject.SetActive(true); // true controls
        ui.GetChild(6).gameObject.SetActive(true); // true health bars
        ui.GetChild(10).gameObject.SetActive(true); // show minimap
        ui.GetChild(12).gameObject.SetActive(false); //disable tutorial
        ui.GetChild(14).gameObject.SetActive(true); //show mission panel
        gmScript.player.finishIntro = 1;
        gmScript.SavePlayerData();
    }
}
