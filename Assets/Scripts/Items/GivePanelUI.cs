using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class GivePanelUI : MonoBehaviour
{
    public static GivePanelUI Instance { get; private set; }
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button giveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_Dropdown PlayersDropDown;

    private Player currentPlayer;
    private Player targetPlayer;
    private List<Player> targetPlayers;

    private Item currentItem;
    private int maxQuantity;
    private int currentQuantity = 1;

    private void Awake()
    {

        Instance = this;
        plusButton.onClick.AddListener(OnPlusButtonClick);
        minusButton.onClick.AddListener(OnMinusButtonClick);
        giveButton.onClick.AddListener(OnGiveButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        PlayersDropDown.ClearOptions();
    }
    public void Initiate(Player current , List<Player> allPlayers)
    {
        currentPlayer = current;
        targetPlayers = allPlayers.ToList<Player>();
        targetPlayers.Remove(currentPlayer);
        targetPlayer = targetPlayers[0];
    }
        
    public void Show(Item item)
    {
        
        
        maxQuantity = item.quantity;
        currentItem = item;
        currentQuantity = 1;
        SetPlayersDropDown(targetPlayers);
        plusButton.interactable = true;
        minusButton.interactable = true;
        // Initialize UI
        itemIcon.sprite = ItemsTypeExtensioin.GetIcon(item.Type);
        quantityText.text = "1";
        gameObject.SetActive(true);
    }

    private void SetPlayersDropDown(List<Player> players)
    {
        PlayersDropDown.ClearOptions();
        List<string> playerNames = new List<string>();
        foreach (Player player in players)
        {
            playerNames.Add(player.profileData.Name);
        }
        PlayersDropDown.AddOptions(playerNames);
        PlayersDropDown.onValueChanged.AddListener(delegate { OnPlayerSelected(PlayersDropDown); });
    }

    private void OnPlayerSelected(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;
        if (selectedIndex < 0 || selectedIndex >= targetPlayers.Count)
        {
            Debug.LogError("Selected index is out of range.");
            currentPlayer = null;
            return;
        }
        targetPlayer = targetPlayers[selectedIndex];
    }



    public void Hide()
    {
        gameObject.SetActive(false);
        ResetPanel();
        ItemInventoryUI.Instance.Refresh(currentPlayer.GetInventory());
    }

    public void ResetPanel()
    {
        itemIcon.sprite = null;
        quantityText.text = "0";
        plusButton.interactable = false;
        minusButton.interactable = false;
        PlayersDropDown.ClearOptions();
        PlayersDropDown.onValueChanged.RemoveAllListeners();
    }
    private void OnPlusButtonClick()
    {
        if (currentQuantity < maxQuantity)
        {
            currentQuantity++;
            quantityText.text = currentQuantity.ToString();
        }
    }
    private void OnMinusButtonClick()
    {
        if (currentQuantity > 1)
        {
            currentQuantity--;
            quantityText.text = currentQuantity.ToString();
        }
    }
    private void OnGiveButtonClick()
    {
        if (currentPlayer != null && targetPlayer != null)
        {
            currentPlayer.Inventory.GiveItem(currentItem.Type, targetPlayer, currentQuantity);
            AudioManager.Instance.PlayGiveItem();
        }
        else
        {
            Debug.LogError("Current player or target player is null.");

        }
        Hide();
    }
    private void OnCancelButtonClick()
    {
        Hide();
    }

}
