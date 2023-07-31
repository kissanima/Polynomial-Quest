using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanDungeonExit : MonoBehaviour
{
    public Transform player;
    public GameObject transition;
    LanGameManager gmScript;

    private void Start() {
        
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        transition = GameObject.FindWithTag("UI").transform.GetChild(5).GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Player") return;
        transition.gameObject.SetActive(true);
        gmScript.player.transform.position = transform.parent.GetChild(3).position;
    }
}
