using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public enum TileType { Normal, Question, Bonus, Penalty, Teleport }
    public TileType type;
    
    public List<Tile> nextTiles; // List of possible paths from this tile
    public bool isCrossway = false; // Crossway is handled separately

    public void TriggerEffect(Player player)
    {
        switch (type)
        {
            case TileType.Question:
                Debug.Log(player.name + " landed on a Question tile!");
                // QuestionManager.Instance.AskQuestion(player);
                break;

            case TileType.Bonus:
                Debug.Log(player.name + " landed on a Bonus tile!");
                // player.GainReward();
                GameManager.Instance.EndTurn();
                break;

            case TileType.Penalty:
                Debug.Log(player.name + " landed on a Penalty tile!");
                // player.ReceivePenalty();
                GameManager.Instance.EndTurn();
                break;

            case TileType.Teleport:
                Debug.Log(player.name + " landed on a Teleport tile!");
                // player.TeleportToRandomTile();
                GameManager.Instance.EndTurn();
                break;

            default:
                Debug.Log(player.name + " landed on a Normal tile.");
                GameManager.Instance.EndTurn();
                break;
        }
    }

    public Tile GetNextTile()
    {
        if (isCrossway)
        {
            Debug.Log("Crossway reached! The player must choose a direction.");
            return null; // Stop movement and wait for player choice
        }

        return nextTiles.Count > 0 ? nextTiles[0] : null; // Default movement
    }
}
