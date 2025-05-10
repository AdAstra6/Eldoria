using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class QuestionManager : MonoBehaviour
{
    public TextAsset questionsJsonFile;
    private QuestionDatabase questionDB;
    private System.Random random;

    void Awake()
    {
        random = new System.Random();
        LoadQuestions();
    }

    void LoadQuestions()
    {
        if (questionsJsonFile != null)
        {
            // Parse the JSON data
            questionDB = JsonUtility.FromJson<QuestionDatabase>(questionsJsonFile.text);
            Debug.Log($"Loaded {questionDB.questions.Count} questions");
        }
        else
        {
            Debug.LogError("Questions JSON file not assigned!");
        }
    }

    public Question GetRandomQuestion()
    {
        if (questionDB == null || questionDB.questions.Count == 0)
        {
            Debug.LogWarning("No questions available!");
            return null;
        }

        int randomIndex = random.Next(0, questionDB.questions.Count);
        return questionDB.questions[randomIndex];
    }

   
    public Question GetRandomQuestion(string difficulty)
    {
        if (questionDB == null || questionDB.questions.Count == 0)
            return null;

        List<Question> filteredQuestions = new List<Question>();

        // Filter questions
        foreach (Question q in questionDB.questions)
        {
            bool difficultyMatch = string.IsNullOrEmpty(difficulty) || q.difficulty == difficulty;

            if (difficultyMatch)
            {
                filteredQuestions.Add(q);
            }
        }

        if (filteredQuestions.Count == 0)
        {
            Debug.LogWarning("No questions match the specified criteria!");
            return null;
        }

        int randomIndex = random.Next(0, filteredQuestions.Count);
        return filteredQuestions[randomIndex];
    }
}
