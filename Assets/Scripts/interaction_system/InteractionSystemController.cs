using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                // Save player runtime data
                GameData.CapitalTransitionData.Clear();
                foreach (Player p in GameplayManager.Instance.Players)
                {
                    GameData.CapitalTransitionData.Add(new PlayerRuntimeData(p));
                }

                // Load capital scene
                SceneManager.LoadScene("Capital");
                break;
            case TileType.RIDDLE:
                Debug.Log(player.name + " landed on a Riddle tile!");
                GameplayManager.Instance.QuestionStarted();
                QuestionUI.Instance.isMotherTree = false;
                GameQuestionManager.Instance.AskRiddle(player);
                break;
            case TileType.EVENT: // Event tiles are managed by the EventManager
            default:
                Debug.Log(" landed on an Event tile! OR");
                Debug.Log(player.name + " landed on a Normal tile.");
                GameplayManager.Instance.StartStrategicPhase();
                break;
        }
    }

    public IEnumerator TriggerPenaltyInteraction(Player player, int stepBack)
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
