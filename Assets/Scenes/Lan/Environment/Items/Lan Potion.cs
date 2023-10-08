using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanPotion : MonoBehaviour
{
    [SerializeField] LanGameManager gmScript;
    int draw;
    bool isPotion;
    private void Awake() {
        draw = Random.Range(0,2); //0 = potion     1 = hint
        if(draw == 0) {
            isPotion = true;
        }
        else {
            gameObject.name = "Hint";
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(isPotion) {
            gmScript.player.potion += 1;
        }
        else {
            gmScript.player.hint += 1;
        }
        gmScript.UpdateUI();
        Destroy(gameObject);
    }
}
