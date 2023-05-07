using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawn Area Radius")]
    [SerializeField] private Vector2 minMaxX;
    [SerializeField] private Vector2 minMaxZ;

    public GameObject SpawnPlayer(string prefabName)
    {
        Vector3 randomOffset = new Vector3(Random.Range(minMaxX.x, minMaxX.y), 0, Random.Range(minMaxZ.x, minMaxZ.y));
        return PhotonNetwork.Instantiate(prefabName, transform.position + randomOffset, Quaternion.identity);
    }

    public Vector3 GetRespawnPosition()
    {
        Vector3 randomOffset = new Vector3(Random.Range(minMaxX.x, minMaxX.y), 0, Random.Range(minMaxZ.x, minMaxZ.y));
        return transform.position + randomOffset;
    }
}
