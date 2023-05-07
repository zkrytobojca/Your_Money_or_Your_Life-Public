using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TabMenuController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private TabMenuItem tabMenuItemPrefab;
    [Header("List")]
    List<TabMenuItem> tabMenuItemsList = new List<TabMenuItem>();
    [SerializeField] private Transform tabMenuListContent;

    private void OnEnable()
    {
        RankingManager.instance.updateRanking += UpdatePlayerList;
    }

    private void OnDisable()
    {
        RankingManager.instance.updateRanking -= UpdatePlayerList;
    }

    private void UpdatePlayerList()
    {
        foreach(TabMenuItem tabMenuItem in tabMenuItemsList)
        {
            Destroy(tabMenuItem.gameObject);
        }
        tabMenuItemsList.Clear();

        List<Player> players = RankingManager.instance.GetPlayers();

        for(int i=0; i!=players.Count; i++)
        {
            TabMenuItem newTabMenuItem = Instantiate(tabMenuItemPrefab, tabMenuListContent);
            newTabMenuItem.AddToList(players[i], i);
            tabMenuItemsList.Add(newTabMenuItem);
        }
    }
}
