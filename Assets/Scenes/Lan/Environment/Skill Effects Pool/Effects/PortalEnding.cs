using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEnding : MonoBehaviour
{
    LanGameManager gmScript;
    [SerializeField]bool isExit, isEntrance;
    void Start()
    {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && isEntrance && gmScript.dungeonStatues == 21) {
            gmScript.isPortalFound = true;
            gmScript.UpdateMission();

            gmScript.player.transform.position = transform.parent.GetChild(2).position; //teleport
        }
        else if(other.CompareTag("Player") && isExit) {
            gmScript.player.transform.position = transform.parent.GetChild(3).position;
        }
    }
}
