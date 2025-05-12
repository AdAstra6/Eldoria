using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

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

    public bool isMotherTree = false; // allah ghalb 5/12/2025
    /*
        integrate new ui by Adlen
     */
    [SerializeField] private AnswerButton[] answerButtons;
    [SerializeField] private GameObject scrollUI;
    [SerializeField] private FadeScript fadeScroll;
    [SerializeField] private TimerCountDown timerCountDown;


    [Header("Riddles")]
    [SerializeField] private RiddleManager riddleManager;
    private Riddle currentRiddle;



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
    void SetButtonsListnersForQuestions()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].Button.onClick.RemoveAllListeners();
            answerButtons[i].Button.onClick.AddListener(() => OnAnswerSelected(index));
        }
    }
    public void RemoveTimerEndListeners()
    {
        timerCountDown.TimerEnd = null;
    }

    public void ShowQuestion(Player player)
    {
        SetButtonsListnersForQuestions();
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


    public void SetRiddle(Riddle riddle)
    {
        currentRiddle = riddle;
    }

    public void ShowRiddle(Player player)
    {
        RemoveTimerEndListeners();
        timerCountDown.TimerEnd += () => StartCoroutine(TimeFinishedRiddle());
        currentPlayer = player;

        if (currentRiddle == null)
        {
            Debug.LogError("No riddle assigned!");
            return;
        }

        questionPanel.SetActive(true);
        fadeScroll.StartFadeIn();

        LoadRiddle();
    }
    private void LoadRiddle()
    {
        questionText.text = currentRiddle.text;

        // Assign riddle answers to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentRiddle.options.Length)
            {
                answerButtons[i].TextAnswer.text = currentRiddle.options[i];
                int index = i;
                answerButtons[i].Button.onClick.RemoveAllListeners(); // Clear previous listeners
                answerButtons[i].Button.onClick.AddListener(() =>
                {
                    bool isCorrect = index == currentRiddle.correctAnswer;
                    StartCoroutine(ProcessRiddleAnswerVisuals(index, isCorrect));
                });
            }
        }
    }
    IEnumerator ProcessRiddleAnswerVisuals(int selectedIndex, bool isCorrect)
    {
        foreach (AnswerButton button in answerButtons)
        {
            button.Button.interactable = false;
        }

        var selectedButtonImage = answerButtons[selectedIndex].Image;
        selectedButtonImage.color = isCorrect ? correctColor : wrongColor;

        if (!isCorrect)
        {
            var correctButtonImage = answerButtons[currentRiddle.correctAnswer].Image;
            correctButtonImage.color = correctColor;
        }

        if (isCorrect)
            AudioManager.Instance.PlayCorrectAnswer();
        else
            AudioManager.Instance.PlayWrongAnswer();

        yield return new WaitForSeconds(2f);
        if (isMotherTree)
        {
            MotherTreeManager.instance.EndMotherTreeEvent(isCorrect);
        }
        else
        {
            GameQuestionManager.Instance.HandleAnswer(currentPlayer, isCorrect);
        }
        HideQuestion(); // reuse existing hide logic
    }
    public IEnumerator TimeFinishedRiddle()
    {
        if (currentRiddle != null)
        {
            timeFinished.SetActive(true);
            foreach (AnswerButton button in answerButtons)
            {
                button.Button.interactable = false;
            }

            var correctButtonImage = answerButtons[currentRiddle.correctAnswer].Image;
            correctButtonImage.color = correctColor;
        }

        yield return new WaitForSeconds(3f);
        if (!isMotherTree)
        {
            GameQuestionManager.Instance.HandleAnswer(currentPlayer, false);
        }
        else
        {
            MotherTreeManager.instance.EndMotherTreeEvent(false);
        }
        HideQuestion();
    }
}