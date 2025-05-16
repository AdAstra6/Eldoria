using Newtonsoft.Json;

[System.Serializable]
public class GameStats
{
    [JsonProperty("played")]
    public int Played;

    [JsonProperty("won")]
    public int Won;

    [JsonProperty("winRatio")]
    public float WinRatio;
}
