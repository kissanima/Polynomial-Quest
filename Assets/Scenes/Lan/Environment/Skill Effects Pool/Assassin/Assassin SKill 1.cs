using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSKill1 : MonoBehaviour
{
    public float finalDamage, additionalDamagePercentage, projectileSpeed, playerID;
    float elapseTime, flightDuration = 2;


    [SerializeField]LanGameManager gmScript;
    Rigidbody2D rb;
    Transform parent, player;
    public Vector2 direction;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        parent = transform.parent.parent.GetChild(0);
    }

     private void OnEnable() {
        projectileSpeed = 3;
        player = gmScript.player.transform;
        StartCoroutine(MovePosition());
        finalDamage = gmScript.player.finalDamage * (additionalDamagePercentage / 100) + gmScript.player.finalDamage;

        StartCoroutine(FlightDuration()); //3 seconds to destroy this gameobject
    }


    IEnumerator MovePosition() {
    while(elapseTime <= flightDuration) { //elapseTime == flightDuration
        elapseTime += Time.fixedDeltaTime;

        //Vector2 direction = target.position - transform.position;
        //rotate projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50 * Time.deltaTime);

        rb.MovePosition(rb.position + (direction * projectileSpeed) * Time.fixedDeltaTime);
        yield return new WaitForFixedUpdate();
    }
    transform.SetParent(parent);
    gameObject.SetActive(false);
    elapseTime = 0;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(playerID != gmScript.player.NetworkObjectId) return;
        LanMobsMelee enemy;
        if(other.CompareTag("Enemy")) {
            enemy = other.GetComponent<LanMobsMelee>();
            gmScript.player.AttackServerRpc(other.transform.GetSiblingIndex(), finalDamage, gmScript.player.NetworkObjectId); //deal damage
            //teleport player to this projectile current position
            player.position = transform.position; 

            //destroy
            Destroy(gameObject);
        }
    }

    IEnumerator FlightDuration() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
