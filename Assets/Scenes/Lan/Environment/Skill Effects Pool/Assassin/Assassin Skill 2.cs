using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSkill2 : MonoBehaviour
{
    public float finalDamage, additionalDamagePercentage, projectileSpeed, playerID;

    [SerializeField] LanGameManager gmScript;
    Transform player;
    Collider2D[] targetList;

   private void OnEnable() {
    if(playerID != gmScript.player.NetworkObjectId) return;
    player = gmScript.player.transform;
    finalDamage = gmScript.player.finalDamage * 2.5f;
    if(playerID != gmScript.player.NetworkObjectId) return; 
        targetList = Physics2D.OverlapCircleAll(transform.position, 0.35f, 1 << 7);

        if(targetList.Length > 0) { //check if there is enemy detected
            foreach (var item in targetList)
            {
                gmScript.player.AttackServerRpc(item.transform.GetSiblingIndex(), finalDamage, gmScript.player.NetworkObjectId);
            }
        }
   }

   void AnimEvent() {
    Destroy(gameObject);
   }
    
}
