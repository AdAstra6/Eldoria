[System.Serializable]
public class Item
{
    public ItemType Type;
    public string Name;
    public string Description;

    public Item(ItemType type, string name, string description)
    {
        Type = type;
        Name = name;
        Description = description;
    }
}