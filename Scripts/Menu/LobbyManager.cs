using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Menu List")]
    [SerializeField] private GameObject createRoomMenu;
    [SerializeField] private GameObject activeRoomMenu;
    [SerializeField] private GameObject roomListMenu;
    [Header("Room Settings")]
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text roomNameWarning;
    [SerializeField] private TMP_InputField maxPlayersInputField;
    [SerializeField] private TMP_Text maxPlayersWarning;
    [SerializeField] private int maxPlayers = 99;
    [SerializeField] private TMP_Text roomNameLabel;
    [SerializeField] private TMP_Dropdown roundTimeInputField;
    [Header("Room List")]
    [SerializeField] private RoomItem roomItemPrefab;
    Dictionary<string, RoomItem> roomItemsList = new Dictionary<string, RoomItem>();
    [SerializeField] private Transform roomListContent;

    bool _firstUpdate = true;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        int maxPlayerCount = -1;
        Int32.TryParse(maxPlayersInputField.text, out maxPlayerCount);

        if(roomNameInputField.text.Length <= 0)
        {
            roomNameWarning.gameObject.SetActive(true);
            roomNameWarning.text = "Room name empty!";
            return;
        }
        if (maxPlayerCount <= 0 || maxPlayerCount > maxPlayers)
        {
            maxPlayersWarning.gameObject.SetActive(true);
            maxPlayersWarning.text = "Max number is " + maxPlayers;
            return;
        }

        Hashtable customProperties = new Hashtable();
        switch(roundTimeInputField.value)
        {
            case 0:
                customProperties.Add("RoundTime", 300f);
                break;
            case 1:
                customProperties.Add("RoundTime", 600f);
                break;
            case 2:
                customProperties.Add("RoundTime", 900f);
                break;
            case 3:
                customProperties.Add("RoundTime", 1800f);
                break;
        }
        customProperties.Add("FinishTime", 15f);
        string[] customRoomPropertiesForLobby = new string[2] { "RoundTime", "FinishTime" };
        bool roomCreated = PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = (byte)maxPlayerCount, PublishUserId = true, CustomRoomPropertiesForLobby = customRoomPropertiesForLobby, CustomRoomProperties = customProperties } );
        if(!roomCreated)
        {
            roomNameWarning.gameObject.SetActive(true);
            roomNameWarning.text = "Room name unavailable!";
        }
        else
        {
            roomNameWarning.gameObject.SetActive(false);
            maxPlayersWarning.gameObject.SetActive(false);
        }
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomNameWarning.gameObject.SetActive(true);
        roomNameWarning.text = "Room name unavailable!";
    }

    public override void OnJoinedRoom()
    {
        roomNameLabel.text = "ROOM: " + PhotonNetwork.CurrentRoom.Name;
        createRoomMenu.SetActive(false);
        roomListMenu.SetActive(false);
        activeRoomMenu.SetActive(false);

        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }
    
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo entry in roomList)
        {
            if (roomItemsList.ContainsKey(entry.Name))
            {
                if (entry.RemovedFromList)
                {
                    // we delete the cell
                    roomItemsList[entry.Name].RemoveFromList();
                    roomItemsList.Remove(entry.Name);
                }
                else
                {
                    // we update the cell
                    roomItemsList[entry.Name].RefreshInfo(entry);
                }

            }
            else
            {
                if (!entry.RemovedFromList)
                {
                    // we create the cell
                    roomItemsList[entry.Name] = Instantiate(roomItemPrefab, roomListContent);
                    roomItemsList[entry.Name].gameObject.SetActive(true);
                    roomItemsList[entry.Name].AddToList(entry, !_firstUpdate);
                }
            }
        }

        _firstUpdate = false;
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ResetList();
    }

    public override void OnLeftRoom()
    {
        createRoomMenu.SetActive(false);
        roomListMenu.SetActive(true);
        activeRoomMenu.SetActive(false);
    }

    public void ResetList()
    {
        _firstUpdate = true;

        foreach (KeyValuePair<string, RoomItem> entry in roomItemsList)
        {

            if (entry.Value != null)
            {
                Destroy(entry.Value.gameObject);
            }

        }
        roomItemsList = new Dictionary<string, RoomItem>();
    }
}
