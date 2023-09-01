using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    [SerializeField] Transform WilsonTransform;
    TextMeshProUGUI wilsonText;
    Vector2 wilsonPosition;
    LanGameManager gmScript;
    private void Start() {
        gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
        wilsonPosition = new(0.588f, -0.072f);
        wilsonText = WilsonTransform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    void finishFade() {
        gameObject.SetActive(false);
    }

    void TeleportPlayersEvent() {
        foreach (var item in gmScript.players) //teleport players
        {
            item.transform.position = Vector2.zero;
        }

        WilsonTransform.position = wilsonPosition;
        wilsonText.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator WilsonDialogue1() {
        wilsonText = null;
        string dialogue1 = "Thank you!! Brave one. The darkness among us has been extinguished by your bravery, determination, and mastery of polynomials.";
        foreach (var item in dialogue1)
        {
            wilsonText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
    }
}
