using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LanAttackButton : NetworkBehaviour
{
   [SerializeField] LanGameManager gmScript;
    
    
    public void ButtonPressed() {
        if(gmScript.player.targetList.Length > 0 && gmScript.player.attackCooldown <= 0) {
            gmScript.player.Attack();
            gmScript.player.attackCooldown = 1 / gmScript.player.attackSpeed;
        }
    }
}
