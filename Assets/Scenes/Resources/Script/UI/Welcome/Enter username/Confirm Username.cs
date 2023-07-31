using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;

public class ConfirmUsername : MonoBehaviour
{
    public Player player;
    public GameManager gmScript;

    public TMP_InputField textField;
    public TextMeshProUGUI console;
    public GameObject welcomePanel, selectDifficulty, enterUsername;
    



    public void ButtonPressed() {
        string username = textField.text;
        saveUsername(username);
    }

    void saveUsername(string username) {
        console.SetText(console.text + "\n save method called");
        try {

        FirebaseDatabase.DefaultInstance.GetReference("users").Child(gmScript.user.UserId).Child("username").SetValueAsync(username);
        gmScript.UpdateUI();
        enterUsername.SetActive(false);
        selectDifficulty.SetActive(true);
        } 
        catch (System.Exception e) {
            console.SetText(console.text + "\n" + "An error occurred while saving the username: " + e.Message);
        }
    }


}
    

