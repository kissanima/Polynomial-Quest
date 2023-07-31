using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public CameraController cameraController;
    public Joystick joystick; 
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject npc, deathPanel;
    public GameManager gmScript;
    public Transform damagePool;
    GameObject[] rootObjects;
    public string playerID;
    public string[] inventory = {"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"};

    public float moveSpeed = 5f, attackSpeed = 1, attackCooldown = 0, baseDamage= 25, finalDamage, currentExp,
    baseRequiredExp = 75, finalRequiredExp, currentHealth, baseHealth = 100, finalHealth, currentMana, baseMana = 75, finalMana,
    potion, weaponDmg, baseArmor = 5, finalArmor, itemArmor, equipedSwordIndex, equipedArmorIndex, deathTimer = 5,
    level = 1;
    public string username;
    public Collider2D[] targetList;

    Rigidbody2D rb;
    Collider2D playerCollider;
    Vector2 targetDirection;
    TextMeshProUGUI deathTimerText;
    public bool isDead;



    private void Awake()     
    {
        //initialize variables
        anim = transform.GetChild(0).GetComponent<Animator>();
        finalDamage = baseDamage + weaponDmg;
        finalArmor = baseArmor + itemArmor;
        finalHealth = baseHealth;
        currentHealth = finalHealth;
        finalMana = baseMana;
        currentMana = finalMana;
        finalRequiredExp = baseRequiredExp;



        deathTimerText = deathPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();      
        StartCoroutine(DetectEnemyWait()); 
    }

    IEnumerator DetectEnemyWait() {   
        while (true) {
            DetectEnemy();   
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void updateStats() {        
        if(currentExp >= finalRequiredExp) {
            currentExp -= finalRequiredExp; //reset current Exp
            level += 1;
            baseRequiredExp += (baseRequiredExp * .20f);
            baseDamage += (baseDamage * .20f);
            baseArmor += (baseArmor * .20f);
            baseHealth += (baseHealth * .20f);

            gmScript.SavePlayerData();
        }
        finalDamage = baseDamage + weaponDmg;
    }

    void FixedUpdate()    
    {
        if(isDead) {
            deathTimer -= Time.deltaTime;
            deathTimerText.SetText(deathTimer.ToString("F2"));
        }
        else {
            attackCooldown -= Time.deltaTime; 
        Vector2 movement = new Vector2(joystick.Horizontal, joystick.Vertical) * moveSpeed;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        //rb.velocity = movement;

        if(joystick.Horizontal > -.25 && joystick.Horizontal < .25 && joystick.Vertical > 0) { //anim up //setBool is yung sa animator
        anim.SetBool("isWalkBack", true);
        anim.SetBool("isWalkFront", false);
        anim.SetBool("isWalkSide", false);
        anim.SetBool("isIdle", false);
        }
        else if(joystick.Horizontal <= -.25) { //anim left
        sprite.flipX = true; 
        anim.SetBool("isWalkSide", true);
        anim.SetBool("isWalkFront", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isIdle", false);
        }
        else if(joystick.Horizontal > -.25 && joystick.Horizontal < .25 && joystick.Vertical < 0) { //anim down
        anim.SetBool("isWalkSide", false);
        anim.SetBool("isWalkFront", true);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isIdle", false);
        }
        else if(joystick.Horizontal >= .25) {
        sprite.flipX = false;
        anim.SetBool("isWalkSide", true);
        anim.SetBool("isWalkFront", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isIdle", false);
        }
        else {
        anim.SetBool("isWalkSide", false);
        anim.SetBool("isWalkFront", false);
        anim.SetBool("isWalkBack", false);
        anim.SetBool("isIdle", true);
        }
        }
    }

    

    void OnDrawGizmos() {    
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .30f);
    }

    void DetectEnemy() { 
        targetList = Physics2D.OverlapCircleAll(transform.position, 0.30f, 1 << 7); 
        if(targetList.Length > 0) {
            targetDirection = (targetList[0].transform.position - transform.position).normalized;
        }
        

    }

    public void Attack() { 
        if(attackCooldown <= 0) { 
            foreach (Collider2D enemy in targetList)  { 
                enemy.GetComponent<MobsMelee>().Attacked(finalDamage, gameObject.GetComponent<Collider2D>());
            }




            //animations // animations ng attack base sa direction ng kalaban
            if(targetDirection.x > -.5 && targetDirection.x < .5 && targetDirection.y > .5) { //attack anim up
            anim.Play("Attack Back");
            attackCooldown = 1 / attackSpeed;
            }
            else if(targetDirection.x <= -.5 && targetDirection.y >= -.5 && targetDirection.y <= .5) { //attack anim left
            sprite.flipX = true;
            anim.Play("Attack Side");
            attackCooldown = 1 / attackSpeed;
            }
            else if(targetDirection.x > -.5 && targetDirection.x < .5 && targetDirection.y < -.5) { //attack anim down
            anim.Play("AttackDown");
            attackCooldown = 1 / attackSpeed;
            }
            else if(targetDirection.x >= .5 && targetDirection.y >= -.5 && targetDirection.y <= .5) { //attack anim right
            sprite.flipX = false;
            anim.Play("Attack Side");
            }
    }

    }

    public void Attacked(float damage) { 
        currentHealth -= damage;
        gmScript.UpdateUI();

        
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform.GetChild(1));
        temp.gameObject.SetActive(true);
        
        if(currentHealth <= 0) {
            isDead = true;
            playerCollider.enabled = false;
            deathPanel.SetActive(true); //show Died message
            StartCoroutine(playerRespawnWait());
        }
    }

    IEnumerator playerRespawnWait() {
        yield return new WaitForSeconds(5);
        deathTimer = 5;
        transform.position = new Vector3(0f,0f,0f);
        isDead = false;
        //set Stats
        currentHealth = finalHealth;
        currentMana = finalMana;
        currentExp -= (currentExp * .20f);
        deathPanel.SetActive(false);
        gmScript.UpdateUI();
    }
}
