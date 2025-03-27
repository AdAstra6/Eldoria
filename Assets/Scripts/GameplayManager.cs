using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class GameplayManager : MonoBehaviour
{

    [SerializeField] private Map map;
    [SerializeField] private List<Player> Players;
    [SerializeField] private int playersCount;
    [SerializeField] private InteractionSystemController interactionSystemController;
    private int currentPlayerIndex;
    [SerializeField] private DiceRoll diceRoll;

    private UIManager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        Debug.Log("GameplayManager initialized.");
        List<Tile> spawnPoints = map.getRandomSpawnpoints(playersCount);
        for (int i = 0; i < playersCount; i++)
        {
            Players[i].currentTile = spawnPoints[i];
            Players[i].transform.position = spawnPoints[i].transform.position;
            Players[i].PlayerState = PlayerStats.IDLE;
            Debug.Log("Player " + i + " spawned at " + spawnPoints[i].name);
        }
        this.currentPlayerIndex = 0;
    }

    void Update()
    {
        switch (Players[currentPlayerIndex].PlayerState)
        {
            case PlayerStats.IDLE:
                Debug.Log("Player " + currentPlayerIndex + " is idle.");
                diceRoll.CurrentPlayer = Players[currentPlayerIndex];
                UIManager.Instance.RollDiceButtonShow();
                Players[currentPlayerIndex].PlayerState = PlayerStats.ROLLING_DICE;
                break;
            case PlayerStats.ROLLING_DICE:
                Debug.Log("Player " + currentPlayerIndex + " is rolling the dice.");
                break;
            case PlayerStats.MOVING:
                Debug.Log("Player " + currentPlayerIndex + " is moving.");
                break;
            case PlayerStats.CHOOSING_PATH:
                Debug.Log("Player " + currentPlayerIndex + " is choosing a path.");
                break;
            case PlayerStats.END_MOVING:
                Debug.Log("Player " + currentPlayerIndex + " has ended moving.");
                // HERE WHERE THE PLAYER SHOULD INTERACT WITH THE TILE
                // NOW BECAUSE NO INTERACTION WITH THE TILE IS IMPLEMENTED THE PLAYER GO TO STRATEGIC CHOICE STATE
                uiManager.RollDiceButtonHide();
                interactionSystemController.Instance.TriggerTileInteraction(Players[currentPlayerIndex]);
                break;
            case PlayerStats.ANSWERING_QUESTION:
                Debug.Log("Player " + currentPlayerIndex + " is answering a question.");
                break;
            case PlayerStats.STRATEGIC_CHOICE:
                Debug.Log("Player " + currentPlayerIndex + " is making a strategic choice.");
                // HERE WHERE THE PLAYER SHOULD MAKE A STRATEGIC CHOICE AND HE CAN END HIS TURN
                UIManager.Instance.EndTurnButtonShow();
                break;
            case PlayerStats.END_TURN:
                Debug.Log("Player " + currentPlayerIndex + " has ended their turn.");
                // HERE WHERE THE PLAYER TURN ENDED AND THE NEXT PLAYER TURN STARTS
                Players[currentPlayerIndex].PlayerState = PlayerStats.IDLE;
                UIManager.Instance.EndTurnButtonHide();
                currentPlayerIndex++;
                if (currentPlayerIndex >= playersCount)
                {
                    currentPlayerIndex = 0;
                }
                //EndTurn();
                break;
        }
    }

    public void EndTurn()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.END_TURN;
    }
}
