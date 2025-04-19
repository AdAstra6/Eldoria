using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesSelectionPanel : MonoBehaviour
{
    public const int MAX_PROFILES_VIEW = 4;
    private int currentIndex = 0;
    [SerializeField] private GameMakerMenuManager gameMakerMenuManager;
    [SerializeField] private ProfileViewer[] profilesViewerList = new ProfileViewer[MAX_PROFILES_VIEW];

    [SerializeField] private TMP_Text selectionCounter;
    



    public void UpdateContent()
    {
        List<PlayerProfile> profiles;
        profiles = gameMakerMenuManager.Profiles;
        List<PlayerProfile> selectedProfiles = gameMakerMenuManager.SelectedProfiles;
        int indexer = currentIndex;
        for (int i = 0; i < MAX_PROFILES_VIEW; i++)
        {
            if (i < profiles.Count)
            {
                profilesViewerList[i].SetProfile(profiles[indexer]);
                profilesViewerList[i].EnableView();
                if (selectedProfiles.Contains(profiles[indexer]))
                {
                    profilesViewerList[i].SetSelected(true);
                }
                else
                {
                    profilesViewerList[i].SetSelected(false);
                }
            } else
            {
                profilesViewerList[i].DisableView();
            }
                indexer++;
            if (indexer == profiles.Count) indexer = 0;
        }
    }

    public void IncreamentCurrentIndex()
    {
        currentIndex++;
        if (currentIndex == gameMakerMenuManager.Profiles.Count) currentIndex = 0;
        UpdateContent();
    }
    public void DecreamentCurrentIndex()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = gameMakerMenuManager.Profiles.Count - 1;
        UpdateContent();
    }

    public void ProfileToggleSelection(int i)
    {
        if (gameMakerMenuManager.PlayersCount == 0) return;
        PlayerProfile profile = profilesViewerList[i].Profile;
        if (profile != null)
        {
            if (gameMakerMenuManager.SelectedProfiles.Contains(profile))
            {
                profilesViewerList[i].SetSelected(false);
                gameMakerMenuManager.RemoveProfileFromSelection(profile);

            }
            else
            {
                PlayerProfile proflieToInselect = null;
                bool removeProfile = gameMakerMenuManager.SelectedProfiles.Count == gameMakerMenuManager.PlayersCount;
                if (removeProfile) proflieToInselect = gameMakerMenuManager.SelectedProfiles[0];
                gameMakerMenuManager.AddProfileToSelection(profile);
                profilesViewerList[i].SetSelected(true);
                if (removeProfile)
                {
                    for (int j = 0; j < profilesViewerList.Length; j++)
                    {
                        if (profilesViewerList[j].Profile == proflieToInselect)
                        {
                            profilesViewerList[j].SetSelected(false);
                            break;
                        }
                    }
                }

            }
        }
        selectionCounter.text = gameMakerMenuManager.SelectedProfiles.Count.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        if (gameMakerMenuManager == null)
        {
            gameMakerMenuManager = GameObject.Find("GameMakerMenuManager").GetComponent<GameMakerMenuManager>();
        }
        selectionCounter.text = "0";
        UpdateContent();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

