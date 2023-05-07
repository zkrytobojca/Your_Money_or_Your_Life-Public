using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpToSpawn : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerModel;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void Teleport(Vector3 newPosition)
    {
        if(photonView == null) photonView = GetComponent<PhotonView>();
        photonView.RPC("RPC_Teleport", RpcTarget.AllViaServer, newPosition);
        Debug.Log(photonView.ViewID);
    }

    [PunRPC]
    public void RPC_Teleport(Vector3 newPosition)
    {
        transform.position = newPosition;
        playerModel.position = newPosition;
    }
}
