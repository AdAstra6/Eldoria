using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField]private Button itemButton;
    [SerializeField]private TMP_Text quantity;
    private Item item;
    private Boolean isExpanded = false;

    [SerializeField]private Button useButton;
    [SerializeField] private Button giveButton;
    // Start is called before the first frame update
    public void Initiate(Item item , int index)
    {

        this.item = item;
        buttonImage.sprite = ItemsTypeExtensioin.GetIcon(item.Type);
        if (itemButton != null)
        {
            itemButton.onClick.AddListener(() => this.OnItemButtonClick());
        }
        if (giveButton != null)
        {
            giveButton.onClick.AddListener(() => this.OnGiveButtonClick());
        }
        if (useButton != null)
        {
            useButton.onClick.AddListener(() => OnUseButtonClick());
        }
        if (quantity != null)
        {
            quantity.text = item.quantity.ToString();
        }
        Collapse();
    }
    public void Reset()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = Resources.Load<Sprite>("Sprites/ItemsIcons/Default");
        }
        if (itemButton != null)
        {
            itemButton.onClick.RemoveAllListeners();
            giveButton.onClick.RemoveAllListeners();
            useButton.onClick.RemoveAllListeners();
        }
        if (quantity != null)
        {
            quantity.text = "0";
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Expand()
    {
        if (useButton != null)
        {
            useButton.gameObject.SetActive(true);
        }
        if (giveButton != null)
        {
            giveButton.gameObject.SetActive(true);

        }
        isExpanded = true;
    }
    public void Collapse()
    {
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }
        if (giveButton != null)
        {
            giveButton.gameObject.SetActive(false);
        }
        isExpanded = false;
    }

    public void OnUseButtonClick()
    {
        GameplayManager.Instance.UseItem(item.Type);
        AudioManager.Instance.PlayUseItem();
        Collapse();
    }

    public void OnGiveButtonClick()
    {
        ItemInventoryUI.Instance.DisableButtons();
        GivePanelUI.Instance.Show(item);
        Collapse();
    }
    public void OnItemButtonClick()
    {
        if (isExpanded)
        {
            Collapse();
        }
        else
        {
            ItemInventoryUI.Instance.CollapseAll();
            Expand();
        }
    }
    public void DisableButtons()
    {
        if (useButton != null)
        {
            useButton.interactable = false;
        }
        if (giveButton != null)
        {
            giveButton.interactable = false;
        }
        if (itemButton != null)
        {
            itemButton.interactable = false;
        }
    }
    public void EnableButtons()
    {
        if (useButton != null)
        {
            useButton.interactable = true;
        }
        if (giveButton != null)
        {
            giveButton.interactable = true;
        }
        if (itemButton != null)
        {
            itemButton.interactable = true;
        }
    }
}
