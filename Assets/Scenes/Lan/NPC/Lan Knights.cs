using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LanKnights : NetworkBehaviour
{
    public NetworkVariable<float> currentHealth = new(10000, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] float finalDamage = 1, moveSpeed = 1f, attackRange = 0.5f,
    attackCooldown, attackSpeed = 1;
    int targetIndex;



   Collider2D target;
   Rigidbody2D rb;
   Vector3 startPosition;
   LanGameManager gmScript;
   Animator anim;
   Transform mobsParent, damagePool;
   LanMobsMelee targetScript;

    void Start() {  
    rb = GetComponent<Rigidbody2D>();
    anim = transform.GetChild(0).GetComponent<Animator>();
    gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    mobsParent = GameObject.FindWithTag("EnemyManager").transform.GetChild(0);
    damagePool = GameObject.FindWithTag("DamagePool").transform;
    startPosition = transform.position;

    //gameobject.name = gameObject.name.Replace()
   }

   private void Update() {
    attackCooldown -= Time.deltaTime;
    if(target != null && !targetScript.isDead) { //if there is target
        float distance = Vector2.Distance(transform.position, target.transform.position); //calculate distance

        if(distance <= attackRange) { //start attacking
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", true);

            if(attackCooldown <=0) {
                AttackServerRpc(targetIndex, finalDamage, NetworkObjectId);
                anim.Play("Attack");
                attackCooldown = 1 / attackSpeed;
            }
        rb.velocity = Vector2.zero;
        }
        else if(distance > attackRange) {// if true, start chasing      0-1
            Vector2 targetDirection = (target.transform.position - transform.position).normalized * moveSpeed; //calculate target direction


            //rotate object to face target
            if(targetDirection.x < 0) {
                transform.GetChild(0).localScale = new Vector2(-0.07f, 0.07f);
            }
            else {
                transform.GetChild(0).localScale = new Vector2(0.07f, 0.07f);
            }
            rb.MovePosition(rb.position + targetDirection * Time.deltaTime); //move to
            anim.SetBool("isRunning", true);
        }
    }
    
   }



   private void OnTriggerEnter2D(Collider2D other) {
    if(!other.CompareTag("Enemy")) return;
    target = other;
    targetScript = target.GetComponent<LanMobsMelee>();
    targetIndex = other.transform.GetSiblingIndex();

   }

    [ServerRpc(RequireOwnership = false)]  //0
    public void AttackServerRpc(int targetIndex, float finalDamage, ulong NetworkObjectId) { //
        int temp = 100;
        LanMobsMelee targetObject = null;;
        if(temp != targetIndex) {   //executed once, then procced to else
            targetObject = mobsParent.transform.GetChild(targetIndex).GetComponent<LanMobsMelee>();
            temp = targetIndex;
            targetObject.AttackedClientRpc(finalDamage, NetworkObjectId);
        }
        else {
            targetObject.AttackedClientRpc(finalDamage, NetworkObjectId);
        }
    }

    public void Attacked(float damage) { 
        if(!IsOwner) return;
        gmScript.UpdateUI();

        
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform.GetChild(1));
        temp.gameObject.SetActive(true);
        
    }
}
