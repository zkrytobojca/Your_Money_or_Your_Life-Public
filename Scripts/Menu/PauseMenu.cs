using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        StartCoroutine(DisconnectAldLoadMenu());
    }

    IEnumerator DisconnectAldLoadMenu()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("Menu");
    }
}

