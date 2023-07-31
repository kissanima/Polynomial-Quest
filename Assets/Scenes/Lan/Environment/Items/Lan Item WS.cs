using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanItemsWS : MonoBehaviour
{
    LanGameManager gmScript;
    public int itemIndex;
    GameObject inventoryManager, itemPool;

    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        inventoryManager = GameObject.FindWithTag("UI").transform.GetChild(4).gameObject;
        itemPool = inventoryManager.transform.GetChild(1).gameObject;
        gameObject.name = gameObject.name.Replace("Clone()", "");
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "enemy" || other == null) return;   //check if object collided is enemy
        itemPool.transform.GetChild(itemIndex-1).gameObject.SetActive(true);
        //itemPool.transform.GetChild(itemIndex).SetParent(inventoryPanel.transform);

        
        int temp = inventoryManager.transform.GetChild(0).childCount; //get child count of inventory panel
        gmScript.player.inventory[temp] = itemIndex.ToString();  //set value to array
        Instantiate(itemPool.transform.GetChild(itemIndex-1), inventoryManager.transform.GetChild(0)); //make a copy of the item
        gmScript.SavePlayerData();
        gameObject.SetActive(false);
    }
}
