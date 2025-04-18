using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class ProfileListWrapper
{
    [JsonProperty("players")]
    public List<PlayerProfile> Players;
}
