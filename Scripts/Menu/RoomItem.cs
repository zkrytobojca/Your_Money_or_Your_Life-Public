using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    [Header("Room Item Elements")]
    [SerializeField] private TMP_Text roomNameTextField;
    [SerializeField] private TMP_Text playerCountTextField;
    [SerializeField] private Button joinButton;

    private string roomName;
    private LobbyManager lobbyManager;

    public RoomInfo info;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void RefreshInfo(RoomInfo info)
    {
        roomNameTextField.text = "Name: " + info.Name;
        roomName = info.Name;

        playerCountTextField.text = "Players: " + info.PlayerCount + "/" + info.MaxPlayers;
        if (info.PlayerCount >= info.MaxPlayers) joinButton.interactable = false;
        else joinButton.interactable = true;
    }

    public void OnClickJoin()
    {
        lobbyManager.JoinRoom(roomName);
    }

    public void AddToList(RoomInfo info, bool animate = false)
    {
        RefreshInfo(info);
    }

    public void RemoveFromList()
    {
        Destroy(this.gameObject);
    }
}
