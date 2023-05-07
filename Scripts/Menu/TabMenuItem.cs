using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabMenuItem : MonoBehaviour
{
    [Header("Text Displayed")]
    [SerializeField] private TMP_Text playerPositionTextField;
    [SerializeField] private TMP_Text playerNameTextField;
    [SerializeField] private TMP_Text playerCoinsTextField;

    public void RefreshInfo(Player newPlayer, int position)
    {
        playerPositionTextField.text = "#" + (position + 1).ToString();
        playerNameTextField.text = newPlayer.NickName;
        if(newPlayer.CustomProperties.ContainsKey("Coins"))
        {
            playerCoinsTextField.text = ((int)newPlayer.CustomProperties["Coins"]).ToString();
        }
    }

    public void AddToList(Player newPlayerData, int position)
    {
        RefreshInfo(newPlayerData, position);
    }

    public void RemoveFromList()
    {
        Destroy(this.gameObject);
    }
}
