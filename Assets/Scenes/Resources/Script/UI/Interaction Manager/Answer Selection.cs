using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerSelection : MonoBehaviour
{
    public GameObject[] buttons; //gameobjects
    public Player player;
    public string[] choices;
    public int answerIndex, attemps = 2;

    private void OnEnable() {
        choices = player.npc.GetComponent<Emanuel>().choices; //answer choices array put in answer variable
        answerIndex = player.npc.GetComponent<Emanuel>().answerIndex;
        //a.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(answer[0].ToString());
        //b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(answer[1].ToString());
        //c.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(answer[2].ToString());
        //d.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(answer[3].ToString()); 
    }

    public void UseHint() {
        int randomValue;
        do
        {
            randomValue = Random.Range(0, 4); // draw 0-33 while random value
        } while (randomValue == answerIndex);

        transform.parent.GetChild(randomValue).gameObject.SetActive(false);
    }
}
