using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill4 : MonoBehaviour
{
    LanGameManager gmScript;
    Transform skillEffectsPool;

    public float damageReduction = 15f, damage = 100;

    public void Initialize() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        skillEffectsPool = GameObject.FindWithTag("SkillEffects").transform;
        //finalDamage = (gmScript.player.finalDamage * (additionalDamagePercentage / 100)) + 100f;
    }

    private void OnEnable() {
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
