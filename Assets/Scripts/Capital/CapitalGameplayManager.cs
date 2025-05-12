using System.Collections.Generic;
using UnityEngine;

public class CapitalGameplayManager : MonoBehaviour
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("Capital Spawn Points")]
    public List<Transform> spawnPoints; // Assign from Inspector (match player count or more)

    private void Awake()
    {
        // Fallback safety
        if (GameData.CapitalTransitionData == null)
        {
            Debug.LogError("No data found for capital transition!");
            GameData.CapitalTransitionData = new List<PlayerRuntimeData>();
        }
    }

    void Start()
    {
        List<PlayerRuntimeData> playersData = GameData.CapitalTransitionData;

        for (int i = 0; i < GameData.playersCount; i++)
        {
            if (i >= spawnPoints.Count)
            {
                Debug.LogWarning("Not enough spawn points for all players!");
                break;
            }

            PlayerRuntimeData data = playersData[i];
            Transform spawn = spawnPoints[i];

            GameObject playerObj = Instantiate(playerPrefab, spawn.position, Quaternion.identity);
            Player player = playerObj.GetComponent<Player>();

            if (player == null)
            {
                Debug.LogError("Player prefab is missing the Player script!");
                continue;
            }

            // Restore data
            player.Initialize(data.Profile);
            player.SetInitialHealth(data.Health);
            player.Inventory = data.Inventory; 
            player.currentTile = spawn.GetComponent<Tile>();
        }
    }
}
