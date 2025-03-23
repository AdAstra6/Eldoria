using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    private Dictionary<Vector2Int, WalkableTile> walkableTiles = new Dictionary<Vector2Int, WalkableTile>();

    public Dictionary<Vector2Int, WalkableTile> WalkableTiles
    {
        get { return walkableTiles; }
    }

    public void DisplayTile()
    {
        foreach (var tile in walkableTiles)
        {
            Debug.Log($"Tile :{tile.Key.x},{tile.Key.y}");
        }
    }
}

