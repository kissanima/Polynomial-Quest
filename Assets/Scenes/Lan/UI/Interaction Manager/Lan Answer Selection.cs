using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanAnswerSelection : MonoBehaviour
{
    public GameObject[] buttons; //gameobjects
    Transform wrongText, correctText, itemsPoolWS, rewardsLabelPool, reward;
    LanGameManager gmScript;
    LanInteractionManager interactionManager;
    public LanPlayer player;
    LanNpc npc;
    public string[] choices;
    public int answerIndex, attempts, rewardIndexRange, rewardIndex, potionOrItem;
    LanMobsMelee enemy;
    Collider2D enemyCollider;
    bool isAI;


    private void Awake() {
        wrongText = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(2);
        correctText = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(1);
        interactionManager = transform.parent.GetComponent<LanInteractionManager>();
        itemsPoolWS = GameObject.FindWithTag("ItemsPoolWS").transform;
        rewardsLabelPool = GameObject.FindWithTag("RewardsLabelPool").transform;
    }

    private void OnEnable() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>(); //find the game manager object and get the script
        player = gmScript.player; //get player from Game Manager
        npc = player.npc.GetComponent<LanNpc>();

        choices = npc.choices; //answer choices array put in answer variable
        answerIndex = npc.answerIndex;
        attempts = npc.attempts;

        if(npc.isAI) {
            isAI = true;
            enemy = npc.GetComponent<LanMobsMelee>();
            enemyCollider = npc.GetComponent<Collider2D>();
        }

        
        if(!npc.isFillinTheBlanks) {  //isfill in the blanks false, if true, it has choices
            Debug.Log("ïs with choices");
            buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[0]);
            buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[1]);
            buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[2]);
            buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[3]);

            transform.GetChild(0).gameObject.SetActive(true); //enable with choices object

            foreach (var item in buttons)
            {
                item.SetActive(true);
            }
        }
        else if(npc.isFillinTheBlanks) {
            Debug.Log("ïs with isFillinTheBlanks");
            transform.GetChild(1).gameObject.SetActive(true); //enable fill in the blanks object
        }
    }

    public void UseHint() {
        if(gmScript.player.hint > 0) {
            gmScript.player.hint--;
            int randomValue;
            do
            {
                randomValue = Random.Range(0, 4); // draw 0-3 while random value
        } while (randomValue == answerIndex);

        if(randomValue == answerIndex) {
            gmScript.player.hint++;
        }
        transform.GetChild(0).GetChild(randomValue).gameObject.SetActive(false);
        interactionManager.UpdateUI();
        }
    }

    public void MediumConfirmButton() {
        string temp = transform.GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text;
        temp = temp.Replace(" ", "");
        choices[0] = choices[0].Replace(" ", "");
        Debug.Log("Answer is " + choices[0]);


        if (temp.Equals(choices[0], System.StringComparison.OrdinalIgnoreCase)) { //Ignore Upper and lower case
            correctText.gameObject.SetActive(true);
            //transform.parent.gameObject.SetActive(false);
            interactionManager.gameObject.SetActive(false);

            npc.IsDone();


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
                        float rarity = Random.Range(0, 1);
                        int itemType = Random.Range(1, 5); //example 4
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
                            reward = Instantiate(itemsPoolWS.transform.GetChild(20+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .40 && rarity < .70) { //30% uncommon
                            reward = Instantiate(itemsPoolWS.transform.GetChild(22+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .70 && rarity < .85) { //15% rare
                            reward = Instantiate(itemsPoolWS.transform.GetChild(24+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .85 && rarity < .95) { //10% epic
                            reward = Instantiate(itemsPoolWS.transform.GetChild(26+itemType), itemsPoolWS.transform);
                        }
                        else if(rarity >= .95 && rarity <= 1) {  //5% legendary
                            reward = Instantiate(itemsPoolWS.transform.GetChild(28+itemType), itemsPoolWS.transform);
                        }
                    }
                    reward.position = gmScript.player.transform.position;
                    break;
                    
                }
                

                //show reward
                TextMeshProUGUI rewardsLabel = rewardsLabelPool.GetChild(0).GetComponent<TextMeshProUGUI>();
                rewardsLabel.SetText("+1 " + reward.gameObject.name.Replace("(Clone)", "")); 
                rewardsLabel.transform.SetParent(gmScript.player.transform.GetChild(1));
                rewardsLabel.transform.position = gmScript.player.transform.position;
                rewardsLabel.gameObject.SetActive(true);

                //give exp
                gmScript.player.currentExp += 25;
                
                gmScript.player.updateStats();
                gmScript.UpdateUI(); //update player healtbar, exp bar etc
                gmScript.SavePlayerData(); //save data
                
            //}
        }
        else { //if wrong
            attempts -= 1;
            wrongText.gameObject.SetActive(true);
            interactionManager.UpdateUI();

            if(attempts <= 0 && isAI) { //if wrong and attemp is zero
                enemyCollider.enabled = true;
                enemy.isDead = false;
                npc.attempts = 3;

                //heal enemy
                if(enemy.currentHealth.Value <= 0) { //if dead and health is negative
                    enemy.HealServerRpc((enemy.currentHealth.Value * -1f) + (enemy.finalHealth.Value * .10f));
                }
                else {
                    enemy.HealServerRpc(enemy.finalHealth.Value * .10f); 
                }

                transform.parent.gameObject.SetActive(false);
                interactionManager.gameObject.SetActive(false);
            }
            else if(attempts <= 0) { //if wrong and attemp is zero and is station
                transform.parent.gameObject.SetActive(false);
                interactionManager.gameObject.SetActive(false);
            }
        }
    }
}
