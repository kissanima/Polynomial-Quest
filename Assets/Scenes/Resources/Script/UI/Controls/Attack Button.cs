using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    Player player;
    public GameManager gmScript;

    private void Start() {
        player = gmScript.player;
    }

    public void ButtonPressed() {
        if(player.targetList.Length > 0 && player.attackCooldown <= 0) {
            player.Attack();
            player.attackCooldown = 1 / player.attackSpeed;
        }
    }
}
