using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GivePanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;

    private Player currentPlayer;

    public void Show(Player current, List<Player> allPlayers)
    {
        currentPlayer = current;
        ClearButtons();

        if (titleText != null)
            titleText.text = "Help a teammate";

        foreach (Player other in allPlayers)
        {
            if (other == current || !other.gameObject.activeSelf) continue;


            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            if (txt != null)
                txt.text = $"Give to {other.profileData.Name}";

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenChoiceSubPanel(other);
            });
        }

        gameObject.SetActive(true);
    }

    private void OpenChoiceSubPanel(Player target)
    {
        ClearButtons();

        GameObject heartBtn = Instantiate(buttonPrefab, buttonContainer);
        heartBtn.GetComponentInChildren<TMP_Text>().text = $"❤️ Give 1 Heart to {target.profileData.Name}";
        heartBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPlayer.GiveHeart(target);
            Hide();
        });

        foreach (Item item in currentPlayer.inventory)
        {
            GameObject itemBtn = Instantiate(buttonPrefab, buttonContainer);
            itemBtn.GetComponentInChildren<TMP_Text>().text = $"🎁 Give {item.Name} to {target.profileData.Name}";
            Item captured = item;
            itemBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentPlayer.GiveItem(target, captured);
                Hide();
            });
        }

        GameObject cancelBtn = Instantiate(buttonPrefab, buttonContainer);
        cancelBtn.GetComponentInChildren<TMP_Text>().text = "❌ Cancel";
        cancelBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Show(currentPlayer, GameplayManager.Instance.Players);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearButtons();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);
    }
}
