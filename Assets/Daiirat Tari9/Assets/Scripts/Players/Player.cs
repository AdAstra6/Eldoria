using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private List<Transform> waypoints; // No need to assign manually
    public float moveSpeed = 5f;
    private bool isMoving = false;
    public int currentTileIndex = 0;

    private void Start()
    {
        // Automatically get waypoints from MapManager
        waypoints = new List<Transform>(FindObjectOfType<MapManager>().waypoints);
    }

    public void Move(int steps)
    {
        if (isMoving || waypoints == null || waypoints.Count == 0) return;
        StartCoroutine(MoveAlongPath(steps));
    }

    private IEnumerator MoveAlongPath(int steps)
    {
        isMoving = true;
        for (int i = 0; i < steps; i++)
        {
            if (currentTileIndex + 1 < waypoints.Count)
            {
                currentTileIndex++;
                Vector3 targetPosition = waypoints[currentTileIndex].position;
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        isMoving = false;
        HandleTileEffect();
    }

    private void HandleTileEffect()
    {
        Tile currentTile = waypoints[currentTileIndex].GetComponent<Tile>();
        if (currentTile != null)
        {
            Debug.Log("Landed on: " + currentTile.name);
        }
        GameManager.Instance.EndTurn();
    }
}
