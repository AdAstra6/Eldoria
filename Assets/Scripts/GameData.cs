using System.Collections.Generic;

public static class GameData
{
    public static List<PlayerProfile> SelectedProfiles = new();
    public static int playersCount = 0;
    public static GameModes GameMode = GameModes.KIDS;
    public static int averageElo;
}
