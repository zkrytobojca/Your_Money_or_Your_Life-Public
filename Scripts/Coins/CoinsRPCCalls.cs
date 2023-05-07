using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CoinsRPCCalls : MonoBehaviour
{
    [Header("Outside Objects")]
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private TMP_Text aboveHeadDisplay;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        playerCoins.updateCoinsRPC += UpdateCoins;
    }

    private void OnDisable()
    {
        playerCoins.updateCoinsRPC -= UpdateCoins;
    }

    void UpdateCoins(int amount)
    {
        photonView.RPC("RPC_UpdateCoins", RpcTarget.AllBuffered, amount);
    }

    [PunRPC]
    void RPC_UpdateCoins(int amount)
    {
        playerCoins.SetCoins(amount, false);
        aboveHeadDisplay.text = (amount > 0 ? amount : 1).ToString();
    }
}
