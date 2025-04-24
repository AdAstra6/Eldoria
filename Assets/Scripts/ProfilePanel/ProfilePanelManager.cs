using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
