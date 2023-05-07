using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatLogItem : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text chatTextField;

    public void RefreshInfo(string text)
    {
        chatTextField.text = text;
    }

    public void AddToList(string text)
    {
        RefreshInfo(text);
    }

    public void RemoveFromList()
    {
        Destroy(this.gameObject);
    }
}
