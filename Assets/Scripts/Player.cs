using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// Guembo

public class Player : MonoBehaviour
    
{
    WalkableTile currentTile;
    int lives;

    public Player(WalkableTile tile, int lives)
    {
        this.currentTile = tile;
        this.lives = lives;
    }

}
