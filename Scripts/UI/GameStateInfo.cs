using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInfo : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject gameStartedInfo;

    private void OnEnable()
    {
        GameStateManager.gameStart += ShowGameStartedInfo;
    }

    private void OnDisable()
    {
        GameStateManager.gameStart -= ShowGameStartedInfo;
    }

    private void ShowGameStartedInfo()
    {
        StartCoroutine(ShowGameStartedInfoIE());
    }

    private IEnumerator ShowGameStartedInfoIE()
    {
        gameStartedInfo.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameStartedInfo.SetActive(false);
    } 
}
