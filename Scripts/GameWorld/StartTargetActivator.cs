using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTargetActivator : MonoBehaviour
{
    [Header("Object to activate")]
    [SerializeField] private GameObject targetWithText;

    private void OnEnable()
    {
        GameStateManager.gameReload += EnableTarget;
        GameStateManager.gameStart += DisableTarget;
    }

    private void OnDisable()
    {
        GameStateManager.gameReload -= EnableTarget;
        GameStateManager.gameStart -= DisableTarget;
    }

    private void EnableTarget()
    {
        targetWithText.SetActive(true);
    }

    private void DisableTarget()
    {
        targetWithText.SetActive(false);
    }
}
