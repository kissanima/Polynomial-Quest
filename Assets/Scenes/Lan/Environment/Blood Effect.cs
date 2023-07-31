using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    Transform parent;
    private void Awake() {
        parent = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>().player.bloodEffectsParent;
    }
    public void AnimationEvent() {
        gameObject.SetActive(false);
        transform.SetParent(parent);
        

    }

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
    }
}
