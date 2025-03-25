using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// Guembo

public class Player1 : MonoBehaviour
    
{
    [SerializeField] Node currentNode;
    int lives;

    public Player1(Node node, int lives)
    {
        this.lives = lives;
        this.currentNode = node;
    }

}
