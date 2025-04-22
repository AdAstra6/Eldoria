using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("CreateGame"); 

    }
    public void optionsPanel()
    {
        SceneManager.LoadScene("Options");
    }
}
