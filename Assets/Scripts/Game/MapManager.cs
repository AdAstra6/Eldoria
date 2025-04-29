using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Container")]
    public Transform mapContainer; // Assign "Map" here

    [Header("Tile Configuration")]
    public List<Tile> tiles = new List<Tile>();

    public int questionFrequency = 2;
    public int penaltyFrequency = 5;
    public int puzzleFrequency = 3;

    void Awake()
    {
        InitializeTiles();
    }

    void InitializeTiles()
    {
        tiles.Clear();

        Tile[] tiless =mapContainer.GetComponentsInChildren<Tile>();
        foreach (Tile tile in tiless)
        {
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }
        Debug.Log($"TOTAL NUMBER OF TILES: {tiles.Count}");
        for (int i = 0; i < tiles.Count; i++)
        {
            Tile currentTile = tiles[i];
            if (currentTile.type == TileType.TELEPORT || currentTile.type == TileType.FINISH || currentTile.isSpawnPoint )
                continue; // Skip teleport and finish tiles and spawn points
            if (i % penaltyFrequency == 0)
                currentTile.type = TileType.PENALTY;
            else if (i % puzzleFrequency == 0)
                currentTile.type = TileType.PUZZLE;
            else if (i % questionFrequency == 0)
                currentTile.type = TileType.PUZZLE;
            else
                currentTile.type = TileType.MCQUESTION;
        }

        Debug.Log($"Dispatched {tiles.Count} tiles across the map.");
    }
}
