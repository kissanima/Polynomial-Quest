using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSKill2 : MonoBehaviour
{
    LanGameManager gmScript;
    public float finalDamage, additionalDamagePercentage = 1f;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        finalDamage = (gmScript.player.finalDamage * additionalDamagePercentage) + 50f;
    }

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag != "Enemy") return;
        gmScript.player.AttackServerRpc(other.transform.GetSiblingIndex(), finalDamage, gmScript.player.OwnerClientId);
    }
}
