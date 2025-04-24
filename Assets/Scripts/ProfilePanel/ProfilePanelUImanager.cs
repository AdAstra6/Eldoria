using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public Button editProfileButton;
    [SerializeField] public Button deleteProfileButton;


    void Start()
    {
       
        Book.StartStatsFadeIn += () => startStatsFadeIn();
        statsFadeIn.FadeInSpeed = 0.8f;

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
}
