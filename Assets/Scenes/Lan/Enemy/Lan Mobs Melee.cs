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
    public bool isWalking = false, isAttacking, isIdle = true, isDead, hasAttacked;
    Collider2D enemyCollider;
    Slider slider;
    public Transform damagePool, textBox;
    TextMeshProUGUI textBoxText;
    public Collider2D target;
    public LanGameManager gmScript;
    [SerializeField] LanInteractionManager interactionManager;
    GameObject npc;
    Vector3 originalPos;
    Animator anim;
    LanPlayer[] players;
    SpriteRenderer[] spriteRenderers;
    LanPlayer targetScript;
    [SerializeField] bool isBoss, isEmmanuel, isRespawnable;
    [SerializeField] string[] dialogues;
    [SerializeField] Transform wilson, ending;
    [SerializeField] LanMobsMelee emmanuel;
    bool dialogue1Done, dialogue2Done;
    //sounds
    AudioSource hitAudioSource, dieAudioSource;
    Transform bloodEffectParent;
    public Collider2D attackersTarget;
    public override void OnNetworkSpawn()
    {
        //initialize variables
        damagePool = GameObject.FindWithTag("DamagePool").transform;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        originalPos = transform.position;
        //animators component
        anim = transform.GetChild(3).GetComponent<Animator>();
        transform.GetChild(3).GetComponent<ClientNetworkAnimator>().Animator = anim;

        hitAudioSource = transform.GetChild(5).GetChild(0).GetComponent<AudioSource>();
        
        slider = transform.GetChild(1).GetComponent<Slider>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        bloodEffectParent = GameObject.FindWithTag("BloodEffects").transform;
        
        if(isBoss) {
            textBox = transform.GetChild(6);
            textBoxText = textBox.GetChild(0).GetComponent<TextMeshProUGUI>();
            dieAudioSource = transform.GetChild(5).GetChild(1).GetComponent<AudioSource>();
        }
        //else { //randomize enemy design
        //    int drawDesign = Random.Range(0, gmScript.MobsDesign.Length);
        //    Instantiate(gmScript.MobsDesign[drawDesign].transform.GetChild(0), transform.GetChild(3));
        //    Destroy(transform.GetChild(3).GetChild(0).gameObject);
        //}
    

        UpdateStats();        

        currentHealth.OnValueChanged += (float oldValue, float newValue) => {
            UpdateHealthBar(oldValue, newValue);
        };

        //get all sprite renderer
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();


        //////////////////////////////////////////////////////////OPTIMIZATIONS//////////////////////////////////////////////////////////////
        transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = false;
        transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false); //disable slider
        transform.GetChild(4).gameObject.SetActive(false); //disable
        if(isRespawnable) {
            GetComponent<LanMobsMelee>().enabled = false;
            anim.enabled = false;
            transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        }
        //END OPTIMIZATIONS


        if(isBoss && !isEmmanuel) {
             target = emmanuel.target;
        }
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

    if (isIdle && !isAttacking && IsOwnedByServer) {        //if idle and not attacking
        if(idleTime !>= 3) {       
            if (!isWalking) {
            Vector2 randomDirection = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
            currentTarget = transform.position + (Vector3)randomDirection;

            isWalking = true;
            elapsedTime = 0f; // Reset elapsed time
            idleTime = Random.Range(2f, 5f);
            }
            if (elapsedTime < walkTime) { //start moving
                anim.SetBool("isMoving", true);
                rb.velocity = (currentTarget - (Vector2)transform.position) * walkSpeed;
                //flip object
                if(isBoss) {
                    if(rb.velocity.x < 0) {
                        transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                    }
                    else {
                        transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                    }
                }
            else {
                if(rb.velocity.x < 0) {
                transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                }
                else {
                transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
            }
            elapsedTime += Time.deltaTime;
        }
        else { //stop moving
            anim.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            isWalking = false;
            idleTime = 0f;
        }
        }   
    }
    else if(isAttacking && target != null) {           //if attacking and the player that attack this object is not dead
        distance = Vector2.Distance(transform.position, new Vector2(target.transform.position.x, target.transform.position.y));

        if (target != null) {
                Vector3 direction = target.transform.position - transform.position;

                //flip object
            if(isBoss) {
                if(direction.x < 0) {
                    transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
                else {
                    transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                }
            }
            else {
                if(direction.x < 0) {
                transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                }
                else {
                transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
            }
            }


        if (distance <= attackRange) {         //if distance of the player is less or equal attackRange, attack
            rb.velocity = Vector2.zero;
            if(targetScript != null && targetScript.currentHealth.Value > 0 && IsOwnedByServer) { //if health is greater than 0, attack
                if(attackCooldown <= 0) {
                    anim.Play("attack");
                    AttackServerRpc(finalDamage, targetScript.NetworkObjectId);
                    attackCooldown = 1 / attackSpeed;
                }
              
            }
            else {
                target = null;
            }
        }
        else if (distance > attackRange && distance <= 5) { //start chasing enemy
            rb.velocity = target.transform.position - transform.position * walkSpeed;
            anim.SetBool("isMoving", true);
        }
        else
            {   /*
                if (target != null) {
                Vector3 direction = target.transform.position - transform.position;

                    //flip object
                if(isBoss) {
                    if(rb.velocity.x < 0) {
                    transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
                else {
                    transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                }
                }
                else {
                    if(rb.velocity.x < 0) {
                    transform.GetChild(3).localScale = new Vector3(.1f, .1f, 1);
                    }
                    else {
                    transform.GetChild(3).localScale = new Vector3(-.1f, .1f, 1);
                    }
                }
                } */
                anim.SetBool("isMoving", false);
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
    private void AttackServerRpc(float finalDamage, ulong playerID) {
            if(target.CompareTag("Knight")) {
                target.GetComponent<LanKnights>().Attacked(finalDamage);
            }
            else {
                AttackServerClientRpc(finalDamage, playerID);
            }
    }

    [ClientRpc] void AttackServerClientRpc(float finalDamage, ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                item.Attacked(finalDamage, playerID);
                break;
            }
        }
    }

    [ClientRpc]public void AttackedClientRpc(float damage, ulong networkId, int isKnight) {
        if(IsOwner) {
            SubtracthealthServerRpc(damage, networkId); // call server to subtract network variable health using ServerRpc
        }

        ulong tempId = 987;
        if(tempId != networkId) { //optimization - if networkID has change, call getComponent;
            tempId = networkId;
            if(isKnight == 1) {
                foreach (var item in gmScript.knights)
                {
                    if(item.NetworkObjectId == networkId) {
                    target = item.GetComponent<Collider2D>();
                    break;
                    }
                }
            }
            else { //is a player
                foreach (var p in gmScript.players) {
                    if (p.NetworkObjectId == networkId) {
                        target = p.GetComponent<Collider2D>();
                        targetScript = p.GetComponent<LanPlayer>();
                        break;
                    }
                }
            }
        }

        
        isIdle = false;
        isAttacking = true;


        //spawn damage pops
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(((int)damage).ToString());
        temp.SetParent(transform);
        temp.gameObject.SetActive(true);


        if(currentHealth.Value <= (finalHealth.Value * .15f) && !isBoss) { //MOBS
            enemyCollider.enabled = false;
            isAttacking = false;
            isIdle = false;
            isDead = true;
            
            if(networkId == gmScript.player.NetworkObjectId) {
                gmScript.player.npc = gameObject;
                interactionManager.gameObject.SetActive(true);
            }

        }
        else if(currentHealth.Value <= 0 && isBoss) { //BOSS
            enemyCollider.enabled = false;
            isDead = true;
            isAttacking = false;
            anim.SetBool("isMoving", false);
            anim.Play("Die");
            dieAudioSource.Play();
        }
        else {
            //play hit sound
            hitAudioSource.Play();
        }

    }
    private void OnDisable() {
        if(isRespawnable) { //if false, dont respawn
            Invoke(nameof(RespawnWait), deathTimer);
        }

    }

    void RespawnWait() {
        isDead = false;
        gameObject.SetActive(true);
        enemyCollider.enabled = true;
        target = null;


        if(!IsOwner) return;
        //reset stats
        currentHealth.Value = finalHealth.Value; //server only
        transform.position = originalPos; //server only
    }

    
    public void UpdateHealthBar(float oldValue, float newValue) { //TODO:
    slider.maxValue = finalHealth.Value;
    slider.value = newValue;

    if(newValue < oldValue) {
        Transform bloodEffectTemp = bloodEffectParent.GetChild(0);
        bloodEffectTemp.SetParent(transform);
        bloodEffectTemp.gameObject.SetActive(true);

    }



    if(isBoss) {
        if(hasAttacked == false && isEmmanuel) {
            StartCoroutine(PlayEmmanuelEvilDialogue());
            hasAttacked = true;
        }
        else if(newValue < (finalHealth.Value * .95) && newValue > (finalHealth.Value * .90)  && isEmmanuel && !dialogue1Done) { //boss 90-95% 
            StartCoroutine(PlayEmmanuelEvilDialogue1());
            dialogue1Done = true;
        }
        else if(newValue < (finalHealth.Value * .50) && newValue > (finalHealth.Value * .45)  && isEmmanuel && !dialogue2Done) { //teleport wilson here
            wilson.transform.position = new(transform.position.x + .5f, transform.position.y);
            StartCoroutine(PlayEmmanuelEvilDialogue2());
            dialogue2Done = true;
        }
        else if(newValue <= 0 && isEmmanuel) {
            StartCoroutine(PlayEmmanuelEvilDialogue3());
        }
    }    
    }


    //call server to subtract
    [ServerRpc(RequireOwnership = false)] public void SubtracthealthServerRpc(float damage, ulong playerID) {
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
        currentHealth.Value += healAmount;
    }

    //////////////////DIALOGUE//////////////////////////////////////////////////////////////
    IEnumerator PlayEmmanuelEvilDialogue() {
        textBox.gameObject.SetActive(true);
        string dialogue0 = "Thus, you've come to deal with me, " + gmScript.player.username + "?" + " What a shame that your efforts were useless.";
        foreach (var item in dialogue0) //
        {
            textBoxText.text += item;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1.5f);
        textBox.gameObject.SetActive(false);
        textBoxText.text = null;
    }

    IEnumerator PlayEmmanuelEvilDialogue1() {
        textBox.gameObject.SetActive(true);
        foreach (var item in dialogues[1])
        {
            textBoxText.text += item;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1.5f);
        textBox.gameObject.SetActive(false);
        textBoxText.text = null;
    }

    IEnumerator PlayEmmanuelEvilDialogue2() { //wilson the evil teleported
        textBox.gameObject.SetActive(true);
        foreach (var item in dialogues[2])
        {
            textBoxText.text += item;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1.5f);
        textBox.gameObject.SetActive(false);
        textBoxText.text = null;
    }

    IEnumerator PlayEmmanuelEvilDialogue3() { //You have demonstrated
        textBox.gameObject.SetActive(true);
        foreach (var item in dialogues[3])
        {
            textBoxText.text += item;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(2f);
        textBox.gameObject.SetActive(false);
        textBoxText.text = null;
        StartCoroutine(PlayEmmanuelEvilDialogue4());
    }

    IEnumerator PlayEmmanuelEvilDialogue4() { //you have won
        string dialogeTemp = "You, " + gmScript.player.username + ", have won.";
        yield return new WaitForSeconds(1f);

        textBox.gameObject.SetActive(true);
        foreach (var item in dialogeTemp)
        {
            textBoxText.text += item;
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1.5f);
        textBox.gameObject.SetActive(false);
        textBoxText.text = null;

        //START ENDING
        ending.gameObject.SetActive(true);
    }
}

