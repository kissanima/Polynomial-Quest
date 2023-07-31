using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    public Transform player;
    public Vector3 spawnPoint;
    public GameObject transition;

    private void Start() {
        spawnPoint = new Vector3(-5.049f, -7.20f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        transition.gameObject.SetActive(true);
        player.position = spawnPoint;
    }
}
