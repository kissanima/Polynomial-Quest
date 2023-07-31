using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    LanPlayer player;

    private void Start() {
        player = transform.parent.GetComponent<LanPlayer>();
    }

    public void Event() {
        player.EventAttack();
    }
    
}
