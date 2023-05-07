using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoinCup : MonoBehaviour
{
    [Header("Photon")]
    [SerializeField] private PhotonView photonView;
    [Header("Visuals")]
    [SerializeField] private GameObject model;

    private void OnEnable()
    {
        RankingManager.instance.updateTopPlayer += ToggleCoinCup;
    }

    private void OnDisable()
    {
        RankingManager.instance.updateTopPlayer -= ToggleCoinCup;
    }

    private void ToggleCoinCup(int topPlayerActorNumber)
    {
        if (photonView.OwnerActorNr == topPlayerActorNumber) model.SetActive(true);
        else model.SetActive(false);
    }
}
