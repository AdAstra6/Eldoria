using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public bool isCorrectAnswer; // Set this in the Inspector for the correct button
    private Button button;
    private Image image;

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(CheckAnswer);
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
