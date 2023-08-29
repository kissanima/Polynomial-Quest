using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanDungeonEntrance : MonoBehaviour
{
    public Transform player;
    Vector3 spawnPoint;
    LanGameManager gmScript;
    public GameObject transition;
    [SerializeField] float minimumLevelRequirement;

    private void Start() {
        spawnPoint = transform.parent.GetChild(1).position;
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        transition = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && gmScript.player.level.Value >= minimumLevelRequirement && other.GetComponent<LanPlayer>().IsLocalPlayer) {
            transition.gameObject.SetActive(true);
            gmScript.player.transform.position = transform.parent.GetChild(1).position;
        }
    }


}
