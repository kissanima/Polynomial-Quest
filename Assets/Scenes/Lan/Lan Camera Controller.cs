using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LanCameraController : MonoBehaviour {
    Transform player;
    LanGameManager gmScript;
    bool hasInitialized;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        player = gmScript.player.transform;
        GetComponent<Camera>().enabled = true;
        hasInitialized = true;
    }

    
    void FixedUpdate() {
        if(!hasInitialized) return;

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }
}
