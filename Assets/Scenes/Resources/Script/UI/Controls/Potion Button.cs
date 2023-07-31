using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotionButton : MonoBehaviour
{
    public Transform playerCanvas, popPool;
    public GameManager gmScript;
    public Player player;
    public void ButtonPressed() {
        //initialize 25% of finalHealth
        float health = player.finalHealth * .25f;

        if(player.potion > 0) {
            player.potion -= 1;

            //add hp
            if(player.currentHealth >= (player.finalHealth * .75)) {
                health = player.finalHealth - player.currentHealth; //set text
                player.currentHealth += (player.finalHealth - player.currentHealth);
            }
            else {
                player.currentHealth += health;
            }
            gmScript.UpdateUI();

            //spawn a pop
            Transform temp = popPool.GetChild(0);
            temp.GetComponent<TextMeshProUGUI>().SetText(health.ToString());
            temp.GetComponent<TextMeshProUGUI>().color = Color.green;
            temp.SetParent(playerCanvas);
            temp.gameObject.SetActive(true);
        }
    }
}
