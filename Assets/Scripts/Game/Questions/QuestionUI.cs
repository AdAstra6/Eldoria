using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestionUI : MonoBehaviour
{
    public static QuestionUI Instance;

    [Header("References")]
    [SerializeField] private QuestionManager questionManager;

    [Header("UI Elements")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] optionTexts;

    [Header("Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    private Question currentQuestion;
    private Player currentPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeButtons();
        }
        else Destroy(gameObject);
    }

    void InitializeButtons()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void ShowQuestion(Player player)
    {
        currentPlayer = player;
        questionPanel.SetActive(true);
        LoadQuestion();
    }

    private void LoadQuestion()
    {
        currentQuestion = questionManager.GetRandomQuestion();

        if (currentQuestion == null)
        {
            Debug.LogError("No questions available!");
            return;
        }

        questionText.text = currentQuestion.text;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (i < currentQuestion.options.Length)
            {
                optionTexts[i].text = currentQuestion.options[i];
                optionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ProcessAnswerVisuals(int selectedIndex, bool isCorrect)
    {
        // Disable all buttons
        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }

        // Color selected button
        Image selectedButtonImage = optionButtons[selectedIndex].GetComponent<Image>();
        selectedButtonImage.color = isCorrect ? correctColor : wrongColor;

        // If wrong, show correct answer
        if (!isCorrect)
        {
            Image correctButtonImage = optionButtons[currentQuestion.correctAnswer].GetComponent<Image>();
            correctButtonImage.color = correctColor;
        }

        // Wait before hiding
        yield return new WaitForSeconds(2f);

        GameQuestionManager.Instance.HandleAnswer(currentPlayer, isCorrect);
        HideQuestion();
    }

    public void HideQuestion()
    {
        questionPanel.SetActive(false);
        ClearQuestion();

        // Reset all button colors
        foreach (Button button in optionButtons)
        {
            button.GetComponent<Image>().color = defaultColor;
            button.interactable = true;
        }
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        bool isCorrect = selectedIndex == currentQuestion.correctAnswer;
        // ACTUALLY START THE COROUTINE
        StartCoroutine(ProcessAnswerVisuals(selectedIndex, isCorrect));
    }

    private void ClearQuestion()
    {
        questionText.text = "";
        foreach (var text in optionTexts) text.text = "";
        currentQuestion = null;
        currentPlayer = null;
    }
}