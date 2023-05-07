using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunRPCCalls : MonoBehaviour
{
    [Header("Outside Objects")]
    [SerializeField] private GunModel gunModel;
    [SerializeField] private string gunFireSoundName = "Revolver_Shoot";

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        Gun.shoot += ShootTP;
    }

    private void OnDisable()
    {
        Gun.shoot -= ShootTP;
    }

    void ShootTP(Vector3 start, Vector3 target)
    {
        if(photonView.IsMine)
        {
            photonView.RPC("RPC_ShootTP", RpcTarget.All, start, target);
        }
    }

    [PunRPC]
    void RPC_ShootTP(Vector3 start, Vector3 target)
    {
        if (!photonView.IsMine)
        {
            gunModel.Shoot();
            gunModel.PlayMuzzleFlash();
            gunModel.SummonBullet(start, target);
            AudioManager.instance.PlaySound(gunFireSoundName, start);
        }
    }
}
