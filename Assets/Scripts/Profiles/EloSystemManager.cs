using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EloSystemManager 
{
    public const int DefaultElo = 300;
    public const int EstimatedMaxElo = 2500;
    private static readonly int CategoryCount = Enum.GetValues(typeof(QuestionsCategories)).Length;

    private const double KFactor = 25.0 * 5; // *5 because number of questions in each game is not big
    private const double LossKFactor = 18.0 *5;
    private const int MaxGain = 20;
    private const int MaxLoss = 15;
    private const int CategoriesCount = 6;

    public static int GetEloByCategory(string categoryName, PlayerProfile profile)
    {
        return profile.CategoriesElo.TryGetValue(categoryName, out int elo) ? elo : 0;
    }
    public static void SetEloByCategory(string categoryName, PlayerProfile profile, int newElo)
    {
        profile.CategoriesElo[categoryName] = newElo;
    }
    public static int GetAverageElo(PlayerProfile profile)
    {
        return (int)profile.Elo;
    }
    public static void AddEloBasedOnQuestionResult(Player player, string difficulty, string categoryName, int gameAverageElo)
    {
        if (player == null || player.profileData == null) return;
        PlayerProfile profile = player.profileData;
        int difficultyValue = difficulty.ToLower() switch
        {
            "easy" => 300,
            "medium" => 600,
            "hard" => 900,
            _ => 0
        };

        if (difficultyValue == 0) return;
        int currentElo = EloSystemManager.GetEloByCategory(categoryName, profile);
        int playerAverage = GetAverageElo(profile);

        double gain = KFactor * (difficultyValue / (double)(currentElo + 1)) * (gameAverageElo / (double)(playerAverage + 1));
        gain = Math.Min(gain, MaxGain);

        if (categoryName.CompareTo(QuestionsCategories.GENERAL_KNOWLEDGE.GetKey()) == 0) gain *= 2.0 / CategoriesCount;

        player.AccumulatedElo[categoryName] = player.AccumulatedElo.TryGetValue(categoryName, out int currentValue)
            ? currentValue + (int)Math.Round(gain)
            : (int)Math.Round(gain);
        Debug.Log($"Adding Elo: {categoryName} - {gain}");
    }

    public static void SubtractEloBasedOnQuestionResult(Player player, string difficulty, string categoryName, int gameAverageElo)
    {
        if (player == null || player.profileData == null) return;
        PlayerProfile profile = player.profileData;
        int difficultyValue = difficulty.ToLower() switch
        {
            "easy" => 300,
            "medium" => 600,
            "hard" => 900,
            _ => 0
        };

        if (difficultyValue == 0) return;

        int currentElo = profile.CategoriesElo[categoryName];
        int playerAverage = GetAverageElo(profile);

        double loss = LossKFactor * ((currentElo + 1) / (double)difficultyValue) * ((playerAverage + 1) / (double)gameAverageElo);
        loss = Math.Min(loss, MaxLoss);
        player.AccumulatedElo[categoryName] = player.AccumulatedElo.TryGetValue(categoryName, out int currentValue)
            ? currentValue - (int)Math.Round(loss)
            : 0;
        Debug.Log($"Subtracting Elo: {categoryName} - {loss}");
    }

    public static int CalculateAverageElo(PlayerProfile profile)
    {
        int newAverage = 0;
        foreach (QuestionsCategories category in Enum.GetValues(typeof(QuestionsCategories)))
        {
            newAverage += profile.CategoriesElo[category.GetKey()];
        }
        newAverage /= Enum.GetValues(typeof(QuestionsCategories)).Length;
        return newAverage;
    }
    
    public static void ApplyAccumulatedElo(Player player, bool gameWin)
    {
        if (player == null || player.profileData == null) return;
        PlayerProfile profile = player.profileData;
        const int MinElo = 1;
        const int MaxAverage = 70;
        int totalCategories = CategoriesCount;
        int maxTotal = MaxAverage * totalCategories;

        int averageElo = GetAverageElo(profile);
        double winMultiplier = Math.Max(0.8, Math.Min(1.2 - 0.0004 * averageElo, 1.2));
        double lossMultiplier = Math.Max(0.5, Math.Min(0.8 - 0.00015 * averageElo, 0.8));

        Dictionary<string, int> changes = new Dictionary<string, int>();
        int totalChange = 0;

        foreach (QuestionsCategories category in Enum.GetValues(typeof(QuestionsCategories)))
        {
            changes.Add(category.GetKey(), (int)Math.Round(player.AccumulatedElo[category.GetKey()] * (gameWin ? winMultiplier : lossMultiplier)));
            totalChange += changes[category.GetKey()];
        }

        if (totalChange > maxTotal)
        {
            double scale = maxTotal / (double)totalChange;
            foreach (var key in changes.Keys.ToList())
            {
                changes[key] = (int)Math.Round(changes[key] * scale);
            }
        }

        foreach (QuestionsCategories category in Enum.GetValues(typeof(QuestionsCategories)))
        {
            profile.CategoriesElo[category.GetKey()] = Math.Max(MinElo, profile.CategoriesElo[category.GetKey()] + changes[category.GetKey()]);
           
        }
        profile.Elo = CalculateAverageElo(profile);

        if (gameWin) profile.Games.Won++;
        profile.Games.Played++;
        UpdateWinRatio(profile);
    }
    public static void UpdateWinRatio(PlayerProfile profile)
    {
        if (profile == null) return;
        if (profile.Games.Played == 0) return;
        profile.Games.WinRatio = (float)profile.Games.Won / profile.Games.Played;
    }
    public static int GetAverageEloForNewGame(List<PlayerProfile> players)
    {
        if (players == null || players.Count == 0) return DefaultElo;
        int totalElo = 0;
        foreach (PlayerProfile player in players)
        {
            totalElo += player.Elo;
        }
        return totalElo / players.Count;
    }
    public static string GetDifficultyLabel(int elo)
    {
        if (elo > 0 && elo < 600) return "easy";
        else if (elo >= 600 && elo < 1200) return "medium";
        else if (elo >= 1200) return "hard";
        else return "unknown";
    }
    public static int GetDifficultyValue(int elo)
    {
        if (elo > 0 && elo < 600) return 2;
        else if (elo >= 600 && elo < 1200) return 3;
        else if (elo >= 1200) return 4;
        else return 0;
    }
    public static int GetNumberOfStars(PlayerProfile profile)
    {
        int elo = profile.Elo;
        if (elo < 500) return 0;
        if (elo < 1000) return 1;
        if (elo < 1500) return 2;
        return 3;
    }
    public static string GetTitle(PlayerProfile profile)
    {
        int elo = profile.Elo;
        if (elo < 300) return "newbie";
        if (elo < 600) return "Thinker";
        if (elo < 900) return "Scholar";
        if (elo < 1200) return "Sage";
        if (elo < 1500) return "Mastermind";
        return "Legendary";
    }
}
