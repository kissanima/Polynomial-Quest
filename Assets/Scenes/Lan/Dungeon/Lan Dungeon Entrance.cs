using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanDungeonEntrance : MonoBehaviour
{
    Transform player;
    Vector3 spawnPoint;
    LanGameManager gmScript;
    public GameObject transition;
    [SerializeField] float minimumLevelRequirement;
    [SerializeField] Transform popPool;
    

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
        else if(other.CompareTag("Player")) { //level not enough
            TextMeshProUGUI pop = popPool.GetChild(0).GetComponent<TextMeshProUGUI>();
            pop.SetText("Requirements not met!");
            pop.fontSize = 55;
            pop.color = Color.red;
            pop.transform.SetParent(transform);
            pop.gameObject.SetActive(true);
            
        
        }
    }


}
