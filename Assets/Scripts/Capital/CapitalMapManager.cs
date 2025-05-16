using System.Collections.Generic;
using UnityEngine;

public class CapitalMapManager : MonoBehaviour
{
    [Header("Capital Map Container")]
    public Transform capitalContainer; // Assign the capital tile parent object

    private List<Tile> capitalTiles = new List<Tile>();

    void Awake()
    {
        AssignRiddleTiles();
    }

    void AssignRiddleTiles()
    {
        capitalTiles.Clear();

        Tile[] tiles = capitalContainer.GetComponentsInChildren<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                if (tile.type == TileType.CAPITAL_START_TILE || tile.type == TileType.CAPITAL_END_TILE)
                {
                    // Skip capital start and end tiles
                    continue;
                }
                tile.type = TileType.RIDDLE;
                capitalTiles.Add(tile);
            }
        }

        Debug.Log($"Assigned RIDDLE to {capitalTiles.Count} capital tiles.");
    }
}
