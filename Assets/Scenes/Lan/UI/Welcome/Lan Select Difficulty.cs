using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanSelectDifficulty : MonoBehaviour
{
    [SerializeField] LanGameManager gmScript;
    [SerializeField] GameObject welcome, nature, desert, snow, decay;
    public Transform enemyManager;
    Transform skillEffectsParent, startButton;
    int playerLevel;
    TextMeshProUGUI difficultyText;

    private void Awake() {
        playerLevel = PlayerPrefs.GetInt("level");
        welcome = GameObject.FindWithTag("UI").transform.GetChild(7).gameObject;
        enemyManager = GameObject.FindWithTag("EnemyManager").transform.GetChild(0);
        skillEffectsParent = GameObject.FindWithTag("SkillEffects").transform;
        startButton = transform.GetChild(2);
        difficultyText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        //maps
        nature = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(0).gameObject;
        desert = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(1).gameObject;
        snow = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(2).gameObject;
        decay = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(3).gameObject;
    }
    public void onValueChange(int var)
    {
        switch (var)
        {
            case 0:
            gmScript.enemyStatsModifier = 100;
            gmScript.difficulty = 0;
            nature.SetActive(true);
            desert.SetActive(false);
            snow.SetActive(false);
            decay.SetActive(false);

            startButton.gameObject.SetActive(true);
            difficultyText.color = Color.white;
            difficultyText.SetText("Select Difficulty: ");
            break;

            case 1:
            gmScript.enemyStatsModifier = 200;
            gmScript.difficulty = 1;
            nature.SetActive(false);
            desert.SetActive(true);
            snow.SetActive(false);
            decay.SetActive(false);

            if(playerLevel < 6) {
                startButton.gameObject.SetActive(false);
                difficultyText.color = Color.red;
                difficultyText.SetText("Level 6 or higher required");
            }
            break;

            case 2:
            gmScript.enemyStatsModifier = 350;
            gmScript.difficulty = 2;
            nature.SetActive(false);
            desert.SetActive(false);
            snow.SetActive(true);
            decay.SetActive(false);

            if(playerLevel < 16) {
                startButton.gameObject.SetActive(false);
                difficultyText.color = Color.red;
                difficultyText.SetText("Level 16 or higher required");
            }
            break;

            case 3:
            gmScript.enemyStatsModifier = 500;
            gmScript.difficulty = 3;
            nature.SetActive(false);
            desert.SetActive(false);
            snow.SetActive(false);
            decay.SetActive(true);
            skillEffectsParent.GetChild(2).GetChild(1).gameObject.SetActive(true); //enable portal

            if(playerLevel < 26) {
                startButton.gameObject.SetActive(false);
                difficultyText.color = Color.red;
                difficultyText.SetText("Level 26 or higher required");
            }
            break;
        }

    }

    public void ButtonPressed()
    {
        for(int i = 0; i < enemyManager.childCount; i++) {
            enemyManager.GetChild(i).GetComponent<LanMobsMelee>().UpdateStats();
        }
        welcome.SetActive(false);
    }
}
