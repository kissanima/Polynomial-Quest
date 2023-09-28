using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill3 : MonoBehaviour
{
    Collider2D[] targetList;
    [SerializeField] LanGameManager gmScript;
    public float finalDamage, additionalDamagePercentage = 150f, ownerID;
    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void OnEnable() {
        finalDamage = (gmScript.player.finalDamage * (additionalDamagePercentage / 100)) + 100f;  //150 / 100 = 1.5 

        //play sound effect
        audioSource.Play();
    }

    

    void DetecEnemy() { //animation event
        if(ownerID != gmScript.player.NetworkObjectId) return;
        targetList = Physics2D.OverlapCircleAll(transform.position, 0.25f, 1 << 7);

        if(targetList.Length > 0) { //check if there is enemy detected
            foreach (var item in targetList)
            {
                gmScript.player.AttackServerRpc(item.transform.GetSiblingIndex(), finalDamage, gmScript.player.NetworkObjectId); 
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
