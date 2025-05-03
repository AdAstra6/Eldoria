using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{

    public Tile currentTile;
    private Tile nextTile;
    private bool pathChosen = false;
   

    public float moveSpeed = 0.5f;
    [SerializeField] private bool isMoving = false;
    private int remainingSteps = 0;
    private PlayerStats playerState;

    private Stack<Tile> pathcrossedPath = new Stack<Tile>();

    public PlayerProfile profileData;
    public Dictionary<string, int> AccumulatedElo;

    private PlayerItemsManager inventory;
    //public bool HasBonusDiceNextTurn { get; set; } = false; // << Added for bonus dice management
    private PlayerEffects effects;
    public PlayerEffects Effects
    {
        get { return effects; }
        set { effects = value; }
    }


    [SerializeField] private PlayerVisual playerVisual;

    private TMP_Text nameLabel;  // << Added for player name display


    public int MaxHealth { get; private set; } // << Added for health management
    public int CurrentHealth { get;  set; }  // << Added for health management

    public void SetInitialHealth(int health)  // << Added for setting initial health
    {
        MaxHealth = health;
        CurrentHealth = health;
    }

    public void Initialize(PlayerProfile profile)
    {
        this.profileData = profile;
        this.effects = new PlayerEffects();

        // Find the TextMeshProUGUI in the children
        if (nameLabel == null)
        {
            nameLabel = GetComponentInChildren<TMP_Text>();
        }

        if (nameLabel != null)
        {
            UpdateNameAndHealthUI();
        }
        AccumulatedElo = new Dictionary<string, int>();
        foreach (QuestionsCategories category in Enum.GetValues(typeof(QuestionsCategories)))
        {
            string key = category.GetKey();
            Debug.Log($"Initializing key: {key}");
            AccumulatedElo.Add(key, 0);
        }
        if (inventory == null)
        {
            inventory = GetComponent<PlayerItemsManager>();
            if (inventory == null)
            {
                Debug.LogError("PlayerItemsManager component not found!");
            }
        }
        inventory.Insitialize(this);
    }
    private void Awake()
    {

        playerVisual = GetComponent<PlayerVisual>();
        if (playerVisual == null)
        {
            Debug.LogError("PlayerVisual component not found in children.");
            return;
        }
        inventory = GetComponent<PlayerItemsManager>();
        if (inventory == null)
        {
            Debug.LogError("PlayerItemsManager component not found!");
        }
    }

    public PlayerStats PlayerState
    {
        get { return playerState; }
        set { playerState = value; }
    }

    private void Start()
    {
        if (playerVisual != null)
        {
            playerVisual.SetMoving(false);
            playerVisual.SetMovementDirection(PlayerMovementDirection.NONE);
        }
    }
    private void Update()
    {
        playerVisual.SetMoving(isMoving);
        playerVisual.SetMovementDirection(isMoving ? currentTile.MovementDirection : PlayerMovementDirection.NONE);
    }

    public void Move(int steps)
    {
        if (isMoving || currentTile == null) return;
        this.playerState = PlayerStats.MOVING;
        isMoving = true;
        playerVisual.SetMoving(true);
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
        GameplayManager.Instance.StartStrategicPhase();
        //GameManager.Instance.EndTurn();
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
           
            isMoving = true;
            yield return StartCoroutine(MoveToTile(nextTile.transform.position));
            //yield return new WaitUntil(() => nextTileReached);
            this.SavePathCrossed(currentTile);
            currentTile = nextTile;
            remainingSteps--;
        }

        isMoving = false;
        this.playerState = PlayerStats.END_MOVING;
        //if (remainingSteps <= 0) GameManager.Instance.EndTurn();
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

    private void UpdateNameAndHealthUI() // << Added for updating name and health UI
    {
        if (nameLabel == null) return;

        int hearts = CurrentHealth;

        nameLabel.text = $"{profileData.Name}\nHP: {hearts}";
    }

    public void DecreaseHealth(int amount = 1) // << Added for decreasing health
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        Debug.Log($"{profileData.Name}'s Health is now {CurrentHealth}");
        UpdateNameAndHealthUI(); // Optional: refresh health display if you show HP
    }
    public void IncreaseHealth(int amount = 1) // << Added for increasing health
    {
        CurrentHealth++;
        Debug.Log($"{profileData.Name}'s Health is now {CurrentHealth}");
        UpdateNameAndHealthUI(); // Optional: refresh health display if you show HP
    }


    public void UseItem(ItemType item)
    {
        inventory.UseItem(item);
    }

    public void GiveItem(Player target, Item item)
    {
        /*if (inventory.Contains(item))
        {
            inventory.Remove(item);
            target.inventory.Add(item);
            Debug.Log($"{profileData.Name} gave {item.Name} to {target.profileData.Name}");
        }*/
    }

    public void GiveHeart(Player target)
    {
        if (CurrentHealth > 1)
        {
            CurrentHealth--;
            target.CurrentHealth++;
            UpdateNameAndHealthUI();
            target.UpdateNameAndHealthUI();
            Debug.Log($"{profileData.Name} gave 1 heart to {target.profileData.Name}");
        }
        else
        {
            Debug.Log("Can't give heart (maybe not enough HP or target is full).");
        }
    }

    public void AddItem(ItemType itemType)
    {
        inventory.AddItem(itemType);
    }
    public void RemoveItem(ItemType itemType)
    {
        inventory.RemoveItem(itemType);
    }
    public List<Item> GetInventory()
    {
        return inventory.inventory;
    }


}
