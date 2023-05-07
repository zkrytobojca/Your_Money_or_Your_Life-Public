using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatLog : MonoBehaviourPunCallbacks
{
    [Header("Chat Log Settings")]
    public float showForSec = 3f;
    [Header("Prefabs")]
    [SerializeField] private GameObject chatLogItemPrefab;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(ShowChatLog(newPlayer.NickName, "joined the game."));
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(ShowChatLog(otherPlayer.NickName, "left the game."));
    }

    IEnumerator ShowChatLog(string playerName, string info)
    {
        GameObject chatLogItemGO = Instantiate(chatLogItemPrefab, transform);
        ChatLogItem chatLogItem = chatLogItemGO.GetComponent<ChatLogItem>();
        chatLogItem.AddToList(playerName + " " + info);
        yield return new WaitForSeconds(showForSec);
        chatLogItem.RemoveFromList();
    }
}
