using System.Collections.Generic;
using UnityEngine;

public class RiddleManager : MonoBehaviour
{
    public static RiddleManager instance;
    public TextAsset kidsRiddleJson;
    public TextAsset adultRiddleJson;

    private RiddleDatabase kidsRiddles;
    private RiddleDatabase adultRiddles;

    private System.Random random = new System.Random();

    void Awake()
    {
        instance = this;
        LoadRiddles();
    }

    void LoadRiddles()
    {
        if (kidsRiddleJson != null)
            kidsRiddles = JsonUtility.FromJson<RiddleDatabase>(kidsRiddleJson.text);
        if (adultRiddleJson != null)
            adultRiddles = JsonUtility.FromJson<RiddleDatabase>(adultRiddleJson.text);
    }

    public Riddle GetRandomRiddle(GameModes difficulty)
    {
        List<Riddle> sourceList = difficulty == GameModes.ADULTS? adultRiddles.riddles : kidsRiddles.riddles;
        if ( sourceList .Count == 0)
        {
            LoadRiddles();
        }
        if (sourceList == null || sourceList.Count == 0)
        {
            Debug.LogWarning($"No riddles found for difficulty: {difficulty}");
            return null;
        }

        return sourceList[random.Next(0, sourceList.Count)];
    }
    public void RemoveRiddel(Riddle riddle , GameModes difficulty)
    {
        List<Riddle> sourceList = difficulty == GameModes.ADULTS ? adultRiddles.riddles : kidsRiddles.riddles;
        if (sourceList == null || sourceList.Count == 0)
        {
            Debug.LogWarning($"No riddles found for difficulty: {difficulty}");
            return;
        }
        sourceList.Remove(riddle);
    }
    
}
