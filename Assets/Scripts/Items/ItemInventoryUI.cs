using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private TMP_Text titleText;

    private Player currentPlayer;

    public void ShowItems(Player player)
    {
        currentPlayer = player;
        ClearButtons();

        if (titleText != null)
            titleText.text = player.profileData.Name + "'s Items";

        if (player.inventory == null || player.inventory.Count == 0)
        {
            Debug.Log("No items in inventory.");
            gameObject.SetActive(false);
            return;
        }

        foreach (var item in player.inventory)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            Button button = buttonObj.GetComponent<Button>();

            if (label != null)
                label.text = item.Name;

            if (button != null)
            {
                Item captured = item; // avoid closure bug
                button.onClick.AddListener(() =>
                {
                    currentPlayer.UseItem(captured);
                    Refresh(); // Refresh list after use
                });
            }
        }

        gameObject.SetActive(true);
    }

    public void Refresh()
    {
        ShowItems(currentPlayer); // Refresh UI with updated inventory
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearButtons();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
    }
}
