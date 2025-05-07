using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public const int spawnpointsCount=10;
    public const int BIOMS_COUNT = 5;
    [SerializeField] private List<Tile> spawnPoints;
    private void Start()
    {
        //spawnpointsCount = spawnPoints.Count;
        Debug.Log($"number points spawn :{spawnpointsCount}");
    }

    public List<Tile> getRandomSpawnpoints(int n)
    {
        if (n > spawnpointsCount)
        {
            Debug.LogError("Not enough spawnpoints!");
            return null;
        }

        List<int> availableBioms = new List<int>();
        for (int i = 0; i < BIOMS_COUNT; i++) availableBioms.Add(i);
        // Shuffle Bioms
        for (int i = 0; i < availableBioms.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, availableBioms.Count);
            int temp = availableBioms[i];
            availableBioms[i] = availableBioms[randIndex];
            availableBioms[randIndex] = temp;
        }

        List<Tile> result = new List<Tile>();

        for (int i = 0; i < n; i++)
        {
            int biomIndex;
            if (i < availableBioms.Count) biomIndex = availableBioms[i];
            else biomIndex = UnityEngine.Random.Range(0, BIOMS_COUNT); // if there is no empty biom just take a random one
            int spawnIndex = biomIndex * 2 + UnityEngine.Random.Range(0, 2);
            if (result.Contains(spawnPoints[spawnIndex]))
            {
                if (spawnIndex % 2 == 0) spawnIndex ++;
                else spawnIndex--;
            }
            result.Add(spawnPoints[spawnIndex]);
        }
        
        return result;
    }
}
