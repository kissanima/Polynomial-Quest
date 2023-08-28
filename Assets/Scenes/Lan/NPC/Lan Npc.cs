using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanNpc : MonoBehaviour
{
    public GameObject interactButton, attackButton;
    public LanGameManager gmScript;
    public LanPlayer player;
    public string[] question, choices;
    public int answerIndex, attempts;
    public bool isNpc, isAI, isDone, isFillinTheBlanks;
    Collider2D trigger;
    Transform questionair;
    public void Initialize() {
        trigger = GetComponent<Collider2D>();
        attackButton = GameObject.FindWithTag("Controls").transform.GetChild(1).gameObject;
        interactButton = GameObject.FindWithTag("Controls").transform.GetChild(2).gameObject;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        questionair = GameObject.FindWithTag("Questionair").transform;
        //GetComponent<LanPlayer>().enabled = true;
        
        DrawQuestions();
    }
    
    
    void DrawQuestions() {
        if(!isNpc) { //0 = easy, 1 = medium
        int draw = Random.Range(0, questionair.GetChild(gmScript.difficulty).transform.childCount); //draw 0-19 the child count of questonair
        LanQuestions temp = questionair.GetChild(gmScript.difficulty).GetChild(draw).GetComponent<LanQuestions>();
            question = temp.question;
            choices = temp.choices;
            answerIndex = temp.answerIndex;
            attempts = temp.attempts;
            isFillinTheBlanks = temp.isFillinTheBlanks;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && trigger.isTrigger) {
            player = other.GetComponent<LanPlayer>();
            if(!player.IsLocalPlayer) return;
            interactButton.SetActive(true);
            attackButton.SetActive(false);
            gmScript.player.npc = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") && trigger.isTrigger) {
            player = null;
            attackButton.SetActive(true);
            interactButton.SetActive(false);
            gmScript.player.npc = null;
        }
    }

    public void IsDone() {
        //if(isNpc) return;
        gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;  //enemy
        

        if(!isAI && !isNpc) {  //
            gmScript.dungeonStatues++;
            gmScript.UpdateMission();
        }
    }


    private void OnDisable() {
        DrawQuestions();
    }
}
