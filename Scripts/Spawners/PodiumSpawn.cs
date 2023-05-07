using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumSpawn : MonoBehaviour
{
    [Header("Player Spawners")]
    [SerializeField] private PlayerSpawner firstPlaceSpawner;
    [SerializeField] private PlayerSpawner secondPlaceSpawner;
    [SerializeField] private PlayerSpawner thirdPlaceSpawner;
    [SerializeField] private PlayerSpawner viewerSpawner;

    public PlayerSpawner GetSpawner()
    {
        List<string> topPlayers = RankingManager.instance.GetTop3Players();
        int placeId = topPlayers.FindIndex(x => x == PhotonNetwork.LocalPlayer.UserId);

        switch(placeId)
        {
            case -1:
                return viewerSpawner;
            case 0:
                return firstPlaceSpawner;
            case 1:
                return secondPlaceSpawner;
            case 2:
                return thirdPlaceSpawner;

            default:
                return viewerSpawner;
        }
    }
}
