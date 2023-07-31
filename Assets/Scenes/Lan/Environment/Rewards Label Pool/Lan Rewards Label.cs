using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanRewardsLabel : MonoBehaviour
{
    Transform rewardsLabelPool;
    private void Start() {
        rewardsLabelPool = GameObject.FindWithTag("RewardsLabelPool").transform;
    }

    void AnimationEvent() {
        transform.SetParent(rewardsLabelPool); 
        gameObject.SetActive(false);
    }
}
