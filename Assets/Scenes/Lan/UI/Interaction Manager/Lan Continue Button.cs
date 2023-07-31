using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanContinueButton : MonoBehaviour
{
    LanInteractionManager interaction;

    private void OnEnable() {
        interaction = GameObject.FindWithTag("InteractionManager").GetComponent<LanInteractionManager>();
    }

    public void ButtonPressed() {
        interaction.Continue();
    }
}
