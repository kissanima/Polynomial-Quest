using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill4 : MonoBehaviour
{
    Collider2D[] targetList;
    [SerializeField]LanGameManager gmScript;
    public float finalDamage, additionalDamagePercentage = .5f, playerID, projectileSpeed;
    float elapseTime, flightDuration = 3;
    public Vector2 direction;
    Rigidbody2D rb;
    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
        finalDamage = gmScript.player.finalDamage + 25;

        StartCoroutine(DetectEnemyWait()); 
        StartCoroutine(MoveTo());
    }


    IEnumerator DetectEnemyWait() {   
        while (true) {
            DetectEnemy();   
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator MoveTo() {
        while(elapseTime <= flightDuration) {
            elapseTime += Time.fixedDeltaTime;

            rb.MovePosition(rb.position + (direction * projectileSpeed) * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        
    }


    private void DetectEnemy() { //.2 seconds 
        if(playerID != gmScript.player.NetworkObjectId) return;    //0   0
        targetList = Physics2D.OverlapCircleAll(transform.position, 0.35f, 1 << 7);

        if(targetList.Length > 0) { //check if there is enemy detected
            foreach (var item in targetList)
            {
                gmScript.player.AttackServerRpc(item.transform.GetSiblingIndex(), finalDamage, gmScript.player.NetworkObjectId); //deal damage
            }
        }
    }







}
