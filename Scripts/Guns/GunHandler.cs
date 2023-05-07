using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunHandlerMain : MonoBehaviour
{
    [Header("Gun Handlers")]
    [SerializeField] private GunHandlerMain gunHandlerFP;
    [SerializeField] private GunHandlerMain gunHandlerTP;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;


    }
}
