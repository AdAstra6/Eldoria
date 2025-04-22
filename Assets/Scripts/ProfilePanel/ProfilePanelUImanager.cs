using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePanelUImanager : MonoBehaviour
{
    [SerializeField] FadeScript statsFadeIn;
    [SerializeField] BookPanel Book;


    public Image avatar;
    public TMP_Text usernameText;
    public TMP_Text idText;
    public TMP_Text eloText;
    public TMP_Text totalGamesText;
    public TMP_Text totalWonsText;
    public TMP_Text errorText;

    public Button prevButton;
    public Button nextButton;


    void Start()
    {
        Book.StartStatsFadeIn += () => startStatsFadeIn();
    }

    // Update is called once per frame  
    void Update()
    {

    }
    public void startStatsFadeIn()
    {
        statsFadeIn.StartFadeIn();

    }
    public void startStatsFadeOut()
    {
        statsFadeIn.StartFadeOut();
    }

    public void UpdateProfileDisplay(PlayerProfile p)
    {



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

        // TODO : Update stars  
    }

    public void SetErrorText()
    {
        if (errorText != null)
        {
            errorText.text = "No profiles found";
            errorText.gameObject.SetActive(true);
        }
    }
    public void HideErrorText()
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }

}
