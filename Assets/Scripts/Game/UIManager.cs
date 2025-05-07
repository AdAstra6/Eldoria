using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] public static UIManager Instance;
    public GameObject pathSelectionPanel; // The panel that appears at crossways
    [SerializeField] private Button[] selectPathButtons = new Button[System.Enum.GetNames(typeof(PlayerMovementDirection)).Length]; // 4 possible Directions = 4 Buttons uss : PlayerMovementDIrections enum
    private Image[] selectPathButtonsShadows = new Image[System.Enum.GetNames(typeof(PlayerMovementDirection)).Length];
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

        Debug.Log("Path Options Available: " + options.Count);


        SelectPathButtonsReset();
        pathSelectionPanel.SetActive(true);
        currentPlayer = player;

        

        foreach (Tile option in options)
        {
            Debug.Log("Button activated");
            Button btn = selectPathButtons[(int) option.MovementDirection];
            Image img = selectPathButtonsShadows[(int)option.MovementDirection]; // d l u r
            Color color = img.color;
            color.a = 0;
            img.color = color;
            //btn.GetComponentInChildren<TMP_Text>().text = option.name;
            btn.onClick.AddListener(() => SelectPath(option));
            btn.onClick.AddListener(() => ButtonClicked());
            btn.interactable = true;
            
        }
    }
    public void ButtonClicked()
    {
        Debug.Log("Clickkkkk");
    }
    private void Start()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(PlayerMovementDirection)).Length-1; i++)
        {
            UnityEngine.UI.Image img = selectPathButtons[i].GetComponent<Image>();
            selectPathButtonsShadows[i] = img;
        }
        SelectPathButtonsReset();
    }

    private void SelectPathButtonsReset()
    {

        for (int i = 0; i < System.Enum.GetNames(typeof(PlayerMovementDirection)).Length-1; i++)
        {
            selectPathButtons[i].interactable = false;
            selectPathButtons[i].onClick.RemoveAllListeners();
            Color color = selectPathButtonsShadows[i].color;
            color.a = 1;
            selectPathButtonsShadows[i].color = color;
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


