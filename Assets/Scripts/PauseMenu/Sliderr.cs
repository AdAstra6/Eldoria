using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliderr : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1f);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}