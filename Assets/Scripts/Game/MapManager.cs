using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Container")]
    public Transform mapContainer; // Assign your "Map" GameObject here

    [Header("Tile Configuration")]
    public List<Tile> tiles = new List<Tile>();

    public int questionFrequency = 3;
    public int penaltyFrequency = 5;
    //public int puzzleFrequency = 7;

    void Awake()
    {
        InitializeTiles();
    }

    void InitializeTiles()
    {
        tiles.Clear();

        foreach (Transform child in mapContainer)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            Tile currentTile = tiles[i];
            if (currentTile.type == TileType.TELEPORT || currentTile.type == TileType.FINISH || currentTile.isSpawnPoint )
                continue; // Skip teleport and finish tiles and spawn points
            if (i % questionFrequency == 0)
                currentTile.type = TileType.MCQUESTION;
            //else if (i % puzzleFrequency == 0)
                //currentTile.type = TileType.PUZZLE;
            else if (i % penaltyFrequency == 0)
                currentTile.type = TileType.PENALTY;

            else
                currentTile.type = TileType.NORMAL;
        }

        Debug.Log($"Dispatched {tiles.Count} tiles across the map.");
    }
}
