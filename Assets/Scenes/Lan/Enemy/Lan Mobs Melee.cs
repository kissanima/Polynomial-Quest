using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class LanMobsMelee : NetworkBehaviour
{
    
    public NetworkVariable<float> baseHealth = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> finalHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public float attackRange = .5f, walkSpeed = 1f, walkTime = .5f, idleTime = 0f, baseDamage = 25, finalDamage,
     attackSpeed = 1, attackCooldown = 0, elapsedTime, distance, deathTimer = 10f;

    Rigidbody2D rb;
    Vector2 currentTarget;
    public bool isWalking = false, isAttacking, isIdle = true, isDead;
    Collider2D enemyCollider;
    Slider slider;
    public Transform damagePool;
    public Collider2D target;
    public LanGameManager gmScript;
    LanInteractionManager interactionManager;
    GameObject npc;
    Vector3 originalPos;
    Animator anim;
    LanPlayer[] players;
    SpriteRenderer[] spriteRenderers;

    public override void OnNetworkSpawn()
    {
        //initialize variables
        damagePool = GameObject.FindWithTag("DamagePool").transform;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        originalPos = transform.position;
        anim = transform.GetChild(3).GetComponent<Animator>();
        transform.GetChild(3).GetComponent<ClientNetworkAnimator>().Animator = anim;
        slider = transform.GetChild(1).GetComponent<Slider>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        UpdateStats();        

        currentHealth.OnValueChanged += (float oldValue, float newValue) => {
            UpdateHealthBar(oldValue, newValue);
        };

        //get all sprite renderer
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        //interaction manager
        interactionManager = GameObject.FindWithTag("UI").transform.GetChild(3).GetComponent<LanInteractionManager>();

    }

    
    public void UpdateStats()
    {
        if(!IsOwner) return;
        finalHealth.Value = baseHealth.Value * (gmScript.enemyStatsModifier / 100f);
        currentHealth.Value = finalHealth.Value;
        finalDamage = baseDamage * (gmScript.enemyStatsModifier / 100f);

        //set slider values
        slider.maxValue = finalHealth.Value;
        slider.value = currentHealth.Value;
    }


    private void FixedUpdate() {
        if(!IsHost || !IsClient) return;
        attackCooldown -= Time.deltaTime;
        idleTime += Time.deltaTime;

    if (isIdle && !isAttacking) {        //if idle and not attacking
        if(idleTime !>= 3) {       
            if (!isWalking) {
            Vector2 randomDirection = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));

            /*
            if(randomDirection.x < 0) {
                transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
            }
            else {
                transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
            } */

            currentTarget = transform.position + (Vector3)randomDirection;

            isWalking = true;
            elapsedTime = 0f; // Reset elapsed time
            idleTime = Random.Range(2f, 5f);
        }

        if (elapsedTime < walkTime) {
            rb.velocity = (currentTarget - (Vector2)transform.position) * walkSpeed;

            
            //flip object
            if(rb.velocity.x < 0) {
                transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
            }
            else {
                transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
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
    else if(isAttacking && target != null) {           //if attacking and the player that attack this object is not dead
        distance = Vector2.Distance(transform.position, new Vector2(target.transform.position.x, target.transform.position.y)); //TODO: need to optimize 

        if (target != null) {
                Vector3 direction = target.transform.position - transform.position;

                if(rb.velocity.x < 0) {
                transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                }
                else {
                transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
            }


        if (distance <= attackRange) {         //if distance of the player is less or equal attackRange
            rb.velocity = Vector2.zero;
            AttackServerRpc();
        }
        else if (distance > attackRange) {
            rb.velocity = target.transform.position - transform.position * walkSpeed;
        }
        else
            {
                if (target != null) {
                Vector3 direction = target.transform.position - transform.position;

                if(direction.x < 0) {
                    transform.GetChild(3).localScale = new Vector2(.1f, transform.GetChild(3).localScale.y);
                }
                else {
                    transform.GetChild(3).localScale = new Vector2(-.1f, transform.GetChild(3).localScale.y);
                }
                }


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


   

    [ServerRpc(RequireOwnership = false)]
    private void AttackServerRpc() {
        if(attackCooldown <= 0) {
            if(target.CompareTag("Knight")) {
                target.GetComponent<LanKnights>().Attacked(finalDamage);
            }
            else {
                target.GetComponent<LanPlayer>().Attacked(finalDamage);
            }

            attackCooldown = 1 / attackSpeed;
        }  
    }

    
    public void Attacked(float damage, ulong networkId) {
        SubtracthealthServerRpc(damage); // call server to subtract network variable health using ServerRpc
        //players = FindObjectsOfType<LanPlayer>();
        Debug.Log(networkId);

        foreach (NetworkObject p in gmScript.player.nObjects) 
        {
            if (p.NetworkObjectId == networkId)
            {
                target = p.GetComponent<Collider2D>();
                break;
            }
        }
        isIdle = false;
        isAttacking = true;

        //spawn damage pops
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform);
        temp.gameObject.SetActive(true);

        //spawn blood effects
        gmScript.player.SpawnBloodEffectServerRpc(NetworkObjectId);

        if(currentHealth.Value <= (finalHealth.Value * .15f)) {
            enemyCollider.enabled = false;
            isAttacking = false;
            isIdle = false;
            isDead = true;


            gmScript.player.npc = gameObject;
            interactionManager.gameObject.SetActive(true);


            //give exp
            /*
            gmScript.player.currentExp += 25;
            gmScript.player.updateStats();
            gmScript.UpdateUI(); //update player healtbar, exp bar etc
            gmScript.SavePlayerData(); //save data
            StartCoroutine(DisableWait()); */
        }

    }

    IEnumerator DisableWait() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);

        Invoke(nameof(RespawnWait), deathTimer);
    }

    void RespawnWait() {
        transform.position = originalPos;
        isDead = false;
        gameObject.SetActive(true);

        //reset stats
        currentHealth.Value = finalHealth.Value;
        enemyCollider.enabled = true;
        target = null;
    }

    
    public void UpdateHealthBar(float oldValue, float newValue) {
        slider.maxValue = finalHealth.Value;
        slider.value = newValue;
    }


    //call server to subtract
    [ServerRpc(RequireOwnership = false)] public void SubtracthealthServerRpc(float damage) {
        currentHealth.Value -= damage;  //currentHealth.Value = currentHealth.Value - damage

        //play animation
        anim.Play("Hit");   
    }


    //blood effect
    /*
    [ServerRpc(RequireOwnership = false)] public void SpawnBloodEffectServerRpc() {
        gmScript.player.SpawnBloodEffectServerRpc(NetworkObjectId);
    } */

    [ServerRpc(RequireOwnership = false)] public void HealServerRpc(float healAmount) {
        Debug.Log("heal called " + healAmount);
        currentHealth.Value += healAmount;


    }
}

