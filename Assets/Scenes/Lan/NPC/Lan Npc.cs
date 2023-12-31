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
    Transform questionair, dungeon;
    public void Initialize() {
        trigger = GetComponent<Collider2D>();
        attackButton = GameObject.FindWithTag("Controls").transform.GetChild(1).gameObject;
        interactButton = GameObject.FindWithTag("Controls").transform.GetChild(2).gameObject;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        questionair = GameObject.FindWithTag("Questionair").transform;
        //GetComponent<LanPlayer>().enabled = true;
        
        //check if is a statue
        if(!isAI && !isNpc) {
            dungeon = transform.parent.parent.parent;
        }
        DrawQuestions();
    }
    
    public void LoadStatue() {
        //server only 
        if(PlayerPrefs.GetInt(transform.GetSiblingIndex().ToString() + transform.parent.GetSiblingIndex().ToString() + transform.parent.parent.GetSiblingIndex().ToString()) == 1 && gmScript.player.IsOwnedByServer) {
                IsDone();
            }
    }
    public void DrawQuestions() {
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
        
        //Debug.Log("isDone called");
        if(!isAI && !isNpc) {  //is a statue

            //first parameter is statue index
            //second parameter is for parent of statue
            //parameter sequence statue, dungeon + difficulty, Dungeon
            gmScript.player.DisableStatueServerRpc(transform.GetSiblingIndex(), transform.parent.GetSiblingIndex(), transform.parent.parent.GetSiblingIndex());

            //server only //save 
            if(gmScript.player.IsOwnedByServer) {
                PlayerPrefs.SetInt(transform.GetSiblingIndex().ToString() + transform.parent.GetSiblingIndex().ToString() + transform.parent.parent.GetSiblingIndex().ToString(), 1);
            }
        }
        else {
            gmScript.player.DisableEnemyServerRpc(transform.GetSiblingIndex());
        }
    }


    private void OnDisable() {
        DrawQuestions();
    }
}
