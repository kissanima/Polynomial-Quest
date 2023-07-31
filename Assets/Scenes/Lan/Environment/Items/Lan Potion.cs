using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanPotion : MonoBehaviour
{
    public LanGameManager gmScript;
    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }
    void OnTriggerEnter2D() {
        gmScript.player.potion += 1;

        gmScript.SavePlayerData();
        gmScript.UpdateUI();
        gameObject.SetActive(false);
    }
}
