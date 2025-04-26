using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] public static UIManager Instance;
    public GameObject pathSelectionPanel; // The panel that appears at crossways
    public Transform buttonParent; // Where buttons will be placed
    public Button pathButtonPrefab; // The prefab for each path choice
    private Player currentPlayer;

    [SerializeField] DiceRoll dice;
    private Button rollDiceButton;
    private Button endTurnButton;

    [SerializeField] private TMP_Text penaltyText;
    [SerializeField] private GameObject penaltyTileEffects;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            Debug.Log("UIManager initialized.");
        }
        else
        {
            Debug.LogError("Multiple UIManager instances detected!");
            Destroy(gameObject);
        }
        rollDiceButton = GameObject.Find("RollDiceButton").GetComponent<Button>();
        rollDiceButton.gameObject.SetActive(false);
        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        endTurnButton.gameObject.SetActive(false);

        pathSelectionPanel.SetActive(false); // Hide the panel at start

    }


    public void DisplayPathChoices(List<Tile> options, Player player)
    {
        if (pathSelectionPanel == null) { Debug.LogError("pathSelectionPanel is NULL!"); return; }
        if (buttonParent == null) { Debug.LogError("buttonParent is NULL!"); return; }
        if (pathButtonPrefab == null) { Debug.LogError("pathButtonPrefab is NULL!"); return; }

        Debug.Log("Path Options Available: " + options.Count);

        pathSelectionPanel.SetActive(true);
        currentPlayer = player;

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Tile option in options)
        {
            Debug.Log("Creating Button for: " + option.name);
            Button btn = Instantiate(pathButtonPrefab, buttonParent);
            btn.GetComponentInChildren<TMP_Text>().text = option.name;
            btn.onClick.AddListener(() => SelectPath(option));
        }
    }


    public void SelectPath(Tile selectedTile)
    {
        pathSelectionPanel.SetActive(false); // Hide the panel
        currentPlayer.ChoosePath(selectedTile); // Move the player
    }

    // Roll Dice button methods
    public void RollDiceButtonShow()
    {
        dice.showBeforeAnim();
    }
    public void RollDiceButtonHide()
    {
        dice.hideAfterAnim();
    }

    // End Turn button methods
    public void EndTurnButtonShow()
    {
        endTurnButton.gameObject.SetActive(true);
    }
    public void EndTurnButtonHide()
    {
        endTurnButton.gameObject.SetActive(false);
    }

    public void ShowPenaltyText(Player player , int stepBack)
    {
        string message = player.profileData.Name + " felt in penalty tyle! \n has to go back " + stepBack + " steps!";
        penaltyText.text = message;
        penaltyTileEffects.SetActive(true);
    }
    public void HidePenaltyText()
    {
       penaltyTileEffects.SetActive(false);

    }

}


