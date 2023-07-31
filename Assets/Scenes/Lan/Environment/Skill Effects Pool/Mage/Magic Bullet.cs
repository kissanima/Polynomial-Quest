using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    LanGameManager gmScript;
    public float finalDamage, additionalDamagePercentage = .5f, projectileSpeed;
    float elapseTime, flightDuration = 2;
    public Vector2 targetPosition;
    Rigidbody2D rb;
    Transform parent;

    private void Awake() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        rb = GetComponent<Rigidbody2D>();
        parent = transform.parent.parent.GetChild(0);
    }

     private void OnEnable() {
        StartCoroutine(MovePosition());
        finalDamage = gmScript.player.finalDamage;
    }



    IEnumerator MovePosition() {
    while(elapseTime <= flightDuration) {
        elapseTime += Time.fixedDeltaTime;

        //Vector2 direction = target.position - transform.position;
        //rotate projectile
        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50 * Time.deltaTime);

        rb.MovePosition(rb.position + (targetPosition * projectileSpeed) * Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
    }
    transform.SetParent(parent);
    gameObject.SetActive(false);
    elapseTime = 0;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        MobsMelee enemy;
        if(other.tag == "Enemy") {
            enemy = other.GetComponent<MobsMelee>();


            //disable projectile
            transform.SetParent(parent);
            gameObject.SetActive(false);
        }
    }
   
    
}
