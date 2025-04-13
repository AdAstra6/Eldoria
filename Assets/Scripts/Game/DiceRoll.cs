using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DiceRoll : MonoBehaviour
{
    private Player currentPlayer;
    public Player CurrentPlayer { get { return currentPlayer; } set { currentPlayer = value; } }

    [SerializeField] private TMP_Text diceResultText; // Assign via Inspector

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void RollDice()
    {
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int total = dice1 + dice2;
        Debug.Log("Rolled: " + total);

        if (diceResultText != null)
        {
            diceResultText.text = total.ToString();
        }
        else
        {
            Debug.LogError("DiceResultText Text component not assigned in the Inspector.");
        }

        currentPlayer.Move(total); 
    }
}