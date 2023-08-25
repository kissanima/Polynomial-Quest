using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanInteractionManager : MonoBehaviour
{
    public int index, attemps;
    public LanPlayer player;
    TextMeshProUGUI textBox, attempsText, continueButtonText;
    LanGameManager gmScript;
    public LanNpc npcScript;
    public GameObject continueButton, answerSelection, attempsCount, attempsLabel;

    AudioSource questionairSoundEffect;


    private void Awake() {
        textBox = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        attempsText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        continueButton = GameObject.FindWithTag("InteractionManager").transform.GetChild(4).gameObject;
        answerSelection = GameObject.FindWithTag("InteractionManager").transform.GetChild(5).gameObject;
        attempsCount = GameObject.FindWithTag("InteractionManager").transform.GetChild(2).gameObject;
        attempsLabel = GameObject.FindWithTag("InteractionManager").transform.GetChild(3).gameObject;
        questionairSoundEffect = GetComponent<AudioSource>();

        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();

        continueButtonText = continueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    

    private void OnEnable() {
        player = gmScript.player;
        npcScript = player.npc.GetComponent<LanNpc>();
        continueButton.SetActive(true);

        textBox.SetText("Follow the direction to solve the questions. ");
        continueButtonText.SetText("Continue");

        if(npcScript.isNpc) { //
            attempsCount.SetActive(false);
            attempsLabel.SetActive(false);
        }
        else {
            attempsCount.SetActive(true);
            attempsLabel.SetActive(true);

            //play questionair sound
            questionairSoundEffect.Play();
        }

        
        UpdateUI();
    }

    public void Continue() {
        if((npcScript.question.Length - 1) == index && !npcScript.isNpc) { //if index is equal to the length of text array of the npc, show the question   0
            continueButton.SetActive(false); //and object is not npc
            answerSelection.SetActive(true);
            
            
            switch (gmScript.difficulty)
            {
            case 0: //easy difficulty
            textBox.SetText(npcScript.question[index]);
            break;

            case 1: //medium difficulty
            textBox.SetText(npcScript.question[index]);
            break;
            }
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
        attempsText.SetText(answerSelection.GetComponent<LanAnswerSelection>().attempts.ToString());
        transform.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("hints: " + gmScript.player.hint.ToString());
    }

}
