using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [Header("Object Info")]
    [SerializeField] private int health = 20;
    [Header("Object Destroy")]
    [SerializeField] private GameObject leftOversPrefab;

    private PhotonView photonView;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void TakeDamage(int amount, RaycastHit hit)
    {
        health -= amount;
        if(health <= 0f)
        {
            Destruct();
        }
    }

    public void Destruct()
    {
        if(leftOversPrefab != null)
        {
            Instantiate(leftOversPrefab, transform.position, Quaternion.identity);
        }
        PhotonNetwork.Destroy(gameObject);
    }
}
