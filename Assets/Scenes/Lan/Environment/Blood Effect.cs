using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] LanGameManager gmScript;
    Transform parent;
    private void Awake() {
        parent = gmScript.player.bloodEffectsParent;
    }
    public void AnimationEvent() {
        gameObject.SetActive(false);
        transform.SetParent(parent);
        

    }

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
    }
}
