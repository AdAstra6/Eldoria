using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ProfileViewer : MonoBehaviour
{
    [SerializeField] private GameObject instance;
    [SerializeField] private Image avatar;
    [SerializeField] private TMP_Text profileName; 
    private bool isSelected = false;
    private PlayerProfile profile;
    public PlayerProfile Profile
    {
        get { return profile; }
        set { profile = value; }
    }

    // Start is called before the first frame update  
    void Start()
    {
    }

    // Update is called once per frame  
    void Update()
    {
    }

    public void SetProfile(PlayerProfile profile)
    {
        this.profile = profile;
        profileName.text = profile.Name;
        avatar.sprite = Resources.Load<Sprite>(profile.Icon);
    }

    public void EnableView()
    {
        instance.SetActive(true);
    }

    public void DisableView()
    {
        instance.SetActive(false);
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
        {
            avatar.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else
        {
            avatar.color = new Color(1, 1, 1, 1);
        }
    }
}
