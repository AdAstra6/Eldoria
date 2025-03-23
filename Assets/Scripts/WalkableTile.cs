using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkableTile : MonoBehaviour
{
    private Vector2 cords;
    public Vector2 Cords
    {
        get { return cords; }
    }

    private void Awake()
    {
        this.cords.x = transform.position.x;
        this.cords.y = transform.position.y;
        transform.name = $"{this.cords.x},{this.cords.y}";
    }

}
