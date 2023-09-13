using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviorOptimizer : MonoBehaviour
{
    LanMobsMelee mob;
    Animator anim;
    private void Start() {
        mob = transform.parent.GetComponent<LanMobsMelee>();
        anim = mob.transform.GetChild(3).GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            mob.enabled = true;
            anim.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            mob.enabled = false;
            anim.enabled = false;
        }
    }
}
