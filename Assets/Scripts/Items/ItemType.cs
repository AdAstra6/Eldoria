public enum ItemType
{
    NONE = -1,
    HEAL_POTION,
    BONUS_DICE

}
public static class ItemsTypeExtensioin
{
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
            default:
                return "Unknown item type";
        }
    }
}