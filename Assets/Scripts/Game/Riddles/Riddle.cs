using System;
using System.Collections.Generic;

[Serializable]
public class Riddle
{
    public int id;
    public string text;
    public string[] options;
    public int correctAnswer;
    public string difficulty; // "kids" or "adult"
}

[Serializable]
public class RiddleDatabase
{
    public List<Riddle> riddles;
}
