using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerModelChanger : MonoBehaviour
{
    [Header("Photon")]
    public PhotonView photonView;

    [Header("Avaliable Player Models")]
    [SerializeField] private GameObject[] characterModels;
    [SerializeField] private bool[] characterGenders;

    private int characterId = 0;

    private void Start()
    {
        ChangeModel((int)photonView.Owner.CustomProperties["CharacterModel"]);
    }

    private void ChangeModel(int selectedModelId)
    {
        characterId = selectedModelId;
        for (int i = 0; i != characterModels.Length; i++)
        {
            if (i == selectedModelId) characterModels[i].SetActive(true);
            else characterModels[i].SetActive(false);
        }
    }

    public int GetCharactedId()
    {
        return characterId;
    }

    public bool GetCharactedGender()
    {
        return characterGenders[characterId];
    }
}
