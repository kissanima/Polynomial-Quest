using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePop : MonoBehaviour
{
    public GameObject damagePool;
    public void AnimationEvent() {
        gameObject.SetActive(false);
        transform.SetParent(damagePool.transform); //return to original parent
    }

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
    }
}
