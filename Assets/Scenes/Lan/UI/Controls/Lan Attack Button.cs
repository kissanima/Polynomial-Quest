using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LanAttackButton : NetworkBehaviour
{
     public LanPlayer player;

    public override void OnNetworkSpawn()
    {

        LanPlayer[] players = FindObjectsOfType<LanPlayer>();
        foreach (LanPlayer p in players)
        {
            if (p.IsLocalPlayer)
            {
                player = p;
                break;
            }
        }
    }
    
    
    public void ButtonPressed() {
        if(player.targetList.Length > 0 && player.attackCooldown <= 0) {
            player.Attack();
            player.attackCooldown = 1 / player.attackSpeed;
        }
    }
}
