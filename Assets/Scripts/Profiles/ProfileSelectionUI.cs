using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileSelectionUI : MonoBehaviour
{
    public Image avatar;
    public TMP_Text usernameText;
    public TMP_Text idText;
    public TMP_Text eloText;
    public TMP_Text totalGamesText;
    public TMP_Text totalWonsText;
    public TMP_Text errorText;
    public GameObject[] stars;

    public Button prevButton;
    public Button nextButton;
    public Button selectButton;
    public Button startGameButton;

    private List<PlayerProfile> profiles;
    private HashSet<int> selectedIds = new();
    private int currentIndex = 0;

    void Start()
    {
        GameData.SelectedProfiles.Clear();

        profiles = new ProfileManager().LoadProfiles() ?? new List<PlayerProfile>();

        bool hasProfiles = profiles.Count > 0;

        prevButton.interactable = hasProfiles;
        nextButton.interactable = hasProfiles;
        selectButton.interactable = hasProfiles;
        startGameButton.interactable = false;

        if (!hasProfiles)
        {
            errorText.text = "No profiles found!";
            return;
        }

        UpdateProfileDisplay();

        prevButton.onClick.AddListener(() => ChangeProfile(-1));
        nextButton.onClick.AddListener(() => ChangeProfile(1));
        selectButton.onClick.AddListener(ToggleCurrentSelection);
        startGameButton.onClick.AddListener(StartGame);

        errorText.text = "";
    }

    void ChangeProfile(int direction)
    {
        if (profiles == null || profiles.Count == 0)
            return;

        currentIndex = (currentIndex + direction + profiles.Count) % profiles.Count;
        UpdateProfileDisplay();
    }

    void UpdateProfileDisplay()
    {
        if (profiles == null || profiles.Count == 0)
            return;

        if (currentIndex < 0 || currentIndex >= profiles.Count)
        {
            Debug.LogError($"Invalid profile index: {currentIndex}");
            return;
        }

        PlayerProfile p = profiles[currentIndex];

        if (usernameText) usernameText.text = p.Name;
        if (idText) idText.text = $"{p.Id}";
        if (eloText) eloText.text = p.Elo.ToString();
        if (totalGamesText) totalGamesText.text = p.Games.Played.ToString();
        if (totalWonsText) totalWonsText.text = p.Games.Won.ToString();

        // Avatar loading
        if (avatar != null)
        {
            Sprite avatarSprite = Resources.Load<Sprite>(p.Icon);
            if (avatarSprite != null)
                avatar.sprite = avatarSprite;
            else
                Debug.LogWarning($"Avatar sprite not found for icon: {p.Icon}");
        }

        // Update stars (ensure it's not null or misconfigured)
        if (stars != null)
        {
            float ratio = Mathf.Clamp01(p.Games.WinRatio);
            int activeCount = Mathf.CeilToInt(ratio * stars.Length);

            for (int i = 0; i < stars.Length; i++)
                stars[i].SetActive(i < activeCount);
        }

        // Select button text
        if (selectButton != null)
        {
            TMP_Text btnText = selectButton.GetComponentInChildren<TMP_Text>();
            if (btnText != null)
                btnText.text = selectedIds.Contains(p.Id) ? "Deselect" : "Select";
        }
    }

    void ToggleCurrentSelection()
    {
        if (profiles == null || profiles.Count == 0)
            return;

        PlayerProfile p = profiles[currentIndex];

        if (selectedIds.Contains(p.Id))
        {
            selectedIds.Remove(p.Id);
            GameData.SelectedProfiles.RemoveAll(x => x.Id == p.Id);
        }
        else
        {
            if (selectedIds.Count >= 4)
            {
                errorText.text = "Maximum 4 players allowed.";
                return;
            }

            selectedIds.Add(p.Id);
            GameData.SelectedProfiles.Add(p);
        }

        errorText.text = "";
        UpdateProfileDisplay();
        startGameButton.interactable = selectedIds.Count >= 2 && selectedIds.Count <= 4;
    }

    void StartGame()
    {
        if (GameData.SelectedProfiles.Count < 2 || GameData.SelectedProfiles.Count > 4)
        {
            errorText.text = "Please select 2 to 4 players.";
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
