using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfileManager
{
    private readonly string filePath = Path.Combine(Application.persistentDataPath, "profiles.json");

    // Load all profiles (tries persistentDataPath first, fallback to Resources)
    public List<PlayerProfile> LoadProfiles()
    {
        string json = "";

        if (File.Exists(filePath))
        {
            // Load from persistent data (if it exists)
            json = File.ReadAllText(filePath);
        }
        else
        {
            // Otherwise, load from Resources (default profiles)
            TextAsset file = Resources.Load<TextAsset>("Profiles/profiles");
            if (file != null)
            {
                json = file.text;
            }
            else
            {
                Debug.LogError("Profiles file not found in Resources/Profiles/profiles.json");
                return new List<PlayerProfile>();
            }
        }

        ProfileListWrapper wrapper = JsonConvert.DeserializeObject<ProfileListWrapper>(json);
        return wrapper?.Players ?? new List<PlayerProfile>();
    }

    // Save profiles to persistent data
    public void SaveProfiles(List<PlayerProfile> profiles)
    {
        ProfileListWrapper wrapper = new() { Players = profiles };
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // Get a single profile by ID
    public PlayerProfile GetProfileById(int id)
    {
        var profiles = LoadProfiles();
        return profiles.Find(p => p.Id == id);
    }
}
