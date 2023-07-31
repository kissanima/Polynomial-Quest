using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class GameManager : MonoBehaviour
{
    public Slider sliderExp, sliderHealth, sliderMana, sliderHealthWS; //sliderHealthWS worlds space slider

    //initialize variables
    TextMeshProUGUI healthText, manaText, expText;
    public TextMeshProUGUI usernameSS, usernameWS; //SS = screen Space, WS = world space
    public TextMeshProUGUI potionText, console;
    public Player player;
    public float enemyStatsModifier;
    public GameObject inventoryPanel, itemPool;
    public int difficulty;


    //authentication variables
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public DatabaseReference reference;

    private void Start() {
        Application.targetFrameRate = 60;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //initialize components
        sliderHealthWS = player.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        healthText = sliderHealth.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        manaText = sliderMana.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        expText = sliderExp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        UpdateUI();
    }
    
    public void UpdateUI() {
        sliderHealth.maxValue = player.finalHealth;
        sliderHealth.value = player.currentHealth;
        healthText.SetText(player.currentHealth.ToString() + " / " + player.finalHealth.ToString());
        sliderHealthWS.maxValue = player.finalHealth;
        sliderHealthWS.value = player.currentHealth;
        
        sliderMana.maxValue = player.finalMana;
        sliderMana.value = player.currentMana;
        manaText.SetText(player.currentMana.ToString() + " / " + player.finalMana.ToString());

        sliderExp.maxValue = player.finalRequiredExp;
        sliderExp.value = player.currentExp;
        expText.SetText(player.currentExp.ToString() + " / " + player.finalRequiredExp.ToString());

        potionText.SetText(player.potion.ToString());

        usernameSS.SetText(player.username);
        usernameWS.SetText(player.username);
    }

    public void LoadPlayerData() {
    console.SetText(console.text + "\n load method called.");
    FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId.ToString())
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                console.SetText(console.text + "\n Error retrieving data: " + task.Exception); // Handle the error...

            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                //get the values from snapshot
                string username = snapshot.Child("username").Value.ToString();
                player.username = username;

                string exp = snapshot.Child("exp").Value.ToString();
                string[] inventory = snapshot.Child("inventory").Value as string[];
                console.SetText(console.text + "\n" + inventory.ToString());
                string level = snapshot.Child("level").Value.ToString();
                string potions = snapshot.Child("potions").Value.ToString();
                Debug.Log(potions);
                
                
                //assign values to player
                player.currentExp = float.Parse(exp);
                //player. inventory = inventory;
                player.level = float.Parse(level);
                player.potion = float.Parse(potions);
                player.username = username;

                //instantiate items base on index
                /*for(int i = 0; i < player. inventory.Length; i++) {
                    if(player. inventory[i] != "0") {
                        console.SetText(console.text + "\n" + player. inventory[i]);
                        itemPool.transform.GetChild(int.Parse(player. inventory[i]) - 1).gameObject.SetActive(true);
                        Instantiate(itemPool.transform.GetChild(int.Parse(player. inventory[i]) - 1), inventoryPanel.transform); //make a copy of the item
                    }
                } */
            }
        });

    UpdateUI();
    } 

    public void SavePlayerData() {
        try
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("exp").SetValueAsync(player.currentExp);
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("level").SetValueAsync(player.level);
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("potions").SetValueAsync(player.potion);
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.UserId).Child("inventory").SetValueAsync(player.inventory);
            console.SetText(console.text + "\n Data saved succesfully!");
        }
        catch (System.Exception e)
        {
            console.SetText(console.text + "\n" + "An error occurred while saving Player Data: " + e.Message);
        }
    }

    
    
}
