using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int spawnpointsCount=4;
    [SerializeField] private List<Tile> spawnPoints;
    private void Start()
    {
        spawnpointsCount = spawnPoints.Count;
        Debug.Log($"number points spawn :{spawnpointsCount}");
    }

    public List<Tile> getRandomSpawnpoints(int n)
    {
        if (n > spawnpointsCount)
        {
            Debug.LogError("Not enough spawnpoints!");
            return null;
        }

        List<Tile> result = new List<Tile>();
        int index = Random.Range(0, spawnpointsCount);
        int step = spawnpointsCount / n;
        for (int i = 0; i < n; i++)
        {
            Tile tile = spawnPoints[index];
            tile.isSpawnPoint = true; // ✅ Mark as spawn tile
            result.Add(tile);
            index = (index + Random.Range(1, step)) % this.spawnpointsCount;
        }
        return result;
    }
}
