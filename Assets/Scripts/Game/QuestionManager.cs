using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AskQuestion(Player player)
    {
        // Display question UI and wait for answer
        UIManager.Instance.ShowQuestionPanel(player);
    }

    public void HandleAnswer(Player player, bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log(player.name + " answered correctly! Rewarding player.");
            // Apply reward logic here
        }
        else
        {
            Debug.Log(player.name + " answered incorrectly! Applying penalty.");
            // Apply penalty logic here
        }

        // Resume the game
        GameManager.Instance.EndTurn();
    }
}
