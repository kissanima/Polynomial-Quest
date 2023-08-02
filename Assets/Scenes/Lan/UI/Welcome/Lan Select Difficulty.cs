using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanSelectDifficulty : MonoBehaviour
{
    public LanGameManager gmScript;
    public GameObject welcome, Nature, Desert;
    public Transform enemyManager;

    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        welcome = GameObject.FindWithTag("UI").transform.GetChild(7).gameObject;
        Nature = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(0).gameObject;
        Desert = GameObject.FindWithTag("Environment").transform.GetChild(0).GetChild(1).gameObject;
        enemyManager = GameObject.FindWithTag("EnemyManager").transform.GetChild(0);
    }
    public void onValueChange(int var)
    {
        switch (var)
        {
            case 0:
            gmScript.enemyStatsModifier = 100;
            gmScript.difficulty = 0;
            Nature.SetActive(true);
            break;

            case 1:
            gmScript.enemyStatsModifier = 200;
            gmScript.difficulty = 1;
            Desert.SetActive(true);
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
            Nature.SetActive(true);
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
