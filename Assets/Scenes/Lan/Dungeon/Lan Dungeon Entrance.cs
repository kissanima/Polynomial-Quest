using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanDungeonEntrance : MonoBehaviour
{
    public Transform player;
    Vector3 spawnPoint;
    LanGameManager gmScript;
    public GameObject transition;

    private void Start() {
        spawnPoint = transform.parent.GetChild(1).position;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        transition = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player") return;
        transition.gameObject.SetActive(true);
        gmScript.player.transform.position = transform.parent.GetChild(1).position;
    }


}
