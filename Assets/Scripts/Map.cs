using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] RoadNetwork roadNetwork;
    [SerializeField] Tilemap roadsTilemap;

    private void Awake()
    {
        Dictionary<Vector2Int, WalkableTile> walkableTiles = roadNetwork.WalkableTiles;
        WalkableTile[] tiles = roadsTilemap.GetComponentsInChildren<WalkableTile>();

        for (int i = 0; i < tiles.Length; i++)
        {
            Vector3Int cellPos = roadsTilemap.WorldToCell(tiles[i].transform.position);
            walkableTiles.Add(new Vector2Int(cellPos.x, cellPos.y), tiles[i]);

        }
        roadNetwork.DisplayTile();
    }
}
