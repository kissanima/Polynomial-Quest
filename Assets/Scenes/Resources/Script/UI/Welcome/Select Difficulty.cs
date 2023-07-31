using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDifficulty : MonoBehaviour
{
    public GameManager gmScript;
    public GameObject welcome, Nature, Snow;
    public Transform enemyManager;
    public void onValueChange(int var)
    {
        switch (var)
        {
            case 0:
            gmScript.enemyStatsModifier = 100;
            gmScript.difficulty = 0;
            Nature.SetActive(true);
            Snow.SetActive(false);
            break;

            case 1:
            gmScript.enemyStatsModifier = 200;
            gmScript.difficulty = 1;
            Nature.SetActive(false);
            Snow.SetActive(true);
            break;

            case 2:
            gmScript.enemyStatsModifier = 350;
            gmScript.difficulty = 2;
            break;

            case 3:
            gmScript.enemyStatsModifier = 500;
            gmScript.difficulty = 3;
            break;

            default:
            gmScript.enemyStatsModifier = 100;
            gmScript.difficulty = 0;
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
