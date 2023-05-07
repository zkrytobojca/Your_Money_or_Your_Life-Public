using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UITimeDisplay : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private TextMeshProUGUI timeLabelText;
    [SerializeField] private TextMeshProUGUI timerText;
    private double roundStartTime;
    private float currentTime;
    private float timeLimit;

    private void OnEnable()
    {
        GameStateManager.gameReload += DisableTimer;
        GameStateManager.gameStart += GameStarted;
        GameStateManager.gameFinish += GameFinishing;
    }

    private void OnDisable()
    {
        GameStateManager.gameReload -= DisableTimer;
        GameStateManager.gameStart -= GameStarted;
        GameStateManager.gameFinish -= GameFinishing;
    }

    private void DisableTimer()
    {
        timeLabelText.text = "Wating for start";
        timerText.text = "--:--";
    }

    private void GameStarted()
    {
        timeLabelText.text = "Round ends in:";
        roundStartTime = GameStateManager.instance.GetGameStartedAt();
        timeLimit = GameStateManager.instance.GetTimeLimit();
    }

    private void GameFinishing()
    {
        timeLabelText.text = "Restarting in:";
        roundStartTime = GameStateManager.instance.GetGameStartedAt();
        timeLimit = GameStateManager.instance.GetTimeLimit();
    }

    private void LateUpdate()
    {
        if (GameStateManager.instance.GetGameState() == GameStateManager.GameState.PreStart) return;
        currentTime = (float)((PhotonNetwork.Time - roundStartTime) % 4294967);
        currentTime = Mathf.FloorToInt(Mathf.Abs(currentTime));
        float minutes = Mathf.FloorToInt((timeLimit - currentTime) / 60);
        float seconds = (timeLimit - currentTime) % 60;
        timerText.text = (minutes == 0f ? "0" : "") + minutes.ToString() + ":" + (seconds < 10f ? "0" : "") + seconds.ToString();
    }
}
