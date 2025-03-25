using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// Guembo

public class Player1 : MonoBehaviour
    
{
    WalkableTile currentTile;
    int lives;

    public Player1(WalkableTile tile, int lives)
    {
        this.currentTile = tile;
        this.lives = lives;
    }

}
