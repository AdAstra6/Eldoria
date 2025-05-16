using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;
    void Start()
    {
        if(!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            Load();
        }
        else
        {
            Load();
        }
        UpdateIcon();
        AudioListener.pause = muted;
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
        Save();
        UpdateIcon();
    }
    public void UpdateIcon()
    {
        if (muted)
        {
            soundOnIcon.gameObject.SetActive(false);
            soundOffIcon.gameObject.SetActive(true);
        }
        else
        {
            soundOnIcon.gameObject.SetActive(true);
            soundOffIcon.gameObject.SetActive(false);
        }
    }
    public void Load()
    {
        muted = PlayerPrefs.GetInt("muted", 0) == 1;
    }
    public void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}
