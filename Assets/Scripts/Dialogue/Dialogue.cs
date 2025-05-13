using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    public Action onDialogueEnd; // Action to be called when dialogue ends
    private bool dialogueFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        //StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !dialogueFinished) // Allow skipping to the next line
        {
            if (textComponent.text == lines[index])
            {
                StopCoroutine(AutoNextLine());
                NextLine();
            }
            else
            {
                //StopCoroutine(TypeLine()); 
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        dialogueFinished = false;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            if (textComponent.text == lines[index])
            {
                StartCoroutine(AutoNextLine());
                yield break; // Stop typing if the text is already complete
            }
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        StartCoroutine(AutoNextLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty; // Clear text when dialogue ends
            StopAllCoroutines(); 
            onDialogueEnd.Invoke(); // Invoke the action if it's set
            dialogueFinished = true;
        }
    }
    private IEnumerator AutoNextLine()
    {
        yield return new WaitForSeconds(0.8f);
        NextLine();
    }
}
