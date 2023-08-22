using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanPotion : MonoBehaviour
{
    public LanGameManager gmScript;
    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        gmScript.player.potion += 1;
        Destroy(gameObject);
    }
}
