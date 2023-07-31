using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButtons : MonoBehaviour
{
    public GameObject correctText, wrongText, interactionManager;
    public AnswerSelection answerSelection;
    public int index;

    public void ButtonPressed() {
        if(answerSelection.answerIndex == index) {
            correctText.SetActive(true);
            interactionManager.SetActive(false);
        }
        else {
            answerSelection.attemps -= 1;
            wrongText.SetActive(true);
            interactionManager.GetComponent<InteractionManager>().UpdateUI();

            if(answerSelection.attemps <= 0) {
                interactionManager.SetActive(false);
            }
        }
    }
}
