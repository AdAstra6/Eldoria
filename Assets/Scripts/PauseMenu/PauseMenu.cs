using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] GameObject ButtonsHolder;
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject OptionsMenu;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        PauseButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        PauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Options()
    {
        ButtonsHolder.SetActive(false);
        OptionsMenu.SetActive(true);

    }
    public void Back()
    {
        ButtonsHolder.SetActive(true);
        OptionsMenu.SetActive(false);
    }
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}