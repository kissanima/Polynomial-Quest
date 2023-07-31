using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public LanGameManager gmScript;
    public int itemIndex = 0;
    public GameObject inventoryPanel, itemPool;
    public Player player;

    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        inventoryPanel = GameObject.FindWithTag("UI").transform.GetChild(4).gameObject;
        itemPool = inventoryPanel.transform.GetChild(1).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        itemPool.transform.GetChild(itemIndex-1).gameObject.SetActive(true); //get item from pool
        itemPool.transform.GetChild(itemIndex).SetParent(inventoryPanel.transform);
        int temp = inventoryPanel.transform.childCount; //get child count of inventory panel
        gmScript.player.inventory[temp] = itemIndex.ToString();  //set value to array
        Instantiate(itemPool.transform.GetChild(itemIndex-1), inventoryPanel.transform); //make a copy of the item
        gmScript.SavePlayerData();
        gameObject.SetActive(false);
    }
}
