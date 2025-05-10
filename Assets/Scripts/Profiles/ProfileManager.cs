using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfileManager
{
    private readonly string filePath = Path.Combine(Application.persistentDataPath, "profiles.json");
    public static string IconsPath = Path.Combine("Sprites", "ProfilesIcons");

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

    public void UpdateProfiles(List<PlayerProfile> updatedProfiles)
    {
        List<PlayerProfile> currentProfiles = LoadProfiles();
        foreach (PlayerProfile updatedProfile in updatedProfiles)
        {
            PlayerProfile existingProfile = currentProfiles.Find(p => p.Id == updatedProfile.Id);
            if (existingProfile != null)
            {
                // Update existing profile
                existingProfile.Name = updatedProfile.Name;
                existingProfile.Elo = updatedProfile.Elo;
                existingProfile.CategoriesElo = updatedProfile.CategoriesElo;
                existingProfile.Games = updatedProfile.Games;
                existingProfile.Age = updatedProfile.Age;
                existingProfile.Icon = updatedProfile.Icon;
            }
            else
            {
                // Add new profile
                currentProfiles.Add(updatedProfile);
            }
        }
        SaveProfiles(currentProfiles);
    }
    public void UpdateProfile(PlayerProfile updatedProfile)
    {
        List<PlayerProfile> currentProfiles = LoadProfiles();
        PlayerProfile existingProfile = currentProfiles.Find(p => p.Id == updatedProfile.Id);
        if (existingProfile != null)
        {
            // Update existing profile
            existingProfile.Name = updatedProfile.Name;
            existingProfile.Elo = updatedProfile.Elo;
            existingProfile.CategoriesElo = updatedProfile.CategoriesElo;
            existingProfile.Games = updatedProfile.Games;
            existingProfile.Age = updatedProfile.Age;
            existingProfile.Icon = updatedProfile.Icon;
        }
        else
        {
            // Add new profile
            currentProfiles.Add(updatedProfile);
        }
        SaveProfiles(currentProfiles);
    }

    public void DeleteProfile(int id)
    {
        List<PlayerProfile> currentProfiles = LoadProfiles();
        PlayerProfile existingProfile = currentProfiles.Find(p => p.Id == id);
        if (existingProfile != null)
        {
            currentProfiles.Remove(existingProfile);
            SaveProfiles(currentProfiles);
        }
    }
    public void AddNewProfile(string name , string icon)
    {
        List<PlayerProfile> currentProfiles = LoadProfiles();
        PlayerProfile newProfile = new PlayerProfile
        {
            Id = currentProfiles.Count > 0 ? currentProfiles[currentProfiles.Count - 1].Id + 1 : 1,
            Name = name,
            Elo = EloSystemManager.DefaultElo,
            CategoriesElo = new Dictionary<string, int>()
            {
                { QuestionsCategories.GENERAL_KNOWLEDGE.GetKey(), EloSystemManager.DefaultElo },
                { QuestionsCategories.MATH.GetKey(), EloSystemManager.DefaultElo },
                { QuestionsCategories.BIOLOGY.GetKey(), EloSystemManager.DefaultElo },
                { QuestionsCategories.GEOGRAPHY.GetKey(), EloSystemManager.DefaultElo },
                { QuestionsCategories.ASTRONOMY.GetKey(), EloSystemManager.DefaultElo },
                { QuestionsCategories.ETHICS.GetKey(),EloSystemManager.DefaultElo },
                { QuestionsCategories.ZOOLOGY.GetKey(),EloSystemManager.DefaultElo }
            },
            Games = new GameStats { Played = 0, Won = 0 },
            Age = 0,
            Icon = icon // Set a default icon or load it from resources
        };
        currentProfiles.Add(newProfile);
        SaveProfiles(currentProfiles);
    }
    public void UpdateProfile(int id , string name, string icon)
    {
        List<PlayerProfile> currentProfiles = LoadProfiles();
        PlayerProfile existingProfile = currentProfiles.Find(p => p.Id == id);
        if (existingProfile != null)
        {
            existingProfile.Name = name;
            existingProfile.Icon = icon;
            SaveProfiles(currentProfiles);
        }
    }
}