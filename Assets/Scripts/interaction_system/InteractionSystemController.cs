using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystemController : MonoBehaviour
{
    public static InteractionSystemController Instance;
    public const int MAX_PERMITTED_STEPS = 6;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void TriggerTileInteraction(Player player)
    {
        DiceRoll.Instance.hideAfterAnim();
        Tile tile = player.currentTile;
        switch (tile.type)
        {
            case TileType.MCQUESTION:
                Debug.Log(player.name + " landed on a MCQuestion tile!");
                GameQuestionManager.Instance.AskQuestion(player);
                //player.PlayerState = PlayerStats.STRATEGIC_CHOICE;// Temporary state until implement full interaction system TODO: delete this line after test
                break;
            case TileType.PUZZLE:
                Debug.Log(player.name + " landed on a Puzzle tile!");
                player.PlayerState = PlayerStats.DOING_PUZZLE;
                PuzzleGameManager.Instance.StartPuzzleGame();
                break;
            case TileType.PENALTY:
                Debug.Log(player.name + " landed on a Penalty tile!");
                int stepsBack = Random.Range(1, MAX_PERMITTED_STEPS);
                StartCoroutine(TriggerPenaltyInteraction(player, stepsBack));

                player.MoveBackward(stepsBack);
                GameplayManager.Instance.StartStrategicPhase();

                break;
            case TileType.TELEPORT:
                Debug.Log(player.name + " landed on a Teleport tile!");
                // player.TeleportToRandomTile();
                break;
            case TileType.FINISH:
                Debug.Log(player.name + " landed on a Finish tile!");
                GameplayManager.Instance.GameOver(true); // Players won the game
                break;
            default:
                Debug.Log(player.name + " landed on a Normal tile.");
                GameplayManager.Instance.StartStrategicPhase();
                break;
        }
    }

    public IEnumerator TriggerPenaltyInteraction(Player player ,int stepBack)
    {
        // Play the penalty sound
        AudioManager.Instance.PlayPenalty();
        player.PlayerState = PlayerStats.MOVING_BACK;
        Tile tile = player.currentTile;
        UIManager.Instance.ShowPenaltyText(player, stepBack);
        yield return new WaitForSeconds(2f);
        UIManager.Instance.HidePenaltyText();

    }

}
