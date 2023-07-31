using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Emanuel : NetworkBehaviour
{
    public GameObject interactButton, attackButton;
    public GameManager gmScript;
    public LanPlayer player;
    public string[] question, choices;
    public int answerIndex, attempts;
    public bool isNpc;
    


    public override void OnNetworkSpawn() {

        LanPlayer[] players = FindObjectsOfType<LanPlayer>();
        foreach (LanPlayer p in players)
        {
            if (p.IsLocalPlayer)
            {
                player = p;
                break;
            }
        }
    }

    private void Start() {
        int draw = Random.Range(0, 10);

        //GetComponent<LanPlayer>().enabled = true;
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        if(!isNpc) {
            try
        {                                                               //difficulty 
            question = rootObjects[3].transform.GetChild(4).GetChild(1).GetChild(0).GetChild(draw).GetComponent<Questions>().question;
            choices = rootObjects[3].transform.GetChild(4).GetChild(1).GetChild(0).GetChild(draw).GetComponent<Questions>().choices;
            attempts = rootObjects[3].transform.GetChild(4).GetChild(1).GetChild(0).GetChild(draw).GetComponent<Questions>().attempts;
        }
        catch (System.Exception)
        {
            
            throw;
        }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        interactButton.SetActive(true);
        attackButton.SetActive(false);
        player.npc = this.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other) {
        attackButton.SetActive(true);
        interactButton.SetActive(false);
        player.npc = null;
    }
}
