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
    public bool isNpc, isAI, isDone;
    Collider2D trigger;

    public void Initialize() {
        trigger = GetComponent<Collider2D>();
        attackButton = GameObject.FindWithTag("Controls").transform.GetChild(1).gameObject;
        interactButton = GameObject.FindWithTag("Controls").transform.GetChild(2).gameObject;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        int draw = Random.Range(0, 10); //draw 0-1
        //GetComponent<LanPlayer>().enabled = true;
        
        
        if(!isNpc) { //0 = easy, 1 = medium
            question = GameObject.FindWithTag("Questionair").transform.GetChild(gmScript.difficulty).GetChild(draw).GetComponent<LanQuestions>().question;
            choices = GameObject.FindWithTag("Questionair").transform.GetChild(gmScript.difficulty).GetChild(draw).GetComponent<LanQuestions>().choices;
            answerIndex = GameObject.FindWithTag("Questionair").transform.GetChild(gmScript.difficulty).GetChild(draw).GetComponent<LanQuestions>().answerIndex;
            attempts = GameObject.FindWithTag("Questionair").transform.GetChild(gmScript.difficulty).GetChild(draw).GetComponent<LanQuestions>().attempts;
        
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && trigger.isTrigger) {
            player = other.GetComponent<LanPlayer>();
            interactButton.SetActive(true);
            attackButton.SetActive(false);
            gmScript.player.npc = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player" && trigger.isTrigger) {
            player = null;
            attackButton.SetActive(true);
            interactButton.SetActive(false);
            gmScript.player.npc = null;
        }
    }

    public void IsDone() {
        //if(isNpc) return;
        gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        Debug.Log("isDone called");
    }

}
