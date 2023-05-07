using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CharacterSelectMenu : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_Dropdown characterModelInputField;
    [Header("Visuals")]
    [SerializeField] private GameObject charactersDisplay;

    private const string PlayerNameKey = "playerName";
    private const string CharacterModelKey = "characterModel";

    private void OnEnable()
    {
        LoadCharacterDetails();
        charactersDisplay.SetActive(true);
    }

    private void OnDisable()
    {
        SaveCharacterDetails();
        if(charactersDisplay != null) charactersDisplay.SetActive(false);
    }

    public void OnClickConfirm()
    {
        PhotonNetwork.NickName = playerNameInputField.text;
        Hashtable playerCharacterHashtable = new Hashtable();
        playerCharacterHashtable.Add("CharacterModel", characterModelInputField.value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCharacterHashtable);
    }

    public void SaveCharacterDetails()
    {
        string playerName = playerNameInputField.text;
        int characterModel = characterModelInputField.value;

        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.SetInt(CharacterModelKey, characterModel);
    }

    private void LoadCharacterDetails()
    {
        string playerName = PlayerPrefs.GetString(PlayerNameKey, "Cowboy1");
        int characterModel = PlayerPrefs.GetInt(CharacterModelKey, 0);

        playerNameInputField.text = playerName;
        characterModelInputField.value = characterModel;
    }
}
