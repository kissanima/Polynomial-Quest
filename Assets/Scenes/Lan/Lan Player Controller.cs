using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine.UI;

public class LanPlayer : NetworkBehaviour
{
    public FixedJoystick joystick; 
    public Animator anim;
    public GameObject npc, deathPanel, mobsParent;
    public LanGameManager gmScript;
    public Transform damagePool;
    public Transform bloodEffectsParent, skillEffectsParent, maps;
    public NetworkObject[] nObjects;
    

    //player customizations network variables
    public NetworkVariable<int> belt = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> boots = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> elbow = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> face = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> hood = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> legs = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> shoulder = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> torso = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> wrist = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> baseHealth = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> finalHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public string[] inventory = {"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"};

    public float moveSpeed = 1f, attackSpeed = 1, attackCooldown = 0, baseDamage= 25, finalDamage, currentExp,
    baseRequiredExp = 75, finalRequiredExp, currentMana, baseMana = 75, finalMana, level,
    potion, weaponDmg, baseArmor = 5, finalArmor, itemArmor, equipedWeaponIndex, equipedArmorIndex, deathTimer = 5,
    damageReduction, attackRange;
    public string username, playerClass;
    public Collider2D[] targetList;
    public Slider sliderHealthWS;
    Rigidbody2D rb;
    Collider2D playerCollider;
    Vector2 targetDirection;
    TextMeshProUGUI deathTimerText;
    LanAttackButton attackButton;
    public bool isDead, isUsingSkill;
    



    void Start()     
    {
        //rename player object for easy debugging
        if(IsOwnedByServer) {
            gameObject.name = "Player: Host";
        }
        else {
            gameObject.name = "Player: Client";
        }
        mobsParent = GameObject.FindWithTag("EnemyManager").transform.GetChild(0).gameObject;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        bloodEffectsParent = GameObject.FindWithTag("BloodEffects").transform;
        skillEffectsParent = GameObject.FindWithTag("SkillEffects").transform;
        maps = GameObject.FindWithTag("Maps").transform;
        FindNetworkObjectsServerRpc();
        
        //check if is owner if not, return;
        if(!IsOwner) return; 
        
        
        
        //assign network variables values from PlayerPrefs
        belt.Value = PlayerPrefs.GetInt("belt");
        boots.Value = PlayerPrefs.GetInt("boots");
        elbow.Value = PlayerPrefs.GetInt("elbow");
        face.Value = PlayerPrefs.GetInt("face");
        hood.Value = PlayerPrefs.GetInt("hood");
        legs.Value = PlayerPrefs.GetInt("legs");
        shoulder.Value = PlayerPrefs.GetInt("shoulder");
        torso.Value = PlayerPrefs.GetInt("torso");
        wrist.Value = PlayerPrefs.GetInt("wrist");
        
        //initialize variables
        finalDamage = baseDamage + weaponDmg;
        finalArmor = baseArmor + itemArmor;
        finalHealth.Value = baseHealth.Value;
        currentHealth.Value = finalHealth.Value;
        finalMana = baseMana;
        currentMana = finalMana;
        finalRequiredExp = baseRequiredExp;
        potion = 10;

        damagePool = GameObject.FindWithTag("DamagePool").transform; // for damage pops
        anim = transform.GetChild(0).GetComponent<Animator>(); //for animations
        transform.GetChild(0).GetComponent<ClientNetworkAnimator>().Animator = anim; //to sync animations across clients
        //transform.parent.GetComponent<LanCameraController>().player = gameObject;
        joystick = GameObject.FindWithTag("UI").transform.GetChild(2).GetChild(0).GetComponent<FixedJoystick>(); //joystick for movement
        //GameObject.FindWithTag("UI").transform.GetChild(2).GetChild(1).GetComponent<LanAttackButton>().player = GetComponent<LanPlayer>();
        rb = GetComponent<Rigidbody2D>();
        deathPanel = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(3).gameObject;
        sliderHealthWS = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        

        deathTimerText = deathPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerCollider = GetComponent<BoxCollider2D>();      
        StartCoroutine(DetectEnemyWait()); 

        

        //initialize GameManager
        gmScript.Initialize();

        updateStats();

        //suscribe to onValueChange listener for healthbar
        currentHealth.OnValueChanged += (float oldValue, float newValue) => {
            UpdateHealthBar(oldValue, newValue);
        };


        //call server to call PlayerCustomizationClientRpc method on all client
        PlayerCustomizationServerRpc(); //load costomization
        CallUpdatePlayerNameInfoServerRpc(); //update names


        //HOST = server and player ///network objects OWNER
        //CLIENT = players
        if(!IsOwnedByServer) { //executed except on HOST
            GetDifficultyServerRpc();  //get difficulty
        }



        if(!IsOwnedByServer) return; //codes below is executed on server only ///////////////////
        RandomWeatherServerRpc(); //weather
    }

    IEnumerator DetectEnemyWait() {   
        while (true) {
            DetectEnemy();   
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void updateStats() {        
        if(currentExp >= finalRequiredExp && level <= 30) {
            currentExp -= finalRequiredExp; //reset current Exp
            level += 1;
            baseRequiredExp += (baseRequiredExp * .20f);
            baseDamage += (baseDamage * .20f);
            baseArmor += (baseArmor * .20f);
            baseHealth.Value += (baseHealth.Value * .20f);

            gmScript.SavePlayerData(); //save data
            gmScript.UpdateUI();
        }
        finalDamage = baseDamage + weaponDmg;
        finalRequiredExp = baseRequiredExp;
        sliderHealthWS.maxValue = finalHealth.Value;
        sliderHealthWS.value = currentHealth.Value;

        gmScript.SavePlayerData(); //save data
        gmScript.UpdateUI();
    }

    void FixedUpdate()    
    {

        //check if owner
        if(!IsOwner) return;
        if(isDead) {
            deathTimer -= Time.deltaTime;
            deathTimerText.SetText(deathTimer.ToString("F2"));
        }
        else {
            attackCooldown -= Time.deltaTime; 

            if(!isUsingSkill) {
                Vector2 movement = new Vector2(joystick.Horizontal, joystick.Vertical) * moveSpeed;
                rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
            } 



        //flip object
        if(joystick.Horizontal < 0) {
            transform.GetChild(0).localScale = new Vector3(-0.025f, 0.025f, 0);
        }
        else if(joystick.Horizontal > 0) {
            transform.GetChild(0).localScale = new Vector3(0.025f, 0.025f, 0);
        }
        //rb.velocity = movement;

        //animation here
        if(joystick.Horizontal < 0 || joystick.Horizontal > 0 || joystick.Vertical < 0 || joystick.Vertical > 0  ) {
            anim.SetBool("isIdle", false);
            anim.SetBool("isRun", true);
        }
        else {
            anim.SetBool("isRun", false);
            anim.SetBool("isIdle", true);
        }
        }
    }

    

    void OnDrawGizmos() {    
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void DetectEnemy() { 
        targetList = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << 7); 
        if(targetList.Length > 0) {
            targetDirection = (targetList[0].transform.position - transform.GetChild(0).position).normalized;
        }
        

    }

    public void EventAttack() {
        AttackServerRpc(targetList[0].transform.GetSiblingIndex(), finalDamage, NetworkObjectId);
    }
    

    public void Attack() {
            switch (playerClass)
            {
                
                case "Warrior":
                    //animations // animations ng attack basi sa direction ng kalaban
                    if(targetDirection.x < 0) { //attack anim up
                    transform.GetChild(0).localScale = new Vector3(-0.025f, 0.025f, 0);     
                    anim.Play("Attack1");
                    }
                    else if(targetDirection.x > 0) { //attack anim left
                    transform.GetChild(0).localScale = new Vector3(0.025f, 0.025f, 0);
                    //play animation
                    anim.Play("Attack1");
                     }
                break;



                case "Mage":
                    Vector2 targetPosition = targetList[0].transform.position - transform.position; //get enemy position
                    SpawnMagicBulletServerRpc(targetPosition);

                break;

                case "Assassin":
                break;
            }
    }


    public void Attacked(float damage) { 
        if(!IsOwner) return;
        anim.Play("Hit");
        currentHealth.Value -= damage;
        gmScript.UpdateUI();

        
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform.GetChild(1));
        temp.gameObject.SetActive(true);
        
        //blood effects
        SpawnBloodEffectServerRpc(NetworkObjectId);

        if(currentHealth.Value <= 0) {
            isDead = true;
            anim.SetBool("isDead", true);
            playerCollider.enabled = false;
            deathPanel.SetActive(true); //show Died message
            StartCoroutine(playerRespawnWait());
        }
    }

    IEnumerator playerRespawnWait() {
        yield return new WaitForSeconds(5);
        deathTimer = 5;
        transform.position = Vector3.zero;
        isDead = false;
        anim.SetBool("isDead", false);
        playerCollider.enabled = true;
        
        //set Stats
        currentHealth.Value = finalHealth.Value;
        currentMana = finalMana;
        currentExp -= (currentExp * .20f);
        deathPanel.SetActive(false);
        gmScript.UpdateUI();
    }

    

    public void UpdateHealthBar(float oldValue, float newValue) {
        sliderHealthWS.value = finalHealth.Value;
        sliderHealthWS.value = newValue;
    }



    //////////////////////////////////////////////////////////////////////NETWORKING///////////////////////////////////////////////////////////TODO: 

    [ServerRpc(RequireOwnership = false)]
    public void SubtractHealthServerRpc(float damage) {
        currentHealth.Value -= damage;
    }


    [ServerRpc(RequireOwnership = false)]
    public void AttackServerRpc(int targetIndex, float finalDamage, ulong networkId) { //
        int temp = 100;
        LanMobsMelee targetObject = null;;
        if(temp != targetIndex) {   //executed once, then procced to else
            targetObject = mobsParent.transform.GetChild(targetIndex).GetComponent<LanMobsMelee>();
            temp = targetIndex;
            targetObject.Attacked(finalDamage, networkId);
        }
        else {
            targetObject.Attacked(finalDamage, networkId);
        }
    }



    //initialize and store network objects
    [ServerRpc(RequireOwnership = false)]  
    public void FindNetworkObjectsServerRpc() {
        FindNetworkObjectsClientRpc();
    }
    [ClientRpc]
    public void FindNetworkObjectsClientRpc() { //also find all players objects
        nObjects = FindObjectsOfType<NetworkObject>();
    }


    //get difficulty from server
    [ServerRpc(RequireOwnership = false)]   //TODO: 
    public void GetDifficultyServerRpc() {
        SetDifficultyClientRpc(gmScript.difficulty);
    }
    [ClientRpc]
    public void SetDifficultyClientRpc(int difficulty) { ///executed on players CLIENT 
    Debug.Log(difficulty);
        this.gmScript.difficulty = difficulty;

        switch (gmScript.difficulty) //difficulty =1
        {
            case 0: //easy
            maps.GetChild(0).gameObject.SetActive(true); //forest
            maps.GetChild(1).gameObject.SetActive(false); //desert
            maps.GetChild(2).gameObject.SetActive(false); //snow
            break;

            case 1:
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(true);
            maps.GetChild(2).gameObject.SetActive(false);
            break;

            case 2:
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(false);
            maps.GetChild(2).gameObject.SetActive(true);
            break;

            case 3: //extreme
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(false);
            maps.GetChild(2).gameObject.SetActive(false);
            break;
        }
    }





    //load customization
    [ServerRpc(RequireOwnership = false)]  
    public void PlayerCustomizationServerRpc() {
        PlayerCustomizationClientRpc();
    }
    [ClientRpc]  
    public void PlayerCustomizationClientRpc() {
        gmScript.LoadPlayerCostumization();
    }



    //weather
    [ServerRpc(RequireOwnership = false)]
    public void RandomWeatherServerRpc() {
        if(!IsOwnedByServer) return;
        int draw = Random.Range(0, 2);
        Debug.Log("RandomWeatherServerRpc called: " + draw);

        if(draw == 1 && gmScript.difficulty == 1) {
            StartWeatherClientRpc();
        }
    }
    [ClientRpc]
    public void StartWeatherClientRpc() {
        gmScript.RainWeather();

    }



    //spawn blood effects
    [ServerRpc(RequireOwnership = false)] 
    public void SpawnBloodEffectServerRpc(ulong targetID) {
        SpawnBloodEffectsClientRpc(targetID);
    }
    [ClientRpc]
    public void SpawnBloodEffectsClientRpc(ulong targetID) { //1
        Transform temp = bloodEffectsParent.GetChild(0);
        NetworkObject objectFound = null; //optimization

        if(objectFound != null && objectFound.NetworkObjectId == targetID) {
            temp.parent = objectFound.transform;
        }
        else {
            foreach (var item in nObjects) 
            {
            if(targetID == item.NetworkObjectId) {
                temp.parent = item.transform;
                objectFound = item;
                break;
            }
            }
        }

        temp.gameObject.SetActive(true);
    }



    [ServerRpc(RequireOwnership = false)]
    public void SpawnMagicBulletServerRpc(Vector2 targetPosition) {
        SpawnMagicBulletClientRpc(targetPosition);
    }






    [ServerRpc(RequireOwnership = false)]
    public void CallUpdatePlayerNameInfoServerRpc() {
        CallUpdatePlayerNameInfoClientRpc();
    }

    [ClientRpc]
    public void CallUpdatePlayerNameInfoClientRpc() {
        UpdatePlayerNameLevel();
    }

    void UpdatePlayerNameLevel() {
        CallUpdatePlayerNameLevelWSServerRpc(this.username, this.level, NetworkObjectId);
    }
    

    [ServerRpc(RequireOwnership = false)]
    public void CallUpdatePlayerNameLevelWSServerRpc(string name, float level, ulong NetworkObjectId) {
        UpdatePlayerNameLevelWSClientRpc(name, level, NetworkObjectId);
    }

    [ClientRpc]
    public void UpdatePlayerNameLevelWSClientRpc(string name, float level, ulong id) { //update player name and level in world space
        //find all player scripts
        LanPlayer[] temp = FindObjectsOfType<LanPlayer>();
        gmScript.players = temp;

        foreach (var item in temp)
        {
            if(item.NetworkObjectId == id) {
                item.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(name + " lvl. " + level);
                break;
            }
        }
    }



    [ClientRpc]
    public void SpawnMagicBulletClientRpc(Vector2 enemyPosition) {
        Transform temp = skillEffectsParent.GetChild(1).GetChild(0).GetChild(0);
        MagicBullet tempScript = temp.GetComponent<MagicBullet>();

        tempScript.targetPosition = enemyPosition;
        temp.position = transform.position;
        temp.SetParent(skillEffectsParent.GetChild(1).GetChild(1)); //move to in Use object
        temp.gameObject.SetActive(true);
    }

    








}