using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RankingManager : MonoBehaviourPunCallbacks
{
    private List<Player> players = new List<Player>();
    private IDictionary<string, int> leftDict = new Dictionary<string, int>();

    public event Action updateRanking;
    public event Action<int> updateTopPlayer;

    [HideInInspector]
    public static RankingManager instance;

    void Awake()
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
        ResetPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players.Add(newPlayer);
        SortPlayersByCoins();

        List<string> keyList = new List<string>(leftDict.Keys);
        Debug.Log(keyList.ToArray());
        Debug.Log("Joined: " + newPlayer.UserId);

        updateRanking?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player thisPlayer)
    {
        if (leftDict.ContainsKey(thisPlayer.UserId))
        {
            leftDict[thisPlayer.UserId] += 1;
        }
        else
        {
            leftDict.Add(thisPlayer.UserId, 1);
        }

        //int idToRemove = players.FindIndex(new PlayerSearch(newPlayer.UserId).SameId);
        Player playerToRemove = players.Find(x => x.ActorNumber == thisPlayer.ActorNumber);

        if(playerToRemove != null)
        {
            players.Remove(playerToRemove);
            SortPlayersByCoins();
        }

        updateRanking?.Invoke();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("Coins")) return;

        //int idToUpdate = players.FindIndex(new PlayerSearch(targetPlayer.UserId).SameId);
        Player playerToUpdate = players.Find(x => x.ActorNumber == targetPlayer.ActorNumber);
        if (playerToUpdate != null)
        {
            players.Remove(playerToUpdate);
            players.Add(targetPlayer);
            SortPlayersByCoins();
        }

        updateRanking?.Invoke();
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

    public void ResetPlayerList()
    {
        players.Clear();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            players.Add(player.Value);
        }
        SortPlayersByCoins();
        updateRanking?.Invoke();
    }

    public bool CheckIfPlayerRejoined(string userId)
    {
        return leftDict.ContainsKey(userId);
    }

    public List<string> GetTop3Players()
    {
        List<string> topPlayers = new List<string>();
        if (players.Count > 0) topPlayers.Add(players[0].UserId);
        if (players.Count > 1) topPlayers.Add(players[1].UserId);
        if (players.Count > 2) topPlayers.Add(players[2].UserId);
        return topPlayers;
    }

    private void SortPlayersByCoins(bool desc = true)
    {
        players.Sort(CompareTwoPlayersByCoins);
        if (desc) players.Reverse();
        updateTopPlayer?.Invoke(GetTopPlayerCoins());
    }

    private int GetTopPlayerCoins()
    {
        if (players.Count == 0) return -1;
        else return (int)players[0].CustomProperties["Coins"];
    }

    private int CompareTwoPlayersByCoins(Player a, Player b)
    {
        if(!a.CustomProperties.ContainsKey("Coins"))
        {
            if (!b.CustomProperties.ContainsKey("Coins")) return b.NickName.CompareTo(a.NickName);
            else return -1;
        }
        else
        {
            if (!b.CustomProperties.ContainsKey("Coins")) return 1;
            else
            {
                int coinsA = (int)a.CustomProperties["Coins"];
                int coinsB = (int)b.CustomProperties["Coins"];

                if (coinsA > coinsB) return 1;
                else if (coinsA == coinsB) return b.NickName.CompareTo(a.NickName);
                else return -1;
            }
        }
    }

    private class PlayerSearch
    {
        private string idToCompare;

        public PlayerSearch(string id)
        {
            idToCompare = id;
        }

        public bool SameId(Player p)
        {
            return p.UserId == idToCompare;
        }
    }
}
