using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanAnswerButtons : MonoBehaviour
{
    public GameObject correctText, wrongText, interactionManager, itemsPoolWS;
    public LanAnswerSelection answerSelection;
    LanInteractionManager interactionScript;
    LanGameManager gmScript;
    int rewardIndexRange, rewardIndex;
    Vector3 playerPosition;
    Transform rewardsLabelPool;
    LanMobsMelee enemy;
    Collider2D enemyCollider;
    bool isAI;


    private void Start() {
        interactionScript = interactionManager.GetComponent<LanInteractionManager>();
        itemsPoolWS = GameObject.FindWithTag("ItemsPoolWS");
        playerPosition = gmScript.player.transform.position;
        rewardsLabelPool = GameObject.FindWithTag("RewardsLabelPool").transform;
    }

    private void OnEnable() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        bool temp = gmScript.player.npc.GetComponent<LanNpc>().isAI;
        if(temp) {
            isAI = true;
            enemy = gmScript.player.npc.GetComponent<LanMobsMelee>();
            enemyCollider = gmScript.player.npc.GetComponent<Collider2D>();
        }
    }


    public void ButtonPressed() {
        if(answerSelection.answerIndex == transform.GetSiblingIndex()) { //0 //
            correctText.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            interactionManager.SetActive(false);

            interactionScript.npcScript.IsDone();


            //give reward to player
            switch (gmScript.difficulty)
            {
                case 0: //0 means easy difficulty
                rewardIndexRange = 5;
                rewardIndex = Random.Range(0, rewardIndexRange); //draw 0-4
                Transform reward = Instantiate(itemsPoolWS.transform.GetChild(rewardIndex), itemsPoolWS.transform);
                reward.position = gmScript.player.transform.position;
            

                //show reward
                TextMeshProUGUI temp = rewardsLabelPool.GetChild(0).GetComponent<TextMeshProUGUI>();
                temp.SetText("+1 " + reward.gameObject.name.Replace("(Clone)", "")); 
                temp.transform.SetParent(gmScript.player.transform.GetChild(1));
                temp.transform.position = gmScript.player.transform.position;
                temp.gameObject.SetActive(true);
                break;
            }
        }
        else { //if wrong
            answerSelection.attempts -= 1; //subtract attemps
            wrongText.SetActive(true);
            interactionManager.GetComponent<LanInteractionManager>().UpdateUI();

            if(answerSelection.attempts <= 0 && isAI) { //if wrong and attemp is zero
                enemyCollider.enabled = true;
                enemy.isDead = false;
                enemy.GetComponent<LanNpc>().attempts = 3;

                //heal enemy
                if(enemy.currentHealth.Value <= 0) { //if dead and health is negative
                    enemy.HealServerRpc((enemy.currentHealth.Value * -1f) + (enemy.finalHealth.Value * .10f));
                }
                else {
                    enemy.HealServerRpc(enemy.finalHealth.Value * .10f);
                }

                transform.parent.gameObject.SetActive(false);
                interactionManager.SetActive(false);
            }
            else if(answerSelection.attempts <= 0) { //if wrong and attemp is zero and is station
                transform.parent.gameObject.SetActive(false);
                interactionManager.SetActive(false);
            }
        }
    }
}
