using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public int index, attemps;
    public Player player;
    TextMeshProUGUI textBox, attempsText;
    Emanuel npcScript;
    public GameObject continueButton, answerSelection, attempsCount, attempsLabel;

    private void Awake() {
        textBox = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        attempsText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        npcScript = player.npc.GetComponent<Emanuel>();
        textBox.SetText(npcScript.question[index]);
        continueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Continue");

        if(npcScript.isNpc) {
            attempsCount.SetActive(false);
            attempsLabel.SetActive(false);
        }
        
        UpdateUI();
    }

    public void Continue() {
        if((npcScript.question.Length - 1) == index && !npcScript.isNpc) { //if index is equal to the length of text array of the npc, show the question
            continueButton.SetActive(false); //and object is not npc
            answerSelection.SetActive(true);
            textBox.SetText(npcScript.question[index]);
        }
        else if(npcScript.question.Length == index && npcScript.isNpc)
        {
            gameObject.SetActive(false);
            index = 0;
        }
        else {
            if ((npcScript.question.Length -1) == index)
            {
                continueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Exit");
            }
            textBox.SetText(npcScript.question[index]);
            index += 1;
        }
    }

    public void UpdateUI() {
        attempsText.SetText(answerSelection.GetComponent<AnswerSelection>().attemps.ToString());
    }

}
