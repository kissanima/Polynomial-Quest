using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanAnswerSelection : MonoBehaviour
{
    public GameObject[] buttons; //gameobjects
    LanGameManager gmScript;
    public LanPlayer player;
    LanNpc npc;
    public string[] choices;
    public int answerIndex, attempts;


    private void OnEnable() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>(); //find the game manager object and get the script
        player = gmScript.player; //get player from Game Manager
        npc = player.npc.GetComponent<LanNpc>();

        choices = npc.choices; //answer choices array put in answer variable
        answerIndex = npc.answerIndex;
        attempts = npc.attempts;

        

        

        switch (gmScript.difficulty)
        {
            
            case 0: //easy difficulty
            buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[0]);
            buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[1]);
            buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[2]);
            buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choices[3]);

            transform.GetChild(0).gameObject.SetActive(true); //enable answer selection
            break;

            case 1: //medium difficulty
            transform.GetChild(1).gameObject.SetActive(true); //enable answer selection
            break;

            case 2: //hard difficulty
            break;

            case 3: //extreme difficulty
            break;
        }
    }

    public void UseHint() {
        int randomValue;
        do
        {
            randomValue = Random.Range(0, 4); // draw 0-3 while random value
        } while (randomValue == answerIndex);

        transform.GetChild(randomValue).gameObject.SetActive(false);
    }

    public void mediumConfirmButton() {
        string temp = transform.GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text;
        temp = temp.Replace(" ", "");
        choices[0] = choices[0].Replace(" ", "");
        Debug.Log("Answer is " + choices[0]);


        if (temp.Equals(choices[0], System.StringComparison.OrdinalIgnoreCase)) { //Ignore Upper and lower case
        Debug.Log("ANSWER IS CORRECT <3");

        }
        else {
            Debug.Log("ANSWER IS WRONG");
        }
    }
}
