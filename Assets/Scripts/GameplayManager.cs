using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; } 
    [SerializeField] private Map map;
    [SerializeField] private List<Player> Players;
    [SerializeField] private int playersCount;
    [SerializeField] private InteractionSystemController interactionSystemController;
    private int currentPlayerIndex;
    [SerializeField] private DiceRoll diceRoll;

    private UIManager uiManager;
    [SerializeField] private GameplayCameraController gameplayCameraController;

    void Start()
    {
        Instance = this; 
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        Debug.Log("GameplayManager initialized.");

        List<PlayerProfile> selectedProfiles = GameData.SelectedProfiles;
        playersCount = selectedProfiles.Count;
        List<Tile> spawnPoints = map.getRandomSpawnpoints(playersCount);
        int initialHealth = playersCount == 2 ? 4 : 3;
        int averageElo = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            if (i < playersCount)
            {
                Players[i].gameObject.SetActive(true); // activate used players
                Players[i].SetInitialHealth(initialHealth); // Set initial health for each player
                Players[i].Initialize(selectedProfiles[i]);
                Players[i].currentTile = spawnPoints[i];
                Players[i].transform.position = spawnPoints[i].transform.position;
                Players[i].PlayerState = PlayerStats.IDLE;
                Debug.Log("Player " + i + " spawned at " + spawnPoints[i].name);




                //temporary
                Players[i].inventory.Add(new Item(ItemType.BONUS_DICE, "Bonus Dice", "Adds extra dice on next roll"));
                averageElo += Players[i].profileData.Elo;
            }
            else
            {
                Players[i].gameObject.SetActive(false); // deactivate unused players
            }
        }
        GameQuestionManager.gameAverageElo = averageElo / playersCount;
        this.currentPlayerIndex = 0;
        gameplayCameraController.SetPlayer(Players[currentPlayerIndex].gameObject.transform);
    }


    void Update()
    {
        switch (Players[currentPlayerIndex].PlayerState)
        {
            case PlayerStats.IDLE:
                Debug.Log("Player " + currentPlayerIndex + " is idle.");
                gameplayCameraController.SetPlayer(Players[currentPlayerIndex].gameObject.transform);
                gameplayCameraController.SetType(CameraType.FIXED);
                diceRoll.CurrentPlayer = Players[currentPlayerIndex];
                UIManager.Instance.RollDiceButtonShow();
                Players[currentPlayerIndex].PlayerState = PlayerStats.ROLLING_DICE;
                break;
            case PlayerStats.ROLLING_DICE:
                Debug.Log("Player " + currentPlayerIndex + " is rolling the dice.");
                break;
            case PlayerStats.MOVING:
                uiManager.RollDiceButtonHide();
                Debug.Log("Player " + currentPlayerIndex + " is moving.");
                break;
            case PlayerStats.CHOOSING_PATH:
                Debug.Log("Player " + currentPlayerIndex + " is choosing a path.");
                break;
            case PlayerStats.END_MOVING:
                Debug.Log("Player " + currentPlayerIndex + " has ended moving.");
                // HERE WHERE THE PLAYER SHOULD INTERACT WITH THE TILE
                // NOW BECAUSE NO INTERACTION WITH THE TILE IS IMPLEMENTED THE PLAYER GO TO STRATEGIC CHOICE STATE
                interactionSystemController.Instance.TriggerTileInteraction(Players[currentPlayerIndex]);
                break;
            case PlayerStats.ANSWERING_QUESTION:
                Debug.Log("Player " + currentPlayerIndex + " is answering a question.");
                break;
            case PlayerStats.STRATEGIC_CHOICE:
                Debug.Log("Player " + currentPlayerIndex + " is making a strategic choice.");
                gameplayCameraController.SetType(CameraType.FREE);
                // HERE WHERE THE PLAYER SHOULD MAKE A STRATEGIC CHOICE AND HE CAN END HIS TURN
                UIManager.Instance.EndTurnButtonShow();


                // temporary
                if (Input.GetKeyDown(KeyCode.I)) // Press I to test
                {
                    TestUseFirstItem();
                }
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

    public void QuestionAnswered()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.STRATEGIC_CHOICE;
    }
    public void QuestionStarted()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.ANSWERING_QUESTION;
    }


    public void EndTurn()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.END_TURN;
    }

    public void GameOver(bool isWin)
    {
        foreach (Player player in Players) 
        {
            EloSystemManager.ApplyAccumulatedElo(player, isWin); 
            Debug.Log($"{player.name} has {player.profileData.Elo} Elo points.");
        }

    }


    // temporary
    public void TestUseFirstItem()
    {
        Player currentPlayer = Players[currentPlayerIndex];
        if (currentPlayer.inventory.Count > 0)
        {
            currentPlayer.UseItem(currentPlayer.inventory[0]);
        }
        else
        {
            Debug.Log("No items available for player.");
        }
    }
}
