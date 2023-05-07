using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameStateManager : MonoBehaviourPunCallbacks
{
    public enum GameState
    {
        PreStart,
        InProgress,
        Finish
    }

    [Header("Photon")]
    [SerializeField] private PhotonView photonView;

    private GameState gameState = GameState.PreStart;

    public static event Action gameReload;
    public static event Action gameStart;
    public static event Action gameFinish;

    private double gameStartedAt;
    private float currentTime;
    private float roundLimitTime;
    private float finishTime;

    private bool lastSecondSoundPlayed = false;

    [HideInInspector]
    public static GameStateManager instance;

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
    }

    private void Start()
    {
        Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        roundLimitTime = (float)customProperties["RoundTime"];
        finishTime = (float)customProperties["FinishTime"];
    }

    private void LateUpdate()
    {
        if (instance == null) return;
        if (instance.GetGameState() == GameState.PreStart) return;

        if(gameState == GameState.InProgress)
        {
            currentTime = (float)((PhotonNetwork.Time - gameStartedAt) % 4294967);
            if (lastSecondSoundPlayed == false && roundLimitTime - currentTime < 3f)
            {
                AudioManager.instance.PlaySound("Finish_BuildUp");
                lastSecondSoundPlayed = true;
            }
            if (currentTime > roundLimitTime && PhotonNetwork.IsMasterClient) instance.SetGameState(GameState.Finish);
        }
        else if (gameState == GameState.Finish)
        {
            currentTime = (float)((PhotonNetwork.Time - gameStartedAt) % 4294967);
            if (currentTime > finishTime && PhotonNetwork.IsMasterClient) instance.SetGameState(GameState.PreStart);
        }
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public double GetGameStartedAt()
    {
        return gameStartedAt;
    }

    public float GetTimeLimit()
    {
        if (gameState == GameState.InProgress) return roundLimitTime;
        else if (gameState == GameState.Finish) return finishTime;
        else return 0f;
    }

    public void SetGameState(GameState newGameState)
    {
        gameStartedAt = PhotonNetwork.Time;
        photonView.RPC("RPC_SetGameState", RpcTarget.AllViaServer, newGameState, gameStartedAt);
    }

    [PunRPC]
    public void RPC_SetGameState(GameState newGameState, double startTime)
    {
        if (newGameState == gameState) return;
        gameState = newGameState;
        gameStartedAt = startTime;
        lastSecondSoundPlayed = false;

        switch (gameState) 
        {
            case GameState.PreStart:
                gameReload?.Invoke();
                Debug.Log("Reload!");
                break;
            case GameState.InProgress:
                gameStart?.Invoke();
                AudioManager.instance.PlaySound("Start_Game");
                Debug.Log("Start!");
                break;
            case GameState.Finish:
                gameFinish?.Invoke();
                AudioManager.instance.PlaySound("Finish");
                Debug.Log("Finish!");
                break;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("RPC_SetGameState", RpcTarget.AllViaServer, gameState, gameStartedAt);
    }
}
