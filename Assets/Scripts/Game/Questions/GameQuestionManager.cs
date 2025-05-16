using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class GameQuestionManager : MonoBehaviour
{
    public static GameQuestionManager Instance;
    public static int gameAverageElo = 0;

    [SerializeField] private QuestionManager QuestionManager;
    [SerializeField] private TimerCountDown timerCountDown;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private Question currentQuestion;

    // riddles 

    [Header("Riddles")]
    [SerializeField] private RiddleManager riddleManager;
    [SerializeField] private bool useRiddle = false; // Toggle this based on game context
    [SerializeField] private GameModes riddleDifficulty = GameData.GameMode; // or "adult"
    [SerializeField] private Riddle currentRiddle;

    public const float MCQTime = 60f; // Time for multiple choice question 
    private const float MSCQcriticalTIme = 15f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        timerCountDown.criticalTime = MSCQcriticalTIme;
        riddleDifficulty = GameData.GameMode;
    }

    public void AskQuestion(Player player)
    {
        // Play the Scroll sound
        AudioManager.Instance.PlayScrollOpen();
        gameplayManager.QuestionStarted();
        this.timerCountDown.SetTotalTime(MCQTime);
        this.timerCountDown.Restart();
        currentQuestion = QuestionManager.GetRandomQuestion(EloSystemManager.GetDifficultyLabel(GameData.averageElo));
        QuestionManager.RemoveQuestion(currentQuestion);  
        QuestionUI.Instance.CurrentQuestion = currentQuestion;
        QuestionUI.Instance.ShowQuestion(player);

    }

    public void AskRiddle(Player player)
    {
        AudioManager.Instance.PlayScrollOpen();
        timerCountDown.SetTotalTime(MCQTime);
        timerCountDown.Restart();
        currentRiddle = riddleManager.GetRandomRiddle(riddleDifficulty);
        if (currentRiddle == null)
        {
            Debug.LogWarning("No riddles available for the current game mode.");
            return;
        }
        riddleManager.RemoveRiddel(currentRiddle, riddleDifficulty);
        QuestionUI.Instance.SetRiddle(currentRiddle);
        QuestionUI.Instance.ShowRiddle(player); // Correct method call
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
        EloSystemManager.AddEloBasedOnQuestionResult(player, currentQuestion.difficulty, currentQuestion.category, gameAverageElo);
        // Add movement bonus or other rewards
    }

    private void PenalizePlayer(Player player)
    {
        Debug.Log($"{player.name} answered incorrectly!");
        EloSystemManager.SubtractEloBasedOnQuestionResult(player, currentQuestion.difficulty, currentQuestion.category, gameAverageElo);
        // Implement penalty logic
        if (!player.Effects.HasHealthProtection) player.DecreaseHealth(1); // Decrease health by 1
        else Debug.Log($"{player.name} has health protection, no damage taken."); // this should be a visual effect
        if (player.CurrentHealth <= 0)
        {
            GameplayManager.Instance.OnPlayerDeath(player);
        }
    }
}
// tguembir