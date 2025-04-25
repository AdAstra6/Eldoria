using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EloSystemManager 
{
    private const int DefaultElo = 300;
    private static readonly int CategoryCount = Enum.GetValues(typeof(QuestionsCategories)).Length;

    private const double KFactor = 25.0;
    private const double LossKFactor = 18.0;
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
        profile.Elo = GetAverageElo(profile);
    }


}
