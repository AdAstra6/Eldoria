using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Tile currentTile;
    private Tile nextTile;
    private bool pathChosen = false;

    public float moveSpeed = 5f;
    private bool isMoving = false;
    private int remainingSteps = 0;
    private PlayerStats playerState;
    public PlayerStats PlayerState
    {  get { return playerState; } set {playerState = value;}}

    public void Move(int steps)
    {
        if (isMoving || currentTile == null) return;
        this.playerState = PlayerStats.MOVING;
        StartCoroutine(MoveAlongPath(steps));
    }

    private IEnumerator MoveAlongPath(int steps)
    {
        isMoving = true;
        remainingSteps = steps;
        Tile nextTile = null;

        while (remainingSteps > 0)
        {
            // Immediately check for crossways at current tile
            if (currentTile.isCrossway)
            {
                Debug.Log("Reached crossway. Waiting for path choice.");
                UIManager.Instance.DisplayPathChoices(currentTile.nextTiles, this);
                isMoving = false;
                yield return new WaitUntil(() => pathChosen); // Pause movement until path is chosen
                pathChosen = false; // Reset the flag for the next crossway
                isMoving = true;
                nextTile = this.nextTile;
            } else { nextTile = currentTile.GetNextTile(); }

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
        this.playerState = PlayerStats.END_MOVING;
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
        //StartCoroutine(CompleteCrosswayMovement(chosenTile));
        nextTile = chosenTile;
        pathChosen = true;
    }

    
}