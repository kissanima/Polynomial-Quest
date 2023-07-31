using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameManager gmScript;
    public void MyAnimationEvent() {
        gameObject.SetActive(false);
    }

    void SpawnPlayer() {
    }
}
