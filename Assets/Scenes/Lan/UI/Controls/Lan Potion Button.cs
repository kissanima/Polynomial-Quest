using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanPotionButton : MonoBehaviour
{
    public Transform playerCanvas, popPool;
    public LanGameManager gmScript;

    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        popPool = GameObject.FindWithTag("DamagePool").transform;
    }
    public void ButtonPressed() {
        playerCanvas = gmScript.player.transform.GetChild(1);

        //initialize 25% of finalHealth
        float health = gmScript.player.finalHealth.Value * .25f;

        if(gmScript.player.potion > 0) {
            gmScript.player.potion -= 1;

            //add hp
            if(gmScript.player.currentHealth.Value >= (gmScript.player.finalHealth.Value * .75)) {
                health = gmScript.player.finalHealth.Value - gmScript.player.currentHealth.Value; //set text
                gmScript.player.currentHealth.Value += (gmScript.player.finalHealth.Value - gmScript.player.currentHealth.Value);
            }
            else {
                gmScript.player.currentHealth.Value += health;
            }
            gmScript.SavePlayerData();
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
