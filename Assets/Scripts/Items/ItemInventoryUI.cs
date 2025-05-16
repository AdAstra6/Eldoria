using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    public static ItemInventoryUI Instance { get; private set; }
    [SerializeField] private ItemButton[] itemButtons;
    [SerializeField] private GameObject ItemsContainer; // Panel contains inventory UI
    private Button collapseAllButton;
    [SerializeField] private Image playerSkinImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Instance = this;
        itemButtons = GetComponentsInChildren<ItemButton>();
        collapseAllButton = GetComponent<Button>();
        if (collapseAllButton != null)
        {
            collapseAllButton.onClick.AddListener(() =>
            {
                CollapseAll();
            });
        }
        Instance.Hide();
    }
    public void Refresh(List<Item> inventory)
    {
        DisableButtons();
        ClearButtons();
        if (inventory == null || inventory.Count == 0)
        {
            Debug.Log("No items in inventory.");
            gameObject.SetActive(false);
            return;
        }
        int index = 0;
        for (int i = 0;i<inventory.Count;i++)
        {
            itemButtons[index].Initiate(inventory[i],i);
            itemButtons[index].Show();
            index++;
        }
        EnableButtons();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearButtons();
        if (GivePanelUI.Instance == null) GivePanelUI.SetInstance();
        GivePanelUI.Instance.Hide();
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
                button.Reset();
                button.Hide();
            }
        }
    }

    public void DisableButtons()
    {
        foreach (ItemButton button in itemButtons)
        {
            if (button != null)
            {
                button.DisableButtons();
            }
        }
    }
    public void EnableButtons()
    {
        foreach (ItemButton button in itemButtons)
        {
            if (button != null && button.gameObject.activeSelf)
            {
                button.EnableButtons();
            }
        }
    }
    public void CollapseAll()
    {
        foreach (ItemButton button in itemButtons)
        {
            if (button != null && button.gameObject.activeSelf)
            {
                button.Collapse();
            }
        }
    }
    public void SetPlayerSkinImage(Sprite sprite)
    {
        playerSkinImage.sprite = sprite;
    }


}
