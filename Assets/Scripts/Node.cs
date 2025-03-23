using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node : MonoBehaviour
{
    int type;// 0:grass , 1:
    private Vector2 cords;
    public Vector2 GetPosition() { return this.cords; }
    public void SetPosition(float x , float y) { this.cords.x = x; this.cords.y = y; }

    [SerializeField] List<Node> next = new List<Node>();

    private void Awake()
    {
        this.cords.x = transform.position.x;
        this.cords.y = transform.position.y;
        transform.name = $"{this.cords.x},{this.cords.y}";
    }




}
