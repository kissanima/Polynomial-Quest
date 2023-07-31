using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanDamagePop : MonoBehaviour
{
    public GameObject damagePool;
    public void AnimationEvent() {
        gameObject.SetActive(false);
        transform.SetParent(damagePool.transform);
    }

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
    }
}
