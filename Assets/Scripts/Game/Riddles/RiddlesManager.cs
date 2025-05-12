using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RiddlesManager : MonoBehaviour
{
    [SerializeField] public static  RiddlesManager Instance { get; private set; }
    public TextAsset riddlesJsonFile;
    private readonly static string kidsRiddlesFilePath = Path.Combine("Riddles", "KidsRiddles.json");
    private readonly static string adultsRiddlesFilePath = Path.Combine("Riddles", "AdultsRiddles.json");
    private QuestionDatabase riddleDB;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Question getRandomRiddle(GameModes moadda)
    {
       if (riddleDB == null || riddleDB.questions.Count == 0)
        {
            Debug.LogWarning("No riddles available!");
            return null;
        }
        int randomIndex = Random.Range(0, riddleDB.questions.Count);
        return riddleDB.questions[randomIndex];
    }
    public void iniciateRiddles(GameModes moadda)
    {
        string filePath = moadda == GameModes.KIDS ? kidsRiddlesFilePath : adultsRiddlesFilePath;
        TextAsset riddlesJsonFile = Resources.Load<TextAsset>(filePath);

        if (riddlesJsonFile != null)
        {
            riddleDB = JsonUtility.FromJson<QuestionDatabase>(riddlesJsonFile.text);
            
            
        }
        else
        {
            Debug.LogError("Riddles JSON file not assigned!");
            
        }
    }
    public void removeRiddle(Question riddle)
    {
        if (riddleDB != null && riddleDB.questions.Contains(riddle))
        {
            riddleDB.questions.Remove(riddle);
        }
        else
        {
            Debug.LogWarning("Riddle not found in the database!");
        }
    }
    
}
