using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultKeybingindsButton : MonoBehaviour
{
    public void SetKeybindingToDefault()
    {
        if (InputManager.instance == null) return;

        InputManager.instance.ResetAllBindigns();
    }
}
