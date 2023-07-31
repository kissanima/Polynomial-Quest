using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameManager gmScript;
    public Player player;
    private void Start() {
        player = gmScript.player;
    }
    void OnTriggerEnter2D() {
        player.potion += 1;
        gmScript.UpdateUI();
        gameObject.SetActive(false);
    }
}
