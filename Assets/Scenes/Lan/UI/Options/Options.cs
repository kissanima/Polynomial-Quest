using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    Transform ui;
    private void Start() {
        ui = transform.parent;
    }
    public void OpenOption() {
        if(gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
        }
    }


    public void ReturnMenu() {
        gameObject.SetActive(false);
        ui.GetChild(10).gameObject.SetActive(false); //disable minimap
        ui.GetChild(7).gameObject.SetActive(true); //disable welcome/menu
        ui.GetChild(7).GetChild(1).gameObject.SetActive(true); //disable welcome/menu
    }

    public void OptionResume() {
        ui.GetChild(7).gameObject.SetActive(false); //disable welcome/menu
        ui.GetChild(10).gameObject.SetActive(true); //enable minimap
    }


}
