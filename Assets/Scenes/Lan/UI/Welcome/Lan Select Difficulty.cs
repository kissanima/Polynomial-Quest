using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanSelectDifficulty : MonoBehaviour
{
    public LanGameManager gmScript;
    [SerializeField] GameObject welcome, nature, desert, snow;
    public Transform enemyManager;

    private void Awake() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        welcome = GameObject.FindWithTag("UI").transform.GetChild(7).gameObject;
        enemyManager = GameObject.FindWithTag("EnemyManager").transform.GetChild(0);

        //maps
        nature = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(0).gameObject;
        desert = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(1).gameObject;
        snow = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(2).gameObject;
        
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
            break;

            case 1:
            gmScript.enemyStatsModifier = 200;
            gmScript.difficulty = 1;
            nature.SetActive(false);
            desert.SetActive(true);
            snow.SetActive(false);
            break;

            case 2:
            gmScript.enemyStatsModifier = 350;
            gmScript.difficulty = 2;
            nature.SetActive(false);
            desert.SetActive(false);
            snow.SetActive(true);
            break;

            case 3:
            gmScript.enemyStatsModifier = 500;
            gmScript.difficulty = 3;
            break;
        }
    }

    public void ButtonPressed()
    {
        for(int i = 0; i < enemyManager.childCount; i++) {
            enemyManager.GetChild(i).GetComponent<MobsMelee>().UpdateStats();
        }
        welcome.SetActive(false);
    }
}
