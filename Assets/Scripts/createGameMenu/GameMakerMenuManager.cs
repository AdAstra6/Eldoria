using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMakerMenuManager : MonoBehaviour
{
    public const int MAX_PLAYERS = 4; // Maximum number of players allowed
    private GameModes gameMode;
    private int playersCount;
    public int PlayersCount
    {
        get { return playersCount; }
        set { playersCount = value; }
    }
    private List<PlayerProfile> profiles;
    public List<PlayerProfile> Profiles
    {
        get { return profiles; }
        set { profiles = value; }
    }
    private List<PlayerProfile> selectedProfiles;
    public List<PlayerProfile> SelectedProfiles
    {
        get { return selectedProfiles; }
        set { selectedProfiles = value; }
    }

    private ProfileManager profileManager;

    // UI elements puted here because there is no need to create a new script for them , not much elements
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private Image kidsButtonIcon;
    [SerializeField] private Image adultsButtonIcon;


    void Start()
    {
        playersCount = 0;
        gameMode = GameModes.KIDS;
        kidsButtonIcon.color = new Color(0.5f, 0.5f, 0.5f, 1);
        selectedProfiles = new List<PlayerProfile>();
        profileManager = new ProfileManager();
        profiles = profileManager.LoadProfiles();
        playerCountText.text = playersCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGameMode(GameModes mode)
    {
        gameMode = mode;
    }
    public void SetGameModeAdult()
    {
        SetGameMode(GameModes.ADULTS);
        kidsButtonIcon.color = new Color(1, 1, 1, 1);
        adultsButtonIcon.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }
    public void SetGameModeKids()
    {
        SetGameMode(GameModes.KIDS);
        kidsButtonIcon.color = new Color(0.5f, 0.5f, 0.5f, 1);
        adultsButtonIcon.color = new Color(1, 1, 1, 1);
    }

    public void IncreasePlayersCount()
    {
        if (playersCount < MAX_PLAYERS)
        {
            playersCount++;
            playerCountText.text = playersCount.ToString();
        }
    }
    public void DecreasePlayersCount()
    {
        if (playersCount > 0)
        {
            playersCount--;
            playerCountText.text = playersCount.ToString();
        }
    }
    public void AddProfileToSelection(PlayerProfile profile)
    {
        if (!selectedProfiles.Contains(profile))
        {
            selectedProfiles.Add(profile);
        }
        if (selectedProfiles.Count > playersCount)
        {
            selectedProfiles.RemoveAt(0);
        }
    }
    public void RemoveProfileFromSelection(PlayerProfile profile)
    {
        if (selectedProfiles.Contains(profile))
        {
            selectedProfiles.Remove(profile);
        }
    }

    public void StartGame()
    {
        if (selectedProfiles.Count != playersCount)
        {
            Debug.LogError("Number of selected profiles does not match the number of players.");
            return;
        }
        if (playersCount < 2) {
            Debug.LogError("Not enough players selected.");
            return;
        }
        GameData.playersCount = playersCount;
        GameData.GameMode = gameMode;   
        GameData.SelectedProfiles = selectedProfiles;
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    public void BackToMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}


