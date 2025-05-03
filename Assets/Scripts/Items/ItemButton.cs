using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Image buttonImage;
    private Button useButton;
    private TMP_Text quantity;
    private Item item;
    // Start is called before the first frame update
    public void Initiate(Item item)
    {

        this.item = item;
        buttonImage = GetComponentInChildren<Image>();
        useButton = GetComponentInChildren<Button>();
        quantity = GetComponentInChildren<TMP_Text>();
        if (buttonImage != null)
        {
            // Set the image based on item type
            switch (item.Type)
            {
                case ItemType.HEAL_POTION:
                    buttonImage.sprite = Resources.Load<Sprite>("Sprites/ItemsIcons/HealPotion");
                    break;
                case ItemType.BONUS_DICE:
                    buttonImage.sprite = Resources.Load<Sprite>("Sprites/ItemsIcons/BonusDice");
                    break;
                default:
                    buttonImage.sprite = Resources.Load<Sprite>("Sprites/ItemsIcons/Default");
                    break;
            }
        }
        if (useButton != null)
        {
            useButton.onClick.AddListener(() => GameplayManager.Instance.UseItem(item.Type));
        }
        if (quantity != null)
        {
            quantity.text = item.quantity.ToString();
        }
    }
    public void Reset()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = Resources.Load<Sprite>("Sprites/ItemsIcons/Default");
        }
        if (useButton != null)
        {
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
}
