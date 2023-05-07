using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [HideInInspector]
    public static CursorManager instance;

    [Header("Permissions flags")]
    public bool canBeLocked = true;
    public bool canBeHidden = true;
    [Header("State flags")]
    public bool isLocked = false;
    public bool isHidden = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);

        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor(bool doLock)
    {
        if (!canBeLocked && doLock == true) return;
        if(doLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isLocked = false;
        }
    }

    public void HideCursor(bool doHide)
    {
        if (!canBeHidden) return;
        Cursor.visible = !doHide;
        isHidden = doHide;
    }
}
