using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject SubMenuPanel;
    public GameObject TutorialPanel;
    public GameObject CreditsPanel;


    [SerializeField] private Animator CameraRig;
    void Start()
    {
        ShowMainMenu();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("CreateGame");

    }
    public void optionsPanel()
    {
        SceneManager.LoadScene("Options");
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); 
    }

    public void ShowMainMenu()
    {
        MainMenuPanel.SetActive(true);
        SubMenuPanel.SetActive(false);
    }
    public void ShowSubMenu()
    {
        MainMenuPanel.SetActive(false);
        SubMenuPanel.SetActive(true);
        TutorialPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }
    public void ShowTutorial()
    {
        SubMenuPanel.SetActive(false);
        TutorialPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        SubMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }
}