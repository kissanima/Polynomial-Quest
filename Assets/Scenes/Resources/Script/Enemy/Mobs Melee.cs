using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobsMelee : MonoBehaviour
{
    public float attackRange = .5f, walkSpeed = 1f, walkTime = .5f, idleTime = 0f, baseHealth = 100, currentHealth,
    finalHealth, baseDamage = 25, finalDamage, attackSpeed = 1, attackCooldown = 0, elapsedTime, distance, deathTimer = 10f;

    Rigidbody2D rb;
    Vector2 currentTarget;
    public bool isWalking = false, isAttacking, isIdle = true, isDead;
    Collider2D enemyCollider;
    Slider slider;
    public Transform damagePool;
    public Collider2D target;
    public GameManager gmScript;
    Vector3 originalPos;


    void Start()
    {
        //initialize variables
        originalPos = transform.position;

        slider = transform.GetChild(1).GetComponent<Slider>();
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthBar();
    }

    public void UpdateStats()
    {
        finalHealth = baseHealth * (gmScript.enemyStatsModifier / 100);
        currentHealth = finalHealth;
        finalDamage = baseDamage * (gmScript.enemyStatsModifier / 100);
        UpdateHealthBar();
    }


    private void FixedUpdate() {
        attackCooldown -= Time.deltaTime;
        idleTime += Time.deltaTime;

    if (isIdle && !isAttacking) {
        if(idleTime !>= 3) {
            if (!isWalking) {
            Vector2 randomDirection = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));

            //rotate object
            if(randomDirection.x < 0) {
                transform.GetChild(3).localScale = new Vector2(-.1f, transform.GetChild(3).localScale.y);
            }
            else {
                transform.GetChild(3).localScale = new Vector2(.1f, transform.GetChild(3).localScale.y);
            }

            currentTarget = transform.position + (Vector3)randomDirection;

            isWalking = true;
            elapsedTime = 0f; // Reset elapsed time
            idleTime = Random.Range(2f, 5f);
        }

        if (elapsedTime < walkTime) {
            rb.velocity = (currentTarget - (Vector2)transform.position) * walkSpeed;
            //rotate object
            if(rb.velocity.x < 0) {
                transform.GetChild(3).localScale = new Vector2(.1f, transform.GetChild(3).localScale.y);
            }
            else {
                transform.GetChild(3).localScale = new Vector2(-.1f, transform.GetChild(3).localScale.y);
            }
            elapsedTime += Time.deltaTime;

        }
        else {
            rb.velocity = Vector2.zero;
            isWalking = false;
            idleTime = 0f;
        }
        }   
    }
    else if(isAttacking && !target.GetComponent<Player>().isDead) {
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRange) {
            rb.velocity = Vector2.zero;
            Attack();
        }
        else if (distance > attackRange) {
            rb.velocity = target.transform.position - transform.position * walkSpeed;
        }
        else
            {
                Debug.Log("Within attack range");
                rb.velocity = Vector2.zero;
            }
        
        }
    else if(isDead) {
        rb.velocity = Vector2.zero;
    }
    else {
        isAttacking = false;
        isIdle = true;
    }  
}









    private void OnEnable() {
        enemyCollider = GetComponent<BoxCollider2D>();
    }

   

    private void Attack() {
        if(attackCooldown <= 0) {
            target.GetComponent<Player>().Attacked(finalDamage);

            attackCooldown = 1 / attackSpeed;
        }

        
    }

    public void Attacked(float damage, Collider2D player) {
        target = player;
        isIdle = false;
        isAttacking = true;
        Transform temp;

        currentHealth -= damage;
        UpdateHealthBar();
        

        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform);
        temp.gameObject.SetActive(true);

        if(currentHealth <= 0) {
            enemyCollider.enabled = false;
            isAttacking = false;
            isIdle = false;
            isDead = true;

            //give exp
            target.GetComponent<Player>().currentExp += 25;
            target.GetComponent<Player>().updateStats();
            gmScript.UpdateUI();
            gmScript.SavePlayerData();
            StartCoroutine(DisableWait());
        }
    }

    public void UpdateHealthBar() {
        slider.maxValue = finalHealth;
        slider.value = currentHealth;
    }

    IEnumerator DisableWait() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);

        Invoke(nameof(RespawnWait), deathTimer);
    }

    void RespawnWait() {
        transform.position = originalPos;
        gameObject.SetActive(true);

        //reset stats
        currentHealth = finalHealth;
        enemyCollider.enabled = true;
        UpdateHealthBar();
    }
}

