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
    public Transform bloodEffectsParent, skillEffectsParent, maps, ui;
    Transform itemsPool, inventoryPanel;

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
    public NetworkVariable<float> score = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> level = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<string> nameNVariable = new("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public string[] inventory = {"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"};

    public float moveSpeed = 1f, attackSpeed = 1, attackCooldown = 0, baseDamage= 25, finalDamage, currentExp,
    baseRequiredExp = 75, finalRequiredExp, currentMana, baseMana = 75, finalMana,
    potion, weaponDmg, baseArmor = 5, finalArmor, itemArmor, equipedWeaponIndex, equipedArmorIndex, deathTimer = 5,
    damageReduction, attackRange, hint = 10, finishIntro, hasStatsInitialized;
    public string username, playerClass;
    public Collider2D[] targetList;
    public Slider sliderHealthWS;
    Rigidbody2D rb;
    Collider2D playerCollider;
    Vector2 targetDirection;
    TextMeshProUGUI deathTimerText;
    LanAttackButton attackButton;
    Image cooldownImage;
    public bool isDead, isUsingSkill, isManaShieldOn;
    AudioSource audioSource, hitAudioSource, dieAudioSource;
    [SerializeField] AudioClip[] WarriorSoundEffects;
    [SerializeField] AudioClip[] MageSoundEffects;


    



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
        anim = transform.GetChild(0).GetComponent<Animator>(); //for animations
        ui = GameObject.FindWithTag("UI").transform;
        itemsPool = ui.GetChild(4).GetChild(1);

        if(!IsLocalPlayer) {
            transform.GetChild(6).gameObject.SetActive(false);
        }

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
        

        damagePool = GameObject.FindWithTag("DamagePool").transform; // for damage pops
        transform.GetChild(0).GetComponent<ClientNetworkAnimator>().Animator = anim; //to sync animations across clients
        //transform.parent.GetComponent<LanCameraController>().player = gameObject;
        joystick = GameObject.FindWithTag("UI").transform.GetChild(2).GetChild(0).GetComponent<FixedJoystick>(); //joystick for movement
        //GameObject.FindWithTag("UI").transform.GetChild(2).GetChild(1).GetComponent<LanAttackButton>().player = GetComponent<LanPlayer>();
        rb = GetComponent<Rigidbody2D>();
        deathPanel = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(3).gameObject;
        sliderHealthWS = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        cooldownImage = GameObject.FindWithTag("Controls").transform.GetChild(1).GetChild(0).GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        inventoryPanel = GameObject.FindWithTag("UI").transform.GetChild(4).GetChild(0);
        hitAudioSource = transform.GetChild(7).GetComponent<AudioSource>();
        dieAudioSource = transform.GetChild(8).GetComponent<AudioSource>();
        hitAudioSource.clip = gmScript.playerHitSoundEffect;
        dieAudioSource.clip = gmScript.playerDieSoundEffect;

        deathTimerText = deathPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerCollider = GetComponent<BoxCollider2D>();      
        StartCoroutine(DetectEnemyWait()); 

        

        //initialize GameManager
        gmScript.Initialize();
        
        //updateStats();

        //suscribe to onValueChange listener for healthbar
        currentHealth.OnValueChanged += (float oldValue, float newValue) => {
            UpdateHealthBar(oldValue, newValue);
        };


        //call server to call PlayerCustomizationClientRpc method on all client
        PlayerCustomizationServerRpc(); //load costomization


        //HOST = server and player ///network objects OWNER
        //CLIENT = players
        if(!IsOwnedByServer) { //executed except on HOST
            GetDifficultyServerRpc();  //get difficulty
        }

        StartCoroutine(ManaRegen()); //start mana regen, will regen 5% of max mana every .5s

        //basic attack sound
        switch (playerClass)
        {
            case "Warrior":
            audioSource.clip = gmScript.WarriorSoundEffects[0];
            break;
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
        if(currentExp != 0 && currentExp >= finalRequiredExp && level.Value <= 30) {
            currentExp -= finalRequiredExp; //reset current Exp
            level.Value += 1;
            baseRequiredExp += (baseRequiredExp * .20f);
            finalRequiredExp = baseRequiredExp;

            baseDamage += (baseDamage * .20f);
            finalDamage = baseDamage + weaponDmg;

            baseArmor += (baseArmor * .20f);

            baseHealth.Value += (baseHealth.Value * .20f);
            finalHealth.Value = baseHealth.Value;

            gmScript.SavePlayerData(); //save data
            gmScript.UpdateUI();
        }

  
        finalMana = baseMana;
        currentMana = finalMana;

        sliderHealthWS.maxValue = finalHealth.Value;
        sliderHealthWS.value = currentHealth.Value;

        //gmScript.SavePlayerData(); //save data
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
            if(attackCooldown >= 0) {
            cooldownImage.gameObject.SetActive(true); //enable cooldown image
            cooldownImage.fillAmount = (attackCooldown - 0) / (attackSpeed - 0);
            }

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
        if(targetList.Length == 0) return; //check to avoid out of bounds error
        AttackServerRpc(targetList[0].transform.GetSiblingIndex(), finalDamage, NetworkObjectId);
    }
    

    public void Attack() {
            switch (playerClass)
            {
                
                case "Warrior":
                    if(targetDirection.x < 0) { //attack anim up
                    transform.GetChild(0).localScale = new Vector3(-0.025f, 0.025f, 0);    //face the enemy before attacking
                    anim.Play("Attack1");

                    audioSource.Play();
                    }
                    else if(targetDirection.x > 0) { //attack anim left
                    transform.GetChild(0).localScale = new Vector3(0.025f, 0.025f, 0);    //face the enemy before attacking
                    //play animation
                    anim.Play("Attack1");
                     }
                break;



                case "Mage":
                    Vector2 targetPosition = targetList[0].transform.position - transform.position; //get enemy position
                    SpawnMagicBulletServerRpc(targetPosition);

                break;

                case "Assassin":
                    if(targetDirection.x < 0) {
                    transform.GetChild(0).localScale = new Vector3(-0.025f, 0.025f, 0);    //face the enemy before attacking
                    anim.Play("Attack1");
                    }
                    else if(targetDirection.x > 0) {
                    transform.GetChild(0).localScale = new Vector3(0.025f, 0.025f, 0);    //face the enemy before attacking
                    anim.Play("Attack1"); //play animation
                    }
                break;
            }
    }


    public void Attacked(float damage, ulong playerID) { //damage = 10
    if(NetworkObjectId == playerID && IsLocalPlayer) {
        Debug.Log("Attacked " + playerID);
    
        anim.Play("Hit");
        hitAudioSource.Play();





        if(isManaShieldOn) {
            float tempDamage = damage * .30f; //expample: damage = 10, 30% is = 3
            currentMana -= tempDamage;
            currentHealth.Value -= damage - tempDamage;
        }
        else {
            currentHealth.Value -= damage;
        }
        gmScript.UpdateUI();

        
        Transform temp;
        temp = damagePool.GetChild(0);
        temp.GetComponent<TextMeshProUGUI>().color = Color.red;
        temp.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        temp.SetParent(transform.GetChild(1));
        temp.gameObject.SetActive(true);
        
        //blood effects
        //SpawnBloodEffectServerRpc(NetworkObjectId);

        if(currentHealth.Value <= 0) {
            //dieAudioSource.Play();
            isDead = true;
            anim.Play("Death");
            playerCollider.enabled = false;
            deathPanel.SetActive(true); //show Died message

            if(level.Value > 1) { //TODO: 
                level.Value -= 1;
            }
            if(inventoryPanel.childCount > 0) {
                for (int i = 0; i < inventoryPanel.childCount; i++)
                {
                    Destroy(inventoryPanel.GetChild(i).gameObject);
                }
                equipedWeaponIndex = 0;
                weaponDmg = 0;
                updateStats();
                gmScript.SavePlayerData();
                EquipItemServerRpc(0, NetworkObjectId);
            }
            StartCoroutine(playerRespawnWait());
        }
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

        if(newValue < oldValue) { //less than means it recieve damage
        Transform bloodEffectTemp = bloodEffectsParent.GetChild(0);
        bloodEffectTemp.SetParent(transform.GetChild(3));
        bloodEffectTemp.gameObject.SetActive(true);
    }
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
            targetObject.AttackedClientRpc(finalDamage, networkId, 0);
        }
        else {
            targetObject.AttackedClientRpc(finalDamage, networkId, 0);
        }
    }


    //get difficulty from server
    [ServerRpc(RequireOwnership = false)]   //TODO: 
    public void GetDifficultyServerRpc() {
        SetDifficultyClientRpc(gmScript.difficulty);
    }
    [ClientRpc]
    public void SetDifficultyClientRpc(int difficulty) { ///executed on players CLIENT 
        this.gmScript.difficulty = difficulty;

        switch (gmScript.difficulty) //difficulty =1
        {
            case 0: //easy
            maps.GetChild(0).gameObject.SetActive(true); //forest
            maps.GetChild(1).gameObject.SetActive(false); //desert
            maps.GetChild(2).gameObject.SetActive(false); //snow
            maps.GetChild(3).gameObject.SetActive(false); //extreme
            break;

            case 1:
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(true);
            maps.GetChild(2).gameObject.SetActive(false);
            maps.GetChild(3).gameObject.SetActive(false);
            break;

            case 2:
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(false);
            maps.GetChild(2).gameObject.SetActive(true);
            maps.GetChild(3).gameObject.SetActive(false);
            break;

            case 3: //extreme
            maps.GetChild(0).gameObject.SetActive(false);
            maps.GetChild(1).gameObject.SetActive(false);
            maps.GetChild(2).gameObject.SetActive(false);
            maps.GetChild(3).gameObject.SetActive(true);
            skillEffectsParent.GetChild(2).GetChild(1).gameObject.SetActive(true); //enable portal
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

        if(draw == 1 && gmScript.difficulty == 0) {
            StartWeatherClientRpc();
        }
        else {
            StartCoroutine(gmScript.RedrawWeather());
        }
    }
    [ClientRpc]
    public void StartWeatherClientRpc() {
        gmScript.RainWeather();

    }




    [ServerRpc(RequireOwnership = false)]
    public void SpawnMagicBulletServerRpc(Vector2 targetPosition) {
        SpawnMagicBulletClientRpc(targetPosition);
    }






    [ServerRpc(RequireOwnership = false)]
    public void CallUpdatePlayerNameInfoServerRpc() {
        CallUpdatePlayerNameInfoClientRpc();
    }

    [ClientRpc] public void CallUpdatePlayerNameInfoClientRpc() {
        GetLocalName(username, NetworkObjectId);
    }

    void GetLocalName(string name, ulong playerID) {
       CallUpdatePlayerNameLevelWSServerRpc(name, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CallUpdatePlayerNameLevelWSServerRpc(string name, ulong NetworkObjectId) {
        UpdatePlayerNameLevelWSClientRpc(name, NetworkObjectId);
    }

    [ClientRpc]
    public void UpdatePlayerNameLevelWSClientRpc(string name, ulong playerID) { //update player name and level in world space
        //find all player scripts
        LanPlayer[] temp = FindObjectsOfType<LanPlayer>();
        gmScript.players = temp;

        foreach (var item in temp)
        {
            if(item.NetworkObjectId == playerID) {
                item.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(name + " lvl. " + level.Value);
                item.username = name;
                break;
            }
        }
    }



    [ClientRpc]
    public void SpawnMagicBulletClientRpc(Vector2 enemyPosition) {
        Transform temp = skillEffectsParent.GetChild(1).GetChild(0).GetChild(0);
        MagicBullet tempScript = temp.GetComponent<MagicBullet>();

        tempScript.direction = enemyPosition;
        temp.position = transform.position;
        temp.SetParent(skillEffectsParent.GetChild(1).GetChild(1)); //move to in Use object
        temp.gameObject.SetActive(true);
    }

    
    public void StartIntroduction() {
        GameObject ui = GameObject.FindWithTag("UI").gameObject;
        if(IsHost && finishIntro == 0) {
            ui.transform.GetChild(11).gameObject.SetActive(true); //start intro
        }
        else if(IsClient && finishIntro == 0) {
            ui.transform.GetChild(7).gameObject.SetActive(false); //disable welcome object
            ui.transform.GetChild(11).gameObject.SetActive(true); //start intro
        }
        else {
            ui.transform.GetChild(10).gameObject.SetActive(true);
            ui.transform.GetChild(14).gameObject.SetActive(true);
            ui.transform.GetChild(7).gameObject.SetActive(false); //disable welcome object
        }
    }

    [ServerRpc(RequireOwnership = false)] //TODO: item sync
    public void EquipItemServerRpc(int itemIndex, ulong playerID) {
        EquipItemClientRpc(itemIndex, playerID);
        
    }
    [ClientRpc]
    void EquipItemClientRpc(int itemIndex, ulong playerID) {
        foreach (var item in gmScript.players)
        {
            if(item.NetworkObjectId == playerID) {
                if(itemIndex == 0) {
                    item.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
                }
                else {
                    item.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = itemsPool.GetChild(itemIndex).GetComponent<LanItemSS>().itemImageWS;
                }
            }
        }
    
    }




    IEnumerator ManaRegen() { //
        while(true) {
            if(currentMana < finalMana) { //mana regen 2.5% of max mana every 1s

                currentMana += finalMana* .025f;
                gmScript.UpdateUI();
            }
            else if(currentHealth.Value < finalHealth.Value) { //health regen 1% of max health every 1s
                currentHealth.Value += finalHealth.Value * .01f;
                gmScript.UpdateUI();
            }
            yield return new WaitForSeconds(1);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableEnemyServerRpc(int enemyIndex) {
        DisableEnemyClientRpc(enemyIndex);
    }

    [ClientRpc]
    void DisableEnemyClientRpc(int enemyIndex) { //TODO:
    Transform enemy = mobsParent.transform.GetChild(enemyIndex);
    enemy.gameObject.SetActive(false);
    enemy.GetComponent<Collider2D>().enabled = false;
    }

}