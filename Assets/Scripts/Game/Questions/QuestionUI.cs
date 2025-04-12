using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        
        if(currentQuestion == null)
        {
            Debug.LogError("No questions available!");
            return;
        }

        questionText.text = currentQuestion.text;
        
        for(int i = 0; i < optionTexts.Length; i++)
        {
            if(i < currentQuestion.options.Length)
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

    public void HideQuestion()
    {
        questionPanel.SetActive(false);
        ClearQuestion();
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        bool isCorrect = selectedIndex == currentQuestion.correctAnswer;
        GameQuestionManager.Instance.HandleAnswer(currentPlayer, isCorrect);
    }

    private void ClearQuestion()
    {
        questionText.text = "";
        foreach(var text in optionTexts) text.text = "";
        currentQuestion = null;
        currentPlayer = null;
    }
}