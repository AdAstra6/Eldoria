using System;
using System.Collections.Generic;

[Serializable]
public class Question
{
    public int id;
    public string text;
    public string[] options;
    public int correctAnswer;
    public string difficulty;
    public string category;
}

[Serializable]
public class QuestionDatabase
{
    public List<Question> questions;
}