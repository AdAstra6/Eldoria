using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile
{
    [JsonProperty("id")]
    public int Id;

    [JsonProperty("name")]
    public string Name;

    [JsonProperty("elo")]
    public int Elo;

    [JsonProperty("categoriesElo")]
    public Dictionary<string, int> CategoriesElo;

    [JsonProperty("games")]
    public GameStats Games;

    [JsonProperty("age")]
    public int Age;

    [JsonProperty("icon")]
    public string Icon;
}
