[System.Serializable]
public class PlayerRuntimeData
{
    public PlayerProfile Profile;
    public int Health;
    public PlayerItemsManager Inventory;

    public PlayerRuntimeData(Player player)
    {
        Profile = player.profileData;
        Health = player.CurrentHealth;
        Inventory = player.Inventory;
    }
}
