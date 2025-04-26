using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DiceRoll : MonoBehaviour
{
    private Player currentPlayer;
    [SerializeField] private GameObject rollDiceButton;

    public Player CurrentPlayer { get { return currentPlayer; } set { currentPlayer = value; } }

    [SerializeField] private TMP_Text diceResultText; // Assign via Inspector
    [SerializeField] private DiceAnimation diceAnim;
    private void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }

    public IEnumerator RollDice()
    {
        int dice1 = UnityEngine.Random.Range(1, 7);
        int dice2 = UnityEngine.Random.Range(1, 7);
        diceAnim.showDices();
        diceAnim.startRoll();
        diceAnim.setResult(dice1, dice2);
        yield return new WaitForSeconds(DiceAnimation.ANIM_DURATION);
        int total = dice1 + dice2;
        if (currentPlayer.HasBonusDiceNextTurn)
        {
            total += UnityEngine.Random.Range(1, 7); // Add bonus dice roll
            currentPlayer.HasBonusDiceNextTurn = false; // Reset bonus dice for next turn
        }
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
        yield return new WaitUntil(() => currentPlayer.PlayerState == PlayerStats.END_MOVING); 
        hideAfterAnim();
    }
    public void StartRollDiceCorroutine()
    {
        StartCoroutine(RollDice());
    }
    public void hideAfterAnim()
    {

        diceAnim.hideDices();
        rollDiceButton.SetActive(false);
    }
    public void showBeforeAnim()
    {
        diceAnim.showDices();
        rollDiceButton.SetActive(true);
    }
}