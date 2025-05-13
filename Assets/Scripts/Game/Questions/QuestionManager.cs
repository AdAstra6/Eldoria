using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class QuestionManager : MonoBehaviour
{
    private readonly string KidsQuestionsPath = Path.Combine("MultipleChoiceQuestions", "kids_quiz_questions");
    private readonly string AdultQuestionsPath = Path.Combine("MultipleChoiceQuestions", "adult_quiz_questions");
    private TextAsset questionsJsonFile;
    private QuestionDatabase questionDB;
    private System.Random random;

    void Awake()
    {
        random = new System.Random();
        LoadQuestions();
    }

    void LoadQuestions()
    {
        if (GameData.GameMode == GameModes.KIDS)
        {
            questionsJsonFile = Resources.Load<TextAsset>(KidsQuestionsPath);
        }
        else if (GameData.GameMode == GameModes.ADULTS)
        {
            questionsJsonFile = Resources.Load<TextAsset>(AdultQuestionsPath);
        }
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
        {
            LoadQuestions();
        }
        if (questionDB == null || questionDB.questions.Count == 0)
        {
            Debug.LogWarning("No questions available!");
            return null;
        }

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
            return GetRandomQuestion(); // Fallback to any question
        }

        int randomIndex = random.Next(0, filteredQuestions.Count);
        return filteredQuestions[randomIndex];
    }
    public void RemoveQuestion(Question question)
    {
        if (questionDB == null || questionDB.questions.Count == 0)
            return;
        questionDB.questions.Remove(question);
        Debug.Log($"Removed question: {question.text}");
    }
}
