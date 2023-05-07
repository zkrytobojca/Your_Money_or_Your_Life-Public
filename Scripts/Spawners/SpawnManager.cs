using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Objects")]
    public GameObject playerPrefab;

    [Header("End Game Podium Spawn")]
    public PodiumSpawn podiumSpawner;

    private GameObject playerReference = null;
    private List<PlayerSpawner> spawnerList = null;

    [HideInInspector]
    public static SpawnManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }

        PlayerSpawner[] spawners = GetComponentsInChildren<PlayerSpawner>();
        spawnerList = new List<PlayerSpawner>(spawners);
    }

    private void OnEnable()
    {
        GameStateManager.gameStart += SpawnPlayer;
        GameStateManager.gameFinish += TeleportPlayerToPodium;
    }

    private void OnDisable()
    {
        GameStateManager.gameStart -= SpawnPlayer;
        GameStateManager.gameFinish -= TeleportPlayerToPodium;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (spawnerList.Count <= 0)
        {
            Debug.LogWarning("No spawners detected!");
            return;
        }
        if (playerReference == null)
        {
            playerReference = spawnerList[Random.Range(0, spawnerList.Count)].SpawnPlayer(playerPrefab.name);
        }
        else
        {
            playerReference.GetComponent<TpToSpawn>().Teleport(spawnerList[Random.Range(0, spawnerList.Count)].GetRespawnPosition());
            AudioManager.instance.PlaySound("Respawn");
        }
    }

    public void TeleportPlayerToPodium()
    {
        playerReference.GetComponent<TpToSpawn>().Teleport(podiumSpawner.GetSpawner().GetRespawnPosition());
        AudioManager.instance.PlaySound("Respawn");
    }
}
