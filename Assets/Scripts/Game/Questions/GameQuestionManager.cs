using UnityEngine;
using System.Collections.Generic;

public class GameQuestionManager : MonoBehaviour
{
    public static GameQuestionManager Instance;
    
    [SerializeField] private QuestionManager QuestionManager; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AskQuestion(Player player)
    {
        QuestionUI.Instance.ShowQuestion(player);
    }

    public void HandleAnswer(Player player, bool isCorrect)
    {
        if (isCorrect) RewardPlayer(player);
        else PenalizePlayer(player);
        
        GameManager.Instance.EndTurn();
        QuestionUI.Instance.HideQuestion();
    }

    private void RewardPlayer(Player player)
    {
        Debug.Log($"{player.name} answered correctly!");
        // Add movement bonus or other rewards
    }

    private void PenalizePlayer(Player player)
    {
        Debug.Log($"{player.name} answered incorrectly!");
        // Implement penalty logic
    }
}