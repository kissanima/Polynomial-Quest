using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public InteractionManager interaction;

    public void ButtonPressed() {
        interaction.Continue();
    }
}
