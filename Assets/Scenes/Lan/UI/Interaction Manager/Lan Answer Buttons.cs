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
    int rewardIndexRange, rewardIndex, potionOrItem;
    Vector3 playerPosition;
    Transform rewardsLabelPool, reward;
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

            gmScript.correctSound.Play();

            interactionScript.npcScript.IsDone();


            //give reward to player
            //switch (gmScript.difficulty)
            //{
                //case 0: //0 means easy difficulty
                switch (gmScript.player.playerClass)
                {
                    case "Warrior":
                    potionOrItem = Random.Range(0, 2); //draw 0-1, if 0 give potion, if 1 give item
                    if(potionOrItem == 0) { //give potion
                    Debug.Log("Potion given");
                        reward = Instantiate(itemsPoolWS.transform.GetChild(0), itemsPoolWS.transform);
                    }
                    else { //draw item and give as a reward
                        float rarity = Random.Range(0, 1); //.05
                        int itemType = Random.Range(1, 5); //example 3
                        if(rarity >= 0 && rarity < .40) { //chance 40% common
                            reward = Instantiate(itemsPoolWS.transform.GetChild(itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .40 && rarity < .70) { //30% uncommon
                            reward = Instantiate(itemsPoolWS.transform.GetChild(itemType+4), itemsPoolWS.transform);
                        }
                        else if(rarity >= .70 && rarity < .85) { //15% rare
                            reward = Instantiate(itemsPoolWS.transform.GetChild(itemType+8), itemsPoolWS.transform);
                        }
                        else if(rarity >= .85 && rarity < .95) { //10% epic
                            reward = Instantiate(itemsPoolWS.transform.GetChild(itemType+12), itemsPoolWS.transform);
                        }
                        else if(rarity >= .95 && rarity <= 1) {  //5% legendary
                            reward = Instantiate(itemsPoolWS.transform.GetChild(itemType+16), itemsPoolWS.transform);
                        }
                    }
                    reward.position = gmScript.player.transform.position;
                    break;

                    case "Mage":
                    potionOrItem = Random.Range(0, 2); //draw 0-1, if 0 give potion, if 1 give item
                    if(potionOrItem == 0) { //give potion
                        reward = Instantiate(itemsPoolWS.transform.GetChild(0), itemsPoolWS.transform);
                    }
                    else { //draw item and give as a reward
                        float rarity = Random.Range(0, 1);
                        int itemType = Random.Range(1, 3); //2
                        if(rarity >= 0 && rarity < .40) { //chance 40% common
                            Debug.Log("common");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(20+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .40 && rarity < .70) { //30% uncommon
                        Debug.Log("uncommon");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(22+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .70 && rarity < .85) { //15% rare
                        Debug.Log("rare");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(24+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .85 && rarity < .95) { //10% epic
                        Debug.Log("epic");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(26+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .95 && rarity <= 1) {  //5% legendary
                        Debug.Log("legendary");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(28+itemType), itemsPoolWS.transform);
                        }
                    }
                    reward.position = gmScript.player.transform.position;
                    break;
                    

                    case "Assassin":
                    potionOrItem = Random.Range(0, 2); //draw 0-1, if 0 give potion, if 1 give item
                    if(potionOrItem == 0) { //give potion
                        reward = Instantiate(itemsPoolWS.transform.GetChild(0), itemsPoolWS.transform);
                    }
                    else { //draw item and give as a reward
                        float rarity = Random.Range(0, 1);
                        int itemType = 1; 
                        if(rarity >= 0 && rarity < .40) { //chance 40% common
                            Debug.Log("common");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(30+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .40 && rarity < .70) { //30% uncommon
                        Debug.Log("uncommon");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(31+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .70 && rarity < .85) { //15% rare
                        Debug.Log("rare");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(32+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .85 && rarity < .95) { //10% epic
                        Debug.Log("epic");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(33+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .95 && rarity <= 1) {  //5% legendary
                        Debug.Log("legendary");
                            reward = Instantiate(itemsPoolWS.transform.GetChild(34+itemType), itemsPoolWS.transform);
                        }
                    }
                    reward.position = gmScript.player.transform.position;
                    break;
                }


                //show reward
                TextMeshProUGUI temp = rewardsLabelPool.GetChild(0).GetComponent<TextMeshProUGUI>();
                temp.SetText("+1 " + reward.gameObject.name.Replace("(Clone)", "")); 
                temp.transform.SetParent(gmScript.player.transform.GetChild(1));
                temp.transform.position = gmScript.player.transform.position;
                temp.gameObject.SetActive(true);
                //break;

                //give exp
                gmScript.player.currentExp += 25;
                gmScript.player.score.Value += 100;
                gmScript.player.updateStats();
                gmScript.UpdateUI(); //update player healtbar, exp bar etc
                gmScript.SavePlayerData(); //save data
            //}
        }
        else { //if wrong
            answerSelection.attempts -= 1; //subtract attemps
            gmScript.wrongSound.Play();
            wrongText.SetActive(true);
            interactionManager.GetComponent<LanInteractionManager>().UpdateUI();

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
            }
            else if(answerSelection.attempts <= 0) { //if wrong and attemp is zero and is station
                transform.parent.gameObject.SetActive(false);
                interactionManager.SetActive(false);
            }
        }
    }
}
