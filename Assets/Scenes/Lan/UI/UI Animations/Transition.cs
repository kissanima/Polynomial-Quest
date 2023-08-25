using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanTransition : MonoBehaviour
{
    public GameObject playerPrefab;
    public LanGameManager gmScript;
    public void MyAnimationEvent() {
        gameObject.SetActive(false);
    }

    void SpawnPlayer() {
    }
}
