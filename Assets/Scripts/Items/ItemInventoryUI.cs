using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    public static ItemInventoryUI Instance { get; private set; }
    [SerializeField] private ItemButton[] itemButtons;
    [SerializeField] private GameObject ItemsContainer; // Panel contains inventory UI

    private void Start()
    {
        Instance = this;
        itemButtons = GetComponentsInChildren<ItemButton>();
        Instance.Hide();
    }
    public void Refresh(List<Item> inventory)
    {
        ClearButtons();
        if (inventory == null || inventory.Count == 0)
        {
            Debug.Log("No items in inventory.");
            gameObject.SetActive(false);
            return;
        }
        int index = 0;
        foreach (Item item in inventory)
        {
            itemButtons[index].Initiate(item);
            itemButtons[index].Show();
            index++;

        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearButtons();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void ClearButtons()
    {
        foreach (ItemButton button in itemButtons)
        {
            if (button != null)
            {
                button.Hide();
            }
        }
    }
}
