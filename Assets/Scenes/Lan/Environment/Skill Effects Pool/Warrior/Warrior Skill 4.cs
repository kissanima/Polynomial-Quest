using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WarriorSkill4 : MonoBehaviour
{
    LanGameManager gmScript;
    Transform skillEffectsPool;
    public float ownerID;

    public float damageReduction = 15f, damage = 100;

    private void OnEnable() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();

        
        if(ownerID != gmScript.player.NetworkObjectId) return;
        skillEffectsPool = GameObject.FindWithTag("SkillEffects").transform;

        gmScript.player.damageReduction += damageReduction;  //
        gmScript.player.baseDamage += damage;
        gmScript.player.updateStats();
    }

    private void OnDisable() {
        gmScript.player.damageReduction -= damageReduction;
        gmScript.player.baseDamage -= damage;
        gmScript.player.updateStats();
    }

}