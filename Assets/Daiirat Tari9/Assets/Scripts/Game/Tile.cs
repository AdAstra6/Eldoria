using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public List<Tile> nextTiles; // List of possible paths from this tile
    public bool isCrossway = false; // Mark if this is a crossway

    public Tile GetNextTile()
    {
        if (isCrossway)
        {
            Debug.Log("Crossway reached! The player must choose a direction.");
            return null; // Stop movement and wait for player choice
        }

        return nextTiles.Count > 0 ? nextTiles[0] : null; // Default movement
    }
}
