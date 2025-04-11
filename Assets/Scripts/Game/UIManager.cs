using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject pathSelectionPanel; // The panel that appears at crossways
    public Transform buttonParent; // Where buttons will be placed
    public Button pathButtonPrefab; // The prefab for each path choice
    private Player currentPlayer;

    private Button rollDiceButton;
    private Button endTurnButton;

    // UI for MCQuestion
    public GameObject questionPanel; // Reference the UI Panel
    public TMP_Text questionText; // Question text field
    public Button correctButton;
    public Button[] wrongButton;
    public TMP_Text feedbackText; // Feedback text field

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
        questionPanel.SetActive(false); // Hide the question panel at start

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
        rollDiceButton.gameObject.SetActive(true);
    }
    public void RollDiceButtonHide()
    {
        rollDiceButton.gameObject.SetActive(false);
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



    public void ShowQuestionPanel(Player player)
    {
        questionPanel.SetActive(true); // Show UI Panel

        // Temporary hardcoded question & options (Replace this with actual question system)
        string question = "What is 2 + 2?";
        string[] options = { "4", "3", "5", "6" };  // First option is correct

        questionText.text = question;

        // Assign correct button text
        correctButton.GetComponentInChildren<TMP_Text>().text = options[0];
        correctButton.onClick.RemoveAllListeners();
        correctButton.onClick.AddListener(() => SubmitAnswer(player, true));

        // Assign wrong buttons text
        for (int i = 0; i < wrongButton.Length; i++)
        {
            wrongButton[i].GetComponentInChildren<TMP_Text>().text = options[i + 1]; // Skip first (correct) option
            wrongButton[i].onClick.RemoveAllListeners();
            wrongButton[i].onClick.AddListener(() => SubmitAnswer(player, false));
        }
    }



    private void SubmitAnswer(Player player, bool isCorrect)
    {

        // Show feedback message
        UIManager.Instance.ShowFeedback(isCorrect ? "Correct Answer!" : "Wrong Answer!");

        QuestionManager.Instance.HandleAnswer(player, isCorrect);
    }

    public void ShowFeedback(string message)
    {
        Debug.Log("ShowFeedback called with message: " + message);
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        StartCoroutine(HideFeedbackAfterDelay());
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.HideQuestionPanel();
    }
    public void HideQuestionPanel()
    {
        questionText.text = ""; // Clear question text
        feedbackText.text = ""; // Clear feedback text
        questionPanel.SetActive(false); // Hide UI Panel
    }

}


