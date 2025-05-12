using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    [SerializeField] private Map map;
    [SerializeField] public List<Player> Players;
    public List<Player> activePlayers;
    [SerializeField] private int playersCount;
    [SerializeField] private InteractionSystemController interactionSystemController;
    private int currentPlayerIndex;
    [SerializeField] private DiceRoll diceRoll;

    private UIManager uiManager;
    [SerializeField] private ItemInventoryUI itemInventoryUI;
    public ItemInventoryUI ItemInventoryUI => itemInventoryUI;

    [SerializeField] public GivePanelUI givePanelUI;

    [SerializeField] private GameplayCameraController gameplayCameraController;

    private GamePhase gamePhase;
    public GamePhase GamePhase
    {
        get { return gamePhase; }
        set { gamePhase = value; }
    }

    void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        Instance = this;
        gamePhase = GamePhase.FIRST_PHASE;
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
                Players[i].AddItem(ItemType.BONUS_DICE);
                Players[i].AddItem(ItemType.HEAL_POTION);
                averageElo += Players[i].profileData.Elo;
                activePlayers.Add(Players[i]);
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
        if (gamePhase == GamePhase.GAME_OVER)
        {
            return;
        }
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
                //uiManager.RollDiceButtonHide();
                Debug.Log("Player " + currentPlayerIndex + " is moving.");
                break;
            case PlayerStats.CHOOSING_PATH:
                Debug.Log("Player " + currentPlayerIndex + " is choosing a path.");
                break;
            case PlayerStats.END_MOVING:
                Debug.Log("Player " + currentPlayerIndex + " has ended moving.");
                // HERE WHERE THE PLAYER SHOULD INTERACT WITH THE TILE
                //InteractionSystemController.Instance.TriggerTileInteraction(Players[currentPlayerIndex]);
                break;
            case PlayerStats.ANSWERING_QUESTION:
                Debug.Log("Player " + currentPlayerIndex + " is answering a question.");
                break;
            case PlayerStats.DOING_PUZZLE:
                Debug.Log("Player " + currentPlayerIndex + " is doing puzzle.");
                break;
            case PlayerStats.STRATEGIC_CHOICE:
                Debug.Log("Player " + currentPlayerIndex + " is making a strategic choice.");
                
                break;
            case PlayerStats.END_TURN:
                Debug.Log("Player " + currentPlayerIndex + " has ended their turn.");
                //EndTurn();
                break;
        }
    }

    public void QuestionAnswered()
    {
        Players[currentPlayerIndex].Effects.HasHealthProtection = false;
        StartStrategicPhase();
    }
    public void QuestionStarted()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.ANSWERING_QUESTION;
    }


    public void EndTurn()
    {
        Players[currentPlayerIndex].PlayerState = PlayerStats.END_TURN;
        // HERE WHERE THE PLAYER TURN ENDED AND THE NEXT PLAYER TURN STARTS
        Players[currentPlayerIndex].PlayerState = PlayerStats.IDLE;
        UIManager.Instance.EndTurnButtonHide();
        //itemInventoryUI.Hide(); // Hide item inventory UI after turn ends
        ItemInventoryUI.Instance.Hide(); // Hide item inventory UI after turn ends  
        //givePanelUI.Hide(); // Hide give panel UI after turn ends
        currentPlayerIndex++;
        if (currentPlayerIndex >= playersCount)
        {
            currentPlayerIndex = 0;
        }
    }

    public void GameOver(bool isWin)
    {
        gamePhase = GamePhase.GAME_OVER;
        ProfileManager profileManager = new ProfileManager();
        List<PlayerProfile> profiles = new List<PlayerProfile>();
        for (int i = 0; i < playersCount; i++)
        {
            Debug.Log("Player acumulated : " + Players[i].AccumulatedElo.Keys.ToList());
            EloSystemManager.ApplyAccumulatedElo(Players[i], isWin);

            Debug.Log($"{Players[i].profileData.Name} has {Players[i].profileData.Elo} Elo points.");
            profiles.Add(Players[i].profileData);

        }
        profileManager.UpdateProfiles(profiles);
        Debug.Log("Game Over. Elo points saved.");
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu"); // temporary TODO : add a statistics scene after game over
    
    }

    public void StartStrategicPhase() {
        Players[currentPlayerIndex].PlayerState = PlayerStats.STRATEGIC_CHOICE;
        gameplayCameraController.SetType(CameraType.FREE);
        UIManager.Instance.EndTurnButtonShow();
        // new item system
        ItemInventoryUI.Instance.Refresh(Players[currentPlayerIndex].GetInventory());
        ItemInventoryUI.Instance.Show(); // Show item inventory UI after answering the question
        GivePanelUI.Instance.Initiate(Players[currentPlayerIndex], activePlayers);
    }
    public void UseItem(ItemType item)
    {
        Players[currentPlayerIndex].UseItem(item);
        ItemInventoryUI.Instance.DisableButtons();
        ItemInventoryUI.Instance.Refresh(Players[currentPlayerIndex].GetInventory());
    }
    public void AddItemToPlayer(ItemType type,int quantity)
    {
        Players[currentPlayerIndex].AddItem(type,quantity);
    }
    public void PuzzleTerminated(bool correct)
    {
        if (correct)
        {
            // handle correct puzzle
        }
        else
        {
            // handle incorrect puzzle
        }
        StartStrategicPhase();
    }
    public void OnPlayerDeath(Player player)
    {
        UIManager.Instance.ShowInfoText($"{Players[currentPlayerIndex].profileData.Name} has died!");
        bool haveTotoem = false;
        Player saver = null;
        foreach (Player pl in activePlayers)
        {
            if (pl.Inventory.HasItemOfType(ItemType.TOTME_OF_UNDYING))
            {
                haveTotoem = true;
                saver = pl;
            }
        }
        if (haveTotoem)
        {
            UIManager.Instance.ShowInfoText("You have been saved by the Totem Of Undying!");
            player.IncreaseHealth();
            saver.Inventory.RemoveItem(ItemType.TOTME_OF_UNDYING);
        } else
        {
            UIManager.Instance.ShowInfoText("You Lost The Game!");
            StartCoroutine(EndGameAfterWait(false,1.0f));
        }
    }
    private IEnumerator EndGameAfterWait(bool win , float waitTIme)
    {
        yield return new WaitForSeconds(waitTIme);
        GameOver(win);
    }
    public void StartCapitalPhase()
    {
        UIManager.Instance.ShowInfoText("Capital phase started!");
        currentPlayerIndex = 0;
        List<Tile> capitalStartPoints = map.GetCapitalStartPoints().ToList();
        for (int i = 0; i < activePlayers.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, capitalStartPoints.Count);
            activePlayers[i].currentTile = capitalStartPoints[index];
            activePlayers[i].transform.position = capitalStartPoints[index].transform.position;
            capitalStartPoints.RemoveAt(index);
            activePlayers[i].PlayerState = PlayerStats.IDLE;
        }
    }



}

