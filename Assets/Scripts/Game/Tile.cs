using UnityEngine;
using System.Collections.Generic;


public class Tile : MonoBehaviour
{
//    public enum TileType { NORMAL, MCQUESTION, PUZZLE, PENALTY, TELEPORT }
    public TileType type;
    
    public List<Tile> nextTiles; // List of possible paths from this tile
    public bool isCrossway = false; // Crossway is handled separately

    public bool isSpawnPoint = false; // Spawn point is handled separately
    
    /// ///////////////////////////////////////////////////////////////////////////////
    
   [SerializeField] public bool isEventTile = false; // events are handled seperately 

   [SerializeField] private EventType eventType = EventType.None; // gonna make this a dropdown option in inspector in unity to manually add

    //  Added a public getter to use it in event manager 
    public EventType Event => eventType;
    [SerializeField]public ItemType itemType = ItemType.NONE; // gonna make this a dropdown option in inspector in unity to manually add

    /// ////////i edited here ///////////////////////////////////////////////////////////////

    [SerializeField] private PlayerMovementDirection movementDirection;
    public PlayerMovementDirection MovementDirection
    {
        get { return movementDirection; }
        set { movementDirection = value; }
    }
    private void Start()
    {
        if (itemType != ItemType.NONE)
        {
            this.isEventTile = true;
            this.eventType = EventType.COLLECT_ITEM;
        }
        if (this.isEventTile) this.type = TileType.EVENT;

    }


    /* public void TriggerEffect(Player player)
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
     }*/ // THE TILE CLASS REPRESENT A MODEL OF THE TILE WHICH STORE TILE DATA AND IT SHOULD NOT HAVE ANY INTERACTION WITH THE PLAYER ANOTHER CLASS SHOULD HANDLE THE INTERACTION
    // THE INTERACTION SHOULD BE HANDLED BY A CONTROLLER CLASS InteractionSystemController 
    // Follow the fkn MVC pattern 

    public Tile GetNextTile()
    {
        if (isCrossway)
        {
            Debug.Log("Crossway reached! The player must choose a direction.");
            return null; // Stop movement and wait for player choice
        }
        /**if (isEventTile)
        {
            Debug.Log("Event tile reached! Triggering event.");
            
            return null; // Stop movement and wait for event resolution
        } ????????? what is this ?????   */  
        
        return nextTiles.Count > 0 ? nextTiles[0] : null; // Default movement
    }
    public ItemType GetItem()
    {
        return itemType;
    }
}
