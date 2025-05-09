using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfilePanelUImanager : MonoBehaviour
{
    [SerializeField] FadeScript statsFadeIn;
    [SerializeField] BookPanel Book;

    [SerializeField] public Image avatar;
    [SerializeField] public TMP_Text usernameText;
    [SerializeField] public TMP_Text idText;
    [SerializeField] public TMP_Text eloText;
    [SerializeField] public TMP_Text totalGamesText;
    [SerializeField] public TMP_Text totalWonsText;
    [SerializeField] public TMP_Text errorText;
    [SerializeField] public GameObject[] stars;

    [SerializeField] public Button prevButton;
    [SerializeField] public Button nextButton;
    [SerializeField] public Button backButton;
    [SerializeField] public GameObject optionPanel;
    [Header("Add profile")]
    [SerializeField] public GameObject addPanel;
    [SerializeField] public TMP_InputField newNameInputField;
    [SerializeField] public Image newAvatar;
    List<Sprite> availableAvatars;
    private int currentAvatarIndex = 0;
    [Header("Edit profile")]
    [SerializeField] public GameObject editPanel;
    [SerializeField] public GameObject removePanel;
    [SerializeField] public TMP_InputField editNameInputField;
    [SerializeField] public Image editAvatar;
    [Header("Radar Chart")]
    [SerializeField] public UI_RadarChart radarChart;




    void Start()
    {

        Book.StartStatsFadeIn += () => startStatsFadeIn();
        statsFadeIn.FadeInSpeed = 0.8f;
        backButton.onClick.AddListener(OnBackButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadProfile(PlayerProfile profile)
    {
        avatar.sprite = profile.Icon != null ? Resources.Load<Sprite>(profile.Icon) : null;
        usernameText.text = profile.Name;
        idText.text = profile.Id.ToString();
        eloText.text = profile.Elo.ToString();
        totalGamesText.text = profile.Games.Played.ToString();
        totalWonsText.text = profile.Games.Won.ToString();
        radarChart.SetElo(profile.CategoriesElo);

    }
    public void SetErrorText(string text)
    {
        errorText.text = text;
    }
    public void ClearErrorText()
    {
        errorText.text = "";
    }
    public void DisableErrorMessage()
    {
        errorText.gameObject.SetActive(false);
    }
    public void EnableErrorMessage()
    {
        errorText.gameObject.SetActive(true);
    }
    public void startStatsFadeIn()
    {
        statsFadeIn.StartFadeIn();

    }
    public void startStatsFadeOut()
    {
        statsFadeIn.StartFadeOut();
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ShowAddPanel()
    {
        optionPanel.SetActive(true);
        addPanel.SetActive(true);
        editPanel.SetActive(false);
        removePanel.SetActive(false);
        newNameInputField.SetTextWithoutNotify("");
        newNameInputField.Select();
        newNameInputField.ActivateInputField();
        availableAvatars = new List<Sprite>(Resources.LoadAll<Sprite>(ProfileManager.IconsPath));
        if (availableAvatars.Count > 0)
        {
            currentAvatarIndex = 0;
            newAvatar.sprite = availableAvatars[currentAvatarIndex];
        }
        else
        {
            Debug.LogError("No avatars found in Resources/Sprites/ProfilesIcons");
        }
        newAvatar.sprite = availableAvatars[currentAvatarIndex];
        // Set the size of the newAvatar to a fixed size
        newAvatar.SetNativeSize();
        RectTransform rectTransform = newAvatar.GetComponent<RectTransform>();
        float scaleFactor = 200f; // Desired size
        rectTransform.sizeDelta = new Vector2(scaleFactor, scaleFactor);
    }
    public void AddProfileNextAvatar()
    {
        if (availableAvatars != null && availableAvatars.Count > 0)
        {
            currentAvatarIndex = (currentAvatarIndex + 1) % availableAvatars.Count;
            newAvatar.sprite = availableAvatars[currentAvatarIndex];
            
        }
    }
    public void AddProfilePreviousAvatar()
    {
        if (availableAvatars != null && availableAvatars.Count > 0)
        {
            currentAvatarIndex = (currentAvatarIndex - 1 + availableAvatars.Count) % availableAvatars.Count;
            newAvatar.sprite = availableAvatars[currentAvatarIndex];
            
        }
    }


    public void ShowEditPanel()
    {
        optionPanel.SetActive(true);
        addPanel.SetActive(false);
        editPanel.SetActive(true);
        removePanel.SetActive(false);
        editNameInputField.SetTextWithoutNotify("");
        editNameInputField.Select();
        editNameInputField.ActivateInputField();
        availableAvatars = new List<Sprite>(Resources.LoadAll<Sprite>(ProfileManager.IconsPath));
        if (availableAvatars.Count > 0)
        {
            currentAvatarIndex = 0;
            editAvatar.sprite = availableAvatars[currentAvatarIndex];
        }
        else
        {
            Debug.LogError("No avatars found in Resources/Sprites/ProfilesIcons");
        }
        editAvatar.sprite = availableAvatars[currentAvatarIndex];
        // Set the size of the newAvatar to a fixed size
        editAvatar.SetNativeSize();
        RectTransform rectTransform = editAvatar.GetComponent<RectTransform>();
        float scaleFactor = 200f; // Desired size
        rectTransform.sizeDelta = new Vector2(scaleFactor, scaleFactor);
    }
    public void EditProfileNextAvatar()
    {
        if (availableAvatars != null && availableAvatars.Count > 0)
        {
            currentAvatarIndex = (currentAvatarIndex + 1) % availableAvatars.Count;
            editAvatar.sprite = availableAvatars[currentAvatarIndex];

        }
    }
    public void EditProfilePreviousAvatar()
    {
        if (availableAvatars != null && availableAvatars.Count > 0)
        {
            currentAvatarIndex = (currentAvatarIndex - 1 + availableAvatars.Count) % availableAvatars.Count;
            editAvatar.sprite = availableAvatars[currentAvatarIndex];
        }
    }


    public void ShowRemovePanel()
    {
        optionPanel.SetActive(true);
        addPanel.SetActive(false);
        editPanel.SetActive(false);
        removePanel.SetActive(true);
    }
    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        addPanel.SetActive(false);
        editPanel.SetActive(false);
        removePanel.SetActive(false);
    }
    
}
