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
        if(!player.IsLocalPlayer) {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy") && !player.IsOwnedByServer) { //the parent of this object is a client
            other.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = true;
            other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = true;

            other.transform.GetChild(1).gameObject.SetActive(true); //disable slider
            other.transform.GetChild(4).gameObject.SetActive(true); //disable circle
        }
        else if(other.CompareTag("Enemy")) {
            other.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
            other.transform.GetChild(1).gameObject.SetActive(true); //disable slider
            other.transform.GetChild(4).gameObject.SetActive(true); //disable
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Enemy") && !player.IsOwnedByServer) { //the parent of this object is a client
            other.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = false;
            other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = false;
            other.transform.GetChild(1).gameObject.SetActive(false); //disable slider
            other.transform.GetChild(4).gameObject.SetActive(false); //disable
        }
        else if(other.CompareTag("Enemy")) {
            other.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false); //disable slider
            other.transform.GetChild(4).gameObject.SetActive(false); //disable
        }
    }
}
