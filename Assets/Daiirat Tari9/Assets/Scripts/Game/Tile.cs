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
            // Temporary: Choose the first path automatically
            return nextTiles[0]; // You will later replace this with player choice
        }
        return nextTiles[0]; // Default single path
    }
}
