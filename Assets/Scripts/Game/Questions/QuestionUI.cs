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
    [SerializeField] private GameObject timeFinished;

    [Header("Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField] private Question currentQuestion;
    public Question CurrentQuestion
    {
        get { return currentQuestion; }
        set { currentQuestion = value; }
    }
    private Player currentPlayer;

    /*
        integrate new ui by Adlen
     */
    [SerializeField] private AnswerButton[] answerButtons;
    [SerializeField] private GameObject scrollUI;
    [SerializeField] private FadeScript fadeScroll;
    [SerializeField] private TimerCountDown timerCountDown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeButtons();
           
            questionPanel.SetActive(false);
            timeFinished.SetActive(false);
        }
        else Destroy(gameObject);
    }

    void InitializeButtons()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].Button = answerButtons[i].GetComponent<Button>(); // Update to new UI
            answerButtons[i].Button.onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void RemoveTimerEndListeners()
    {
        timerCountDown.TimerEnd = null;
    }

    public void ShowQuestion(Player player)
    {
        RemoveTimerEndListeners(); // Remove all existing listeners before adding a new one  
        timerCountDown.TimerEnd += () => StartCoroutine(this.TimeFinished());
        currentPlayer = player;
        LoadQuestion();
        questionPanel.SetActive(true);
        fadeScroll.StartFadeIn();
    }

    private void LoadQuestion()
    {
        //currentQuestion = questionManager.GetRandomQuestion(); // Question loaded in GameQuestionManager

        if (currentQuestion == null)
        {
            Debug.LogError("No questions available!");
            return;
        }

        questionText.text = currentQuestion.text;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.options.Length)
            {
                answerButtons[i].TextAnswer.text = currentQuestion.options[i]; // Update new UI text

            }
        }
    }
    public  void ShowRiddle()
    {
        
    }
    IEnumerator ProcessAnswerVisuals(int selectedIndex, bool isCorrect)
    {
        foreach (AnswerButton button in answerButtons)
        {
            button.Button.interactable = false;
        }

        // Color selected button
        Image selectedButtonImage = answerButtons[selectedIndex].Image; // Update to new UI
        selectedButtonImage = answerButtons[selectedIndex].Image; // Update to new UI

        selectedButtonImage.color = isCorrect ? correctColor : wrongColor;

        // If wrong, show correct answer
        if (!isCorrect)
        {
            Image correctButtonImage = answerButtons[currentQuestion.correctAnswer].Image; // Update to new UI
            correctButtonImage.color = correctColor;
        }
        // Play sound based on answer
        if (isCorrect)
        {
            AudioManager.Instance.PlayCorrectAnswer();
        }
        else
        {
            AudioManager.Instance.PlayWrongAnswer();
        }

        // Wait before hiding
        yield return new WaitForSeconds(2f);

        GameQuestionManager.Instance.HandleAnswer(currentPlayer, isCorrect);
        HideQuestion();
    }

    public void HideQuestion()
    {
        // Hide the question panel
        fadeScroll.StartFadeOut();
        //fadeScroll.disableAnimation();
        questionPanel.SetActive(false);

        ClearQuestion();

        // Reset all button colors
        foreach (AnswerButton button in answerButtons)
        {
            button.Button.GetComponent<Image>().color = defaultColor;
            button.Button.interactable = true;
        }
        timeFinished.SetActive(false);
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
        currentQuestion = null;
        currentPlayer = null;
    }

    public IEnumerator TimeFinished()
    {
        if (currentQuestion != null)
        {
            timeFinished.SetActive(true);
            foreach (AnswerButton button in answerButtons)
            {
                button.Button.interactable = false;
            }
            Image correctButtonImage = answerButtons[currentQuestion.correctAnswer].Image; // Update to new UI
            correctButtonImage.color = correctColor;
        }
        yield return new WaitForSeconds(3f);
        GameQuestionManager.Instance.HandleAnswer(currentPlayer, false);
        HideQuestion();
    }
}