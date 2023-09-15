using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanRewardsLabel : MonoBehaviour
{
    [SerializeField]Transform rewardsLabelPool;


    void AnimationEvent() {
        transform.SetParent(rewardsLabelPool); 
        gameObject.SetActive(false);
    }
}
