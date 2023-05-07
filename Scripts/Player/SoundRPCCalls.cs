using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRPCCalls : MonoBehaviour
{
    [Header("Outside Objects")]
    [SerializeField] private PlayerCoins playerCoins;
    [SerializeField] private PlayerModelChanger playerModelChanger;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        playerCoins.tookDamage += TookDamage;
        Gun.shoot += Shoot;
        Gun.reload += Reload;
        StarterAssets.FirstPersonController.move += Move;
    }

    private void OnDisable()
    {
        playerCoins.tookDamage -= TookDamage;
        Gun.shoot -= Shoot;
        Gun.reload -= Reload;
        StarterAssets.FirstPersonController.move -= Move;
    }

    void TookDamage()
    {
        if (playerModelChanger.GetCharactedGender()) AudioManager.instance.PlayRandomSound("Hurt_Woman");
        else AudioManager.instance.PlayRandomSound("Hurt_Man");
        photonView.RPC("RPC_TookDamage", RpcTarget.Others);
    }

    [PunRPC]
    void RPC_TookDamage()
    {
        if (playerModelChanger.GetCharactedGender()) AudioManager.instance.PlayRandomSound("Hurt_Woman", playerCoins.transform.position);
        else AudioManager.instance.PlayRandomSound("Hurt_Man", playerCoins.transform.position);
    }

    void Shoot(Vector3 position, Vector3 target)
    {
        photonView.RPC("RPC_Shoot", RpcTarget.AllViaServer, position);
    }

    [PunRPC]
    void RPC_Shoot(Vector3 position)
    {
        AudioManager.instance.PlaySound("Revolver_Shoot", position);
    }

    void Reload(Vector3 position)
    {
        AudioManager.instance.PlaySound("Revolver_Reload");
        photonView.RPC("RPC_Reload", RpcTarget.Others, position);
    }

    [PunRPC]
    void RPC_Reload(Vector3 position)
    {
        AudioManager.instance.PlaySound("Revolver_Reload", position);
    }

    void Move(Vector3 position)
    {
        AudioManager.instance.PlayRandomSound("Footstep");
        photonView.RPC("RPC_Move", RpcTarget.Others, position);
    }

    [PunRPC]
    void RPC_Move(Vector3 position)
    {
        AudioManager.instance.PlayRandomSound("Footstep", position);
    }
}
