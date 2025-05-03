[System.Serializable]
public class Item
{
    public ItemType Type;
    public string Name;
    public string Description;
    public int quantity = 1; // Default quantity is 1

    public Item(ItemType type, string name, string description)
    {
        Type = type;
        Name = name;
        Description = description;
    }
}