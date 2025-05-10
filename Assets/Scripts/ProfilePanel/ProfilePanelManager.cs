using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

public class ProfilePanelManager : MonoBehaviour
{
    [SerializeField] private Animator bookAnimator;
    [SerializeField] private ProfilePanelUImanager uimanager;
    private float animationDelay = 1.1f;
    private List<PlayerProfile> profiles;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        ProfileManager profileManager = new ProfileManager();
        profiles = profileManager.LoadProfiles();
        if (profiles.Count > 0)
        {
            // Load the first profile
            PlayerProfile currentProfile = profiles[0];
            uimanager.LoadProfile(currentProfile);
            uimanager.ClearErrorText();
            uimanager.DisableErrorMessage();
        }
        else
        {
            Debug.LogError("No profiles found.");
            uimanager.SetErrorText("No profiles found.");
            uimanager.EnableErrorMessage();
        }
        // set up buttons listeners
        uimanager.nextButton.onClick.AddListener(() => StartNextProfile());
        uimanager.prevButton.onClick.AddListener(() => StartPrevProfile());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartNextProfile()
    {
        StartCoroutine(NextProfile());
    }

    public IEnumerator NextProfile()
    {
        // update profiles
        uimanager.startStatsFadeOut();
        bookAnimator.SetTrigger("RightFlipPage");
        yield return new WaitForSeconds(animationDelay);
        currentIndex = (currentIndex + 1) % profiles.Count;
        PlayerProfile currentProfile = profiles[currentIndex];
        uimanager.LoadProfile(currentProfile);
        uimanager.startStatsFadeIn();
    }

    public void StartPrevProfile()
    {
        StartCoroutine(PrevProfile());
    }

    public IEnumerator PrevProfile()
    {
        // update profiles
        uimanager.startStatsFadeOut();
        bookAnimator.SetTrigger("LeftFlipPage");
        yield return new WaitForSeconds(animationDelay);
        currentIndex = (currentIndex - 1) ;
        if (currentIndex < 0) currentIndex = profiles.Count - 1;
        PlayerProfile currentProfile = profiles[currentIndex];
        uimanager.LoadProfile(currentProfile);
        uimanager.startStatsFadeIn();
    }

    public void DeleteProfile(int id)
    {
        ProfileManager profileManager = new ProfileManager();
        profileManager.DeleteProfile(id);
        profiles = profileManager.LoadProfiles();
        RefreshProfiles();
    }
    public void RefreshProfiles()
    {
        ProfileManager profileManager = new ProfileManager();
        profiles = profileManager.LoadProfiles();
        if (profiles.Count > 0)
        {
            // Load the first profile
            PlayerProfile currentProfile = profiles[0];
            uimanager.LoadProfile(currentProfile);
            uimanager.ClearErrorText();
            uimanager.DisableErrorMessage();
        }
        else
        {
            Debug.LogError("No profiles found.");
            uimanager.SetErrorText("No profiles found.");
            uimanager.EnableErrorMessage();
        }
    }

    public void OnDeleteProfileClicked()
    {
        int profileIdToDelete = profiles[currentIndex].Id;
        DeleteProfile(profileIdToDelete);
        uimanager.CloseOptionPanel();
    }
    
    public void OnAddNewProfileClick()
    {
        string name = uimanager.newNameInputField.text;
        string icon = Path.Combine(ProfileManager.IconsPath, uimanager.newAvatar.sprite.name);
        ProfileManager profileManager = new ProfileManager();
        if (!string.IsNullOrEmpty(name))
        {
            profileManager.AddNewProfile(name, icon);
        }
        RefreshProfiles();
        uimanager.CloseOptionPanel();
    }
    public void OnEditProfileClick()
    {
        string name = uimanager.editNameInputField.text;
        string icon = Path.Combine(ProfileManager.IconsPath, uimanager.editAvatar.sprite.name);
        ProfileManager profileManager = new ProfileManager();
        if (!string.IsNullOrEmpty(name))
        {
            profileManager.UpdateProfile(profiles[currentIndex].Id, name, icon);
        }
        else
        {
            profileManager.UpdateProfile(profiles[currentIndex].Id, profiles[currentIndex].Name, icon);
        }
            RefreshProfiles();
        uimanager.CloseOptionPanel();
    }
}