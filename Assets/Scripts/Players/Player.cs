using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Tile currentTile;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private int remainingSteps = 0;

    public void Move(int steps)
    {
        if (isMoving || currentTile == null) return;
        StartCoroutine(MoveAlongPath(steps));
    }

    private IEnumerator MoveAlongPath(int steps)
    {
        isMoving = true;
        remainingSteps = steps;

        while (remainingSteps > 0)
        {
            // Immediately check for crossways at current tile
            if (currentTile.isCrossway)
            {
                Debug.Log("Reached crossway. Waiting for path choice.");
                UIManager.Instance.DisplayPathChoices(currentTile.nextTiles, this);
                isMoving = false;
                yield break; // Pause movement until path is chosen
            }

            Tile nextTile = currentTile.GetNextTile();

            if (nextTile == null)
            {
                Debug.Log("Path blocked. Stopping movement.");
                break;
            }

            // Move to next tile
            yield return StartCoroutine(MoveToTile(nextTile.transform.position));
            currentTile = nextTile;
            remainingSteps--;
        }

        isMoving = false;
        if (remainingSteps <= 0) GameManager.Instance.EndTurn();
    }

    // New helper function for movement between tiles
    private IEnumerator MoveToTile(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    public void ChoosePath(Tile chosenTile)
    {
        if (chosenTile == null) return;

        // Start movement to chosen path first
        StartCoroutine(CompleteCrosswayMovement(chosenTile));
    }

    private IEnumerator CompleteCrosswayMovement(Tile chosenTile)
    {
        isMoving = true;

        // 1. Move to the chosen tile
        yield return StartCoroutine(MoveToTile(chosenTile.transform.position));
        
        // 2. Update current tile AFTER movement
        currentTile = chosenTile;
        
        // 3. Consume the step
        remainingSteps--;

        isMoving = false;

        // 4. Continue movement if steps remain
        if (remainingSteps > 0)
        {
            StartCoroutine(MoveAlongPath(remainingSteps));
        }
        else
        {
            GameManager.Instance.EndTurn();
        }
    }
}