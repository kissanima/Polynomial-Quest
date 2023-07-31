using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public Transform player;
    Vector3 spawnPoint;
    LanGameManager gmScript;
    public GameObject transition;

    private void Start() {
        spawnPoint = transform.parent.GetChild(1).position;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        transition.gameObject.SetActive(true);
        gmScript.player.transform.position = spawnPoint;
    }


}
