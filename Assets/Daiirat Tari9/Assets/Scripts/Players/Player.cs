using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Tile currentTile; // The tile the player is currently on
    public float moveSpeed = 5f;
    private bool isMoving = false;

    public void Move(int steps)
    {
        if (isMoving || currentTile == null) return;
        StartCoroutine(MoveAlongPath(steps));
    }

    private IEnumerator MoveAlongPath(int steps)
    {
        isMoving = true;

        for (int i = 0; i < steps; i++)
        {
            Tile nextTile = currentTile.GetNextTile(); // Get the next tile from the current one

            if (nextTile == null) break; // Stop if there is no next tile

            Vector3 targetPosition = nextTile.transform.position;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            currentTile = nextTile; // Update current tile
        }

        isMoving = false;
        HandleTileEffect();
    }

    private void HandleTileEffect()
    {
        if (currentTile.isCrossway)
        {
            Debug.Log("Crossway reached! The player must choose a direction.");
            // TODO: Show UI for player choice
        }
        else
        {
            Debug.Log("Landed on: " + currentTile.name);
        }

        GameManager.Instance.EndTurn();
    }
}
