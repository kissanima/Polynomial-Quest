using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;

public class LanDetectEnemy : MonoBehaviour
{
    LanPlayer player;
    private void Start() {
        player = transform.parent.GetComponent<LanPlayer>();
        if(player.IsOwnedByServer) {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Enemy")) return;

        //other.transform.GetChild(3).GetComponent<Animator>().enabled = true;
        //other.GetComponent<ClientNetworkTransform>().enabled = true;

        other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = true;
        other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!other.CompareTag("Enemy")) return;

        //other.transform.GetChild(3).GetComponent<Animator>().enabled = true;
        //other.GetComponent<ClientNetworkTransform>().enabled = true;

        other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = false;
        other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = false;
    }
}
