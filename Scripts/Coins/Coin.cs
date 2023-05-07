using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Coin : MonoBehaviour, IPunInstantiateMagicCallback
{
    [Header("Coin Settings")]
    [SerializeField] private Vector2 forceMinMax = new Vector2(-75f, 75f);
    private Rigidbody rigidb;
    private PhotonView photonView;
    private int ownerId;
    private bool isPickable = false;
    private float timeToPickable = 1f;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        ownerId = info.Sender.ActorNumber;
    }

    private void Awake()
    {
        rigidb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        Vector3 randomForce = new Vector3(
            Random.Range(forceMinMax.x, forceMinMax.y), 
            Mathf.Abs(Random.Range(forceMinMax.x, forceMinMax.y)), 
            Random.Range(forceMinMax.x, forceMinMax.y)
            );
        rigidb.AddForce(randomForce);
    }

    private void Update()
    {
        if (isPickable == false)
        {
            timeToPickable -= Time.deltaTime;
            if (timeToPickable <= 0f)
            {
                timeToPickable = 0f;
                isPickable = true;
            }
        }
    }

    private void OnEnable()
    {
        GameStateManager.gameReload += CleanUp;
    }

    private void OnDisable()
    {
        GameStateManager.gameReload -= CleanUp;
    }

    private void CleanUp()
    {
        photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCoins playerCoins = other.gameObject.GetComponent<PlayerCoins>();
        if(playerCoins != null)
        {
            if (isPickable)
            {
                playerCoins.PickUpCoins(1);
                photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    private void RPC_DestroySelf()
    {
        AudioManager.instance.PlaySound("Coin_PickUp", transform.position);
        Destroy(gameObject);
    }
}
