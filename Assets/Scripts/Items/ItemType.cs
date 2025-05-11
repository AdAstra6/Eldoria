using System.IO;
using UnityEngine;

public enum ItemType
{
    NONE = -1,
    HEAL_POTION,
    BONUS_DICE,
    STURDY_SWORD,
    HEALING_HERB,
    
    // Collectable items starts here from 200
    LAVA_STONE = 200,
    COCONUTS,
    LOG,
    FLOWER,
    SNOW_MUSHROOM,
    // Collectable items ends here



}
public static class ItemsTypeExtensioin
{
    private static string itemsIconsPath = Path.Combine("Sprites", "ItemsIcons");
    public static string GetDescription(this ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HEAL_POTION:
                return "Restores 1 HP";
            case ItemType.BONUS_DICE:
                return "Roll 3 dice next turn";
            default:
                return "Unknown item type";
        }
    }
    public static string GetName(this ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HEAL_POTION:
                return "Heal Potion";
            case ItemType.BONUS_DICE:
                return "Bonus Dice";
            case ItemType.LAVA_STONE:
                return "Lava Stone";
            case ItemType.COCONUTS:
                return "Coconuts";
            case ItemType.LOG:
                return "Log";
            case ItemType.FLOWER:
                return "Flower";
            case ItemType.SNOW_MUSHROOM:
                return "Snow Mushroom";
            default:
                return "Unknown item type";
        }
    }
    private static string GetIconPath(this ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HEAL_POTION:
                return Path.Combine(itemsIconsPath, "HealPotion");
            case ItemType.BONUS_DICE:
                return Path.Combine(itemsIconsPath, "BonusDice");
            case ItemType.LAVA_STONE:
                return Path.Combine(itemsIconsPath, "LavaStone");
            case ItemType.COCONUTS:
                return Path.Combine(itemsIconsPath, "Coconuts");
            case ItemType.LOG:
                return Path.Combine(itemsIconsPath, "Log");
            case ItemType.FLOWER:
                return Path.Combine(itemsIconsPath, "Flower");
            case ItemType.SNOW_MUSHROOM:
                return Path.Combine(itemsIconsPath, "SnowMushroom");
            default:
                return Path.Combine(itemsIconsPath, "Default");
        }
    }
    public static Sprite GetIcon(ItemType itemType)
    {
        Sprite sprite= Resources.Load<Sprite>(GetIconPath(itemType));
        if (sprite == null)
        {
            Debug.LogError($"Icon not found for item type: {itemType}");
            return Resources.Load<Sprite>(GetIconPath(ItemType.NONE));
        }
        return sprite;

    }
    public static bool IsUsable(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.HEAL_POTION:
            case ItemType.BONUS_DICE:
            case ItemType.STURDY_SWORD:
            case ItemType.HEALING_HERB:
                return true;
            case ItemType.LAVA_STONE:
            case ItemType.COCONUTS:
            case ItemType.LOG:
            case ItemType.FLOWER:
            case ItemType.SNOW_MUSHROOM:
                return false;
            default:
                return false;
        }
    }
}