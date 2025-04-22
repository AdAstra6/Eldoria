using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ProfilePanelManager : MonoBehaviour
{
    [SerializeField] private Animator bookAnimator;
    [SerializeField] private ProfilePanelUImanager uimanager;
    private float delayTime = 1.1f;  // Delay time for the fade animation when flipping the page
    List<PlayerProfile> profiles;
    int currentProfileIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        ProfileManager profileManager = new ProfileManager();
        profiles = profileManager.LoadProfiles();
        bool hasProfiles = profiles.Count > 0;
        if (hasProfiles)
        {
            uimanager.UpdateProfileDisplay(profiles[currentProfileIndex]);
            uimanager.HideErrorText();
        }
        else
        {
            uimanager.SetErrorText();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Wrapper method with void return type
    public void StartNextProfile()
    {
        StartCoroutine(NextProfile());
    }
    public IEnumerator NextProfile()
    {
        // update profiles
        uimanager.startStatsFadeOut();
        currentProfileIndex++;
        if (currentProfileIndex >= profiles.Count)
        {
            currentProfileIndex = 0;
        }
        bookAnimator.SetTrigger("RightFlipPage");
        yield return new WaitForSeconds(delayTime);
        uimanager.startStatsFadeIn();
        uimanager.UpdateProfileDisplay(profiles[currentProfileIndex]);
    }
}
