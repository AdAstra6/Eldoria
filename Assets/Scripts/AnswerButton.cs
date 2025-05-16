using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public bool isCorrectAnswer; // Set this in the Inspector for the correct button
    [SerializeField] private Button button;
    public Button Button { get { return button; } set { button = value; } }
    [SerializeField] private TMP_Text Text; // Assign via Inspector
    public TMP_Text TextAnswer { get { return Text; } set { Text = value; } }
    [SerializeField] private Image image;
    public Image Image { get { return image; } set { image = value; } }

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        //button.onClick.AddListener(CheckAnswer);
    }

    private void CheckAnswer()
    {
        if (isCorrectAnswer)
        {
            image.color = Color.green; // Turns green if correct
        }
        else
        {
            image.color = Color.red; // Turns red if wrong
        }
    }
}
