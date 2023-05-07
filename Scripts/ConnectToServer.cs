using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{ 
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private MenuCameraMovement cameraMovement;

    private void Start()
    {
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        if(loadingScreen.activeInHierarchy)
        {
            loadingScreen.SetActive(false);
            mainMenu.SetActive(true);
            cameraMovement.ChangeState(1);
        }
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
