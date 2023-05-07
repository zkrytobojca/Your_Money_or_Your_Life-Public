using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInputField;
    public TMP_InputField joinInputField;
    public TMP_InputField playerNameInputField;
    public TMP_InputField characterModelInputField;

    public void CreateRoom()
    {
        SetPlayerData();
        PhotonNetwork.CreateRoom(createInputField.text);
    }

    public void JoinRoom()
    {
        SetPlayerData();
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void SetPlayerData()
    {
        PhotonNetwork.NickName = playerNameInputField.text;
        Hashtable playerCharacterHashtable = new Hashtable();
        int characterModelId = 0;
        Int32.TryParse(characterModelInputField.text, out characterModelId);
        playerCharacterHashtable.Add("CharacterModel", characterModelId);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCharacterHashtable);
    }
}
