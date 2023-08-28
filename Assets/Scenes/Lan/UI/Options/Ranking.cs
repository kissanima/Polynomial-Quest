using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class Ranking : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI byScore, byLevel;
    [SerializeField] LanGameManager gmScript;
    int arrayLength;
    LanPlayer[] players;
    LanPlayer temp;

    private void OnEnable() {
        players = gmScript.players;
        arrayLength = players.Length;
        
        ByScore();
    }

    public void ByScore() {
        for(int i = 0; i < arrayLength; i++) {
            for(int j = 0; j < arrayLength - i - 1; j++) {
                if(players[j].score.Value > players[j+1].score.Value ) {
                    temp = players[j];
                    players[j] = players[j+1];
                    players[j+1] = temp;
                }
            }
        }
        byScore.SetText("");
        foreach (var item in players)
        {
            byScore.SetText("Name                 SCORE \n" + byScore.text + "\n" + item.username + "              " + item.score.Value.ToString() + "\n");
        }
        byLevel.gameObject.SetActive(false);
        byScore.gameObject.SetActive(true);
    }

    public void ByLevel() {
        for(int i = 0; i < arrayLength; i++) {
            for(int j = 0; j < arrayLength - i - 1; j++) {
                if(players[j].level.Value > players[j+1].level.Value ) {
                    temp = players[j];
                    players[j] = players[j+1];
                    players[j+1] = temp;
                }
            }
        }
        byLevel.SetText("");
        foreach (var item in players)
        {
            byScore.SetText("Name                 SCORE \n" + byScore.text + "\n" + item.username + "              " + item.level.ToString() + "\n");
        }
        byLevel.gameObject.SetActive(true);
        byScore.gameObject.SetActive(false);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
