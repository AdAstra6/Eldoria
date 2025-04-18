using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    public Tile currentTile;
    private Tile nextTile;
    private bool pathChosen = false;

    public float moveSpeed = 5f;
    private bool isMoving = false;
    private int remainingSteps = 0;
    private PlayerStats playerState;

    private Stack<Tile> pathcrossedPath = new Stack<Tile>();

    public PlayerProfile profileData;

    private TMP_Text nameLabel;  // << Added for player name display

    public void Initialize(PlayerProfile profile)
    {
        this.profileData = profile;

        // Find the TextMeshProUGUI in the children
        if (nameLabel == null)
        {
            nameLabel = GetComponentInChildren<TMP_Text>();
        }

        if (nameLabel != null)
        {
            nameLabel.text = profile.Name;
        }
    }

    public PlayerStats PlayerState
    {
        get { return playerState; }
        set { playerState = value; }
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void Move(int steps)
    {
        if (isMoving || currentTile == null) return;
        this.playerState = PlayerStats.MOVING;
        StartCoroutine(MoveAlongPath(steps));
    }

    public void MoveBackward(int steps)
    {
        if (isMoving || currentTile == null) return;
        this.playerState = PlayerStats.MOVING_BACK;
        StartCoroutine(MoveBackwardCoroutine(steps));
    }

    private IEnumerator MoveBackwardCoroutine(int steps)
    {
        isMoving = true;

        while (steps > 0 && pathcrossedPath.Count > 0)
        {
            Tile lastTile = GetLastPathCrossed();
            yield return StartCoroutine(MoveToTile(lastTile.transform.position));
            currentTile = lastTile;
            steps--;
        }

        isMoving = false;
        this.playerState = PlayerStats.END_MOVING;
        GameManager.Instance.EndTurn();
    }

    private IEnumerator MoveAlongPath(int steps)
    {
        isMoving = true;
        remainingSteps = steps;
        Tile nextTile = null;

        while (remainingSteps > 0)
        {
            if (currentTile.isCrossway)
            {
                Debug.Log("Reached crossway. Waiting for path choice.");
                UIManager.Instance.DisplayPathChoices(currentTile.nextTiles, this);
                isMoving = false;
                yield return new WaitUntil(() => pathChosen);
                pathChosen = false;
                isMoving = true;
                nextTile = this.nextTile;
            }
            else
            {
                nextTile = currentTile.GetNextTile();
            }

            if (nextTile == null)
            {
                Debug.Log("Path blocked. Stopping movement.");
                break;
            }

            yield return StartCoroutine(MoveToTile(nextTile.transform.position));
            this.SavePathCrossed(currentTile);
            currentTile = nextTile;
            remainingSteps--;
        }

        isMoving = false;
        this.playerState = PlayerStats.END_MOVING;
        if (remainingSteps <= 0) GameManager.Instance.EndTurn();
    }

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

        nextTile = chosenTile;
        pathChosen = true;
    }

    public void SavePathCrossed(Tile tile)
    {
        pathcrossedPath.Push(tile);
    }

    public Tile GetLastPathCrossed()
    {
        if (pathcrossedPath.Count > 0)
        {
            Tile lastTile = pathcrossedPath.Pop();
            Debug.Log("Last tile crossed: " + lastTile.name);
            return lastTile;
        }
        return null;
    }
}
