using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class GameQuestionManager : MonoBehaviour
{
    public static GameQuestionManager Instance;
    
    [SerializeField] private QuestionManager QuestionManager;
    [SerializeField] private TimerCountDown timerCountDown;
    [SerializeField] private GameplayManager gameplayManager;
    public const float MCQTime = 60f; // Time for multiple choice question 
    private const float MSCQcriticalTIme = 15f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        timerCountDown.criticalTime = MSCQcriticalTIme;
    }

    public void AskQuestion(Player player)
    {
        gameplayManager.QuestionStarted();
        this.timerCountDown.SetTotalTime(MCQTime);
        this.timerCountDown.Restart();
        QuestionUI.Instance.ShowQuestion(player);

    }

    public void HandleAnswer(Player player, bool isCorrect)
    {
        if (isCorrect) RewardPlayer(player);
        else PenalizePlayer(player);
        gameplayManager.QuestionAnswered();
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