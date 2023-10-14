using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanAnswerButtons : MonoBehaviour
{
    [SerializeField] GameObject correctText, wrongText, interactionManager, itemsPoolWS;
    public LanAnswerSelection answerSelection;
    [SerializeField] LanInteractionManager interactionScript;
    [SerializeField] LanGameManager gmScript;
    int rewardIndexRange, rewardIndex, potionOrItem;
    Vector3 playerPosition;
    [SerializeField] Transform rewardsLabelPool;
    Transform reward;
    LanMobsMelee enemy;
    Collider2D enemyCollider;
    bool isAI;
    LanNpc npc;
    TextMeshProUGUI textmesh;


    private void Awake() {
        playerPosition = gmScript.player.transform.position;
        textmesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        npc = gmScript.player.npc.GetComponent<LanNpc>();
        bool temp = gmScript.player.npc.GetComponent<LanNpc>().isAI;
        if(temp) {
            isAI = true;
            enemy = gmScript.player.npc.GetComponent<LanMobsMelee>();
            enemyCollider = gmScript.player.npc.GetComponent<Collider2D>();
        }

        //set size
        if(textmesh.text.Length > 5) {
            textmesh.fontSize = 15;
        }
        else {
            textmesh.fontSize = 24;
        }
    }


    public void ButtonPressed() {
        if(answerSelection.answerIndex == transform.GetSiblingIndex()) { //0 //
            correctText.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            interactionManager.SetActive(false);
            gmScript.correctSound.Play(); //play sound
            interactionScript.npcScript.IsDone(); //method to disable gameobjects
            gmScript.player.GiveRewardsServerRpc(25f);

                /*  REMOVED/TRANSFERRED to player script networking block
                
                potionOrItem = Random.Range(0, 3); //draw 0-2, if 0 give potion or hint, if 1 give item, 2 give armor
                float rarity = Random.Range(0, 1); //the rarity of the weapon/armor
                int itemIndex;
                    if(potionOrItem == 0) { //give potion
                        reward = Instantiate(itemsPoolWS.transform.GetChild(0), itemsPoolWS.transform);
                    }
                    else if(potionOrItem == 2) {
                        itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(7).childCount);
                        reward = Instantiate(itemsPoolWS.transform.GetChild(7).GetChild(itemIndex), itemsPoolWS.transform);
                    }
                    else { //draw item and give as a reward
                        
                        if(rarity >= 0 && rarity < .40) { //chance 40% common
                            itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(1).childCount);
                            reward = Instantiate(itemsPoolWS.transform.GetChild(1).GetChild(itemIndex), itemsPoolWS.transform);
                        }
                        else if(rarity >= .40 && rarity < .70) { //30% uncommon
                            itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(2).childCount);
                            reward = Instantiate(itemsPoolWS.transform.GetChild(2).GetChild(itemIndex), itemsPoolWS.transform);
                        }
                        else if(rarity >= .70 && rarity < .85) { //15% rare
                            itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(3).childCount);
                            reward = Instantiate(itemsPoolWS.transform.GetChild(3).GetChild(itemIndex), itemsPoolWS.transform);
                        }
                        else if(rarity >= .85 && rarity < .95) { //10% epic
                            itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(4).childCount);
                            reward = Instantiate(itemsPoolWS.transform.GetChild(4).GetChild(itemIndex), itemsPoolWS.transform);
                        }
                        else if(rarity >= .95 && rarity <= 1) {  //5% legendary
                            itemIndex = Random.Range(0, itemsPoolWS.transform.GetChild(5).childCount);
                            reward = Instantiate(itemsPoolWS.transform.GetChild(5).GetChild(itemIndex), itemsPoolWS.transform);
                        }
                    }
                    reward.position = gmScript.player.transform.position;


                show reward
                TextMeshProUGUI temp = rewardsLabelPool.GetChild(0).GetComponent<TextMeshProUGUI>();
                temp.SetText("+1 " + reward.gameObject.name.Replace("(Clone)", "")); 
                temp.transform.SetParent(gmScript.player.transform.GetChild(1));
                temp.transform.position = gmScript.player.transform.position;
                temp.gameObject.SetActive(true);
                

                
                
                gmScript.player.score.Value += 100;
                gmScript.player.UpdateStats();
                gmScript.UpdateUI(); //update player healtbar, exp bar etc
                gmScript.SavePlayerData(); //save data 
            }  */
        }
        else { //if wrong
            answerSelection.attempts -= 1; //subtract attemps
            gmScript.wrongSound.Play();
            wrongText.SetActive(true);
            interactionScript.UpdateUI();

            if(answerSelection.attempts <= 0 && isAI) { //if wrong and attemp is zero and its an enemy
                enemyCollider.enabled = true;
                enemy.isDead = false;
                enemy.target = null;
                enemy.GetComponent<LanNpc>().attempts = 2;

                //heal enemy
                if(enemy.currentHealth.Value <= 0) { //if dead and health is negative
                    enemy.HealServerRpc((enemy.currentHealth.Value * -1f) + (enemy.finalHealth.Value * .10f));
                }
                else {
                    enemy.HealServerRpc(enemy.finalHealth.Value * .10f);
                }
                transform.parent.gameObject.SetActive(false); //disable 
                interactionManager.SetActive(false);
                npc.DrawQuestions();
            }
            else if(answerSelection.attempts <= 0) { //if wrong and attemp is zero and is station
                transform.parent.gameObject.SetActive(false);
                interactionManager.SetActive(false);
                npc.DrawQuestions();
            }
        }
    }
}
