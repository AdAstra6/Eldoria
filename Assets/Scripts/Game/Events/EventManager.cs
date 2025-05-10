using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EventManager : MonoBehaviour
{
    //  Static instance
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        //  Check if instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you want it to persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate singletons
        }
    }

    // Main handler method
    public void HandleEvent(EventType eventType, Player player)
    {
        Debug.Log($"Handling event of type: {eventType}");

        switch (eventType)
        {
            case EventType.MotherTree:
                LoadEventScene("Mothertree");
                break;
            case EventType.DeadendChest:
                //LoadEventScene("TrapScene");
                break;
            case EventType.XO_game:
                //LoadEventScene("HealScene");
                break;
            case EventType.SandCastle:
                //LoadEventScene("TeleportScene");
                break;
                case EventType.Blacksmith:
                //LoadEventScene("TeleportScene");
                break;
            case EventType.Shop:
                //LoadEventScene("TeleportScene");
                break;
            case EventType.Bookstore:
                //LoadEventScene("TeleportScene");
                break;
            case EventType.COLLECT_ITEM:

            default:
                Debug.LogWarning("Unhandled event type!");
                break;
        }
        StartCoroutine(SimulateEventDuration(player));
    }

    private void LoadEventScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single); // i might need additives here keep in mind 
    }

    private void updatejsonforcapital()
    {

    }

     private IEnumerator SimulateEventDuration(Player player)
    {
        yield return new WaitForSeconds(2f); // Simulate event time
        FinishEvent(player);
    }

    public void FinishEvent(Player player)
    {
        Debug.Log("Event finished, resuming player movement by 1 tile.");

        if (player != null)
        {
            player.PlayerState = PlayerStats.FINISHED_EVENT;
            player = null;
        }
        else
        {
            Debug.LogError("No player stored to resume after event!");
        }
    }
    private void StartPlayerCollectItem(Player player)
    {
        Tile tile = player.currentTile;
        if (tile== null)
        {
            Debug.LogError("Player's current tile is null!");
            return;
        }
        ItemType item = tile.GetItem();


    }


}
