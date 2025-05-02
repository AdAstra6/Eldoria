using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsManager : MonoBehaviour
{
    public const int MAX_INVENTORY_SIZE = 8;
    [SerializeField] public List<Item> inventory;
    private Player player;
    public Player Player
    {
        get { return player; }
        set
        {
            player = value;
        }
    }


    public void Insitialize(Player player)
    {
        this.player = player;
        inventory = new List<Item>();
    }
    public void UseItem(ItemType item)
    {
        switch (item)
        {
            case ItemType.HEAL_POTION:
                player.IncreaseHealth();
                Debug.Log($"{player.profileData.Name} used a Heal Potion! HP is now {player.CurrentHealth}");
                break;

            case ItemType.BONUS_DICE:
                player.HasBonusDiceNextTurn = true;
                Debug.Log($"{player.profileData.Name} used a Bonus Dice! They?ll roll 3 dice next turn.");
                break;
        }

        RemoveItem(item);
    }
    public void AddItem(ItemType itemType)
    {
        // Check if the item already exists in the inventory  
        Item existingItem = inventory.Find(item => item.Type == itemType);
        if (existingItem != null)
        {
            existingItem.quantity++;
            Debug.Log($"{player.profileData.Name} now has {existingItem.quantity} {existingItem.Name}(s)!");
            return;
        }

        // Check if the inventory is full  
        if (inventory.Count >= MAX_INVENTORY_SIZE)
        {
            Debug.Log($"{player.profileData.Name}'s inventory is full! Cannot add more items.");
            return;
        }

        // Add a new item with quantity 1  
        Item newItem = new Item(itemType, ItemsTypeExtensioin.GetName(itemType), ItemsTypeExtensioin.GetDescription(itemType));
        inventory.Add(newItem);
        //Debug.Log($"{player.profileData.Name} received a {newItem.Name}!");
    }
    public void RemoveItem(ItemType itemType)
    {
        Item itemToRemove = inventory.Find(item => item.Type == itemType);
        if (itemToRemove != null)
        {
            itemToRemove.quantity--;
            if (itemToRemove.quantity <= 0)
            {
                inventory.Remove(itemToRemove);
                Debug.Log($"{player.profileData.Name} has used all of their {itemToRemove.Name}(s)!");
            }
            else
            {
                Debug.Log($"{player.profileData.Name} now has {itemToRemove.quantity} {itemToRemove.Name}(s) left!");
            }
        }
        else
        {
            Debug.Log($"{player.profileData.Name} does not have a {ItemsTypeExtensioin.GetName(itemType)} to remove.");
        }
    }

}
