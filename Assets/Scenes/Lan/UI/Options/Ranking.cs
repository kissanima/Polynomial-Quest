using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Ranking : MonoBehaviour
{
    LanGameManager gmScript;
    List<Tuple<int, string >> players;
    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }

    private void OnEnable() {
        players = new List<Tuple<int, string>>();

        foreach (var player in gmScript.players) {
            players.Add(new Tuple<int, string> (Mathf.FloorToInt(player.score.Value), player.username));
        }
    }

    //var sortedPlayers = players.OrderBy(player)
}
