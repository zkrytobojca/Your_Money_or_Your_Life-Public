using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumActivator : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject podiumModel;

    private void OnEnable()
    {
        GameStateManager.gameFinish += EnableTarget;
        GameStateManager.gameReload += DisableTarget;
    }

    private void OnDisable()
    {
        GameStateManager.gameFinish -= EnableTarget;
        GameStateManager.gameReload -= DisableTarget;
    }

    private void EnableTarget()
    {
        podiumModel.SetActive(true);
    }

    private void DisableTarget()
    {
        podiumModel.SetActive(false);
    }
}
