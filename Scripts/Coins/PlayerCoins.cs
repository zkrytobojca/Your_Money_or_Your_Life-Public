using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerCoins : MonoBehaviourPunCallbacks, IDamageable
{
    [Header("Coins")]
    [SerializeField] private int coins = 20;
    [SerializeField] private int startingCoins = 20;
    [SerializeField] private int rejoinCoins = 1;
    [SerializeField] private GameObject coinPrefab;
    [Header("Photon")]
    [SerializeField] private PhotonView photonView;
    [Header("Display")]
    [SerializeField] private GameObject coinCup;

    public event Action<int> updateCoinsRPC;
    public event Action tookDamage;
    public static event Action<int> updateCoinsUI;

    public override void OnEnable()
    {
        RankingManager.instance.updateTopPlayer += ToggleCoinCup;
        GameStateManager.gameReload += CleanUp;
    }

    public override void OnDisable()
    {
        RankingManager.instance.updateTopPlayer -= ToggleCoinCup;
        GameStateManager.gameReload -= CleanUp;
    }

    private void Start()
    {
        SetCoins((RankingManager.instance.CheckIfPlayerRejoined(photonView.Controller.UserId) && GameStateManager.instance.GetGameState() != GameStateManager.GameState.PreStart) ? rejoinCoins : startingCoins);
    }

    public void TakeDamage(int amount, RaycastHit hit)
    {
        if (GameStateManager.instance.GetGameState() == GameStateManager.GameState.PreStart) return;
        int numberOfCoinsToInstantiate = Mathf.Min(coins, amount);
        int coinForSpare = numberOfCoinsToInstantiate == coins ? 1 : 0;
        tookDamage?.Invoke();
        StartCoroutine(DropCoinsCorutine(numberOfCoinsToInstantiate - coinForSpare, hit.point));
        SetCoins(coins - amount);
    }

    public void PickUpCoins(int amount)
    {
        if (!photonView.IsMine) return;
        SetCoins(coins + amount);
    }

    public void SetCoins(int amount, bool doUpdateCoinsRPC = true)
    {
        coins = amount;

        if (photonView.IsMine)
        {
            if (coins <= 0)
            {
                coins = 1;
                SpawnManager.instance.SpawnPlayer();
            }

            Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            customProperties["Coins"] = coins;
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            updateCoinsUI?.Invoke(coins);
        }
        if (doUpdateCoinsRPC) updateCoinsRPC?.Invoke(coins);
        if(!photonView.IsMine && coins <= 0) coins = 1;
    }

    IEnumerator DropCoinsCorutine(int numberOfCoinsToInstantiate, Vector3 point)
    {
        for (int i = 0; i != numberOfCoinsToInstantiate; i++)
        {
            PhotonNetwork.Instantiate(coinPrefab.name, point, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        if (photonView.IsMine) return;
        if (GameStateManager.instance.GetGameState() == GameStateManager.GameState.PreStart) return;
        for (int i = 0; i < coins-1; i++)
        {
            PhotonNetwork.Instantiate(coinPrefab.name, transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0.5f, 1.5f), UnityEngine.Random.Range(-0.5f, 0.5f)), Quaternion.identity);
        }
    }

    private void ToggleCoinCup(int topPlayerCoins)
    {
        if (coins == topPlayerCoins) coinCup.SetActive(true);
        else coinCup.SetActive(false);
    }

    private void CleanUp()
    {
        SetCoins(startingCoins);
    }
}
