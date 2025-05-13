using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MotherTreeManager : MonoBehaviour
{
    public static MotherTreeManager instance;

    [SerializeField] private GameObject motherTreePanel;
    [SerializeField] private Dialogue dialogue;
    Player currentPlayer;
    [Header("Reward Panel")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TMP_Text rewardUpperText;
    [SerializeField] private TMP_Text rewardLowerText;
    [SerializeField] private Image rewardImage;

    private void Awake()
    {
        instance = transform.GetComponent<MotherTreeManager>();
        dialogue.onDialogueEnd = null;
        dialogue.onDialogueEnd += () => AskRiddle(currentPlayer);
        HideMotherTreePanel();
        HideRewardPanel();
    }
    public void ShowMotherTreePanel()
    {
        motherTreePanel.SetActive(true);
    }
    public void HideMotherTreePanel()
    {
        motherTreePanel.SetActive(false);
    }
    public void StartMotherTreeEvent(Player player)
    {
        ShowMotherTreePanel();
        dialogue.StartDialogue();
        currentPlayer = player;
    }
    public void AskRiddle(Player player)
    {
        QuestionUI.Instance.isMotherTree = true;
        GameQuestionManager.Instance.AskRiddle(player);
    }
    public void EndMotherTreeEvent(bool correct)
    {
        if (correct)
        {
            currentPlayer.Inventory.AddItem(ItemType.TOTME_OF_UNDYING, 1);
            StartCoroutine(ShowRewardPanel());
        } else
        {
            HideRewardPanel();
            HideMotherTreePanel();
        }
            //HideMotherTreePanel(); // The Panel is hided after the reward panel is shown
            currentPlayer.PlayerState = PlayerStats.FINISHED_EVENT;
    }
    private IEnumerator ShowRewardPanel()
    {
        rewardPanel.SetActive(true);
        rewardUpperText.text = "Now not even death can stop you";
        rewardLowerText.text = "You have received a Tome of Undying";
        rewardImage.sprite = ItemsTypeExtensioin.GetIcon(ItemType.TOTME_OF_UNDYING);
        yield return new WaitForSeconds(2.0f);
        HideRewardPanel();
        HideMotherTreePanel();
    }
    public void HideRewardPanel()
    {
        rewardPanel.SetActive(false);
    }
}
