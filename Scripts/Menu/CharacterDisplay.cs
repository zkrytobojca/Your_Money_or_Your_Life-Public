using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    [Header("Avaliable Player Models")]
    [SerializeField] private GameObject[] characters;

    public void ChangeCharacter(int selectedCharacterId)
    {
        for (int i = 0; i != characters.Length; i++)
        {
            if (i == selectedCharacterId) characters[i].SetActive(true);
            else characters[i].SetActive(false);
        }
    }
}
