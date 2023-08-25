using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class LanDetectEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Enemy") return;
        other.transform.GetChild(3).GetComponent<Animator>().enabled = true;
        other.GetComponent<ClientNetworkTransform>().enabled = true;
        other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = true;
        other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = true;
        other.GetComponent<LanMobsMelee>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag != "Enemy") return;
        other.GetComponent<ClientNetworkTransform>().enabled = false;
        other.transform.GetChild(3).GetComponent<ClientNetworkAnimator>().enabled = false;
        other.transform.GetChild(3).GetComponent<ClientNetworkTransform>().enabled = false;
        other.GetComponent<LanMobsMelee>().enabled = false;
        other.transform.GetChild(3).GetComponent<Animator>().enabled = false;
    }
}
