using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WarriorSkill4 : MonoBehaviour
{
    [SerializeField] LanGameManager gmScript;
    AudioSource audioSource;
    public float ownerID;

    public float damageReduction = 15f, damage = 100;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() {
        audioSource.Play();

        
        if(ownerID != gmScript.player.NetworkObjectId) return;

        gmScript.player.damageReduction += damageReduction;  //
        gmScript.player.baseDamage += damage;
        gmScript.player.UpdateStats();
    }

    private void OnDisable() {
        gmScript.player.damageReduction -= damageReduction;
        gmScript.player.baseDamage -= damage;
        gmScript.player.UpdateStats();
    }

}
