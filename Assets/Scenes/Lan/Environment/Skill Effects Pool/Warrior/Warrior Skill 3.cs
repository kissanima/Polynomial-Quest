using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill3 : MonoBehaviour
{
    Collider2D[] targetList;
    LanGameManager gmScript;
    public float finalDamage, additionalDamagePercentage = 150f;
    Transform skillEffectsPool;
    
    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        skillEffectsPool = GameObject.FindWithTag("SkillEffects").transform;
        finalDamage = (gmScript.player.finalDamage * (additionalDamagePercentage / 100)) + 100f;  //150 / 100 = 1.5 
    }

    

    void DetecEnemy() { //animation event
        targetList = Physics2D.OverlapCircleAll(transform.position, 0.25f, 1 << 7);

        if(targetList.Length > 0) { //check if there is enemy detected
            foreach (var item in targetList)
            {
                gmScript.player.AttackServerRpc(item.transform.GetSiblingIndex(), finalDamage, gmScript.player.OwnerClientId); 
            }
            
        }
    }

    void OnDrawGizmos() {    
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .25f);
    }

    void Finish() { //animation event
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }



    
}
