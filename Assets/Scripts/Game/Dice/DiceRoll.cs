using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DiceRoll : MonoBehaviour
{
    public static DiceRoll Instance { get; private set; }
    private Player currentPlayer;
    [SerializeField] private GameObject rollDiceButton;

    public Player CurrentPlayer { get { return currentPlayer; } set { currentPlayer = value; } }

    //[SerializeField] private TMP_Text diceResultText; 
    [SerializeField] private DiceAnimation diceAnim;

    private void Awake()
    {
        Instance = this;
        currentPlayer = null;
    }
    private void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    }

    public IEnumerator RollDice()
    {
        // Play the dice roll sound
        AudioManager.Instance.PlayDiceRoll();
        bool bonusDiceActive = currentPlayer.Effects.HasBonusDiceNextTurn;
        diceAnim.BonusDiceActive = bonusDiceActive; // Set bonus dice active if the player has it
        currentPlayer.Effects.HasBonusDiceNextTurn = false; // Reset bonus dice effect after use
        int dice1 = UnityEngine.Random.Range(1, 7);
        int dice2 = UnityEngine.Random.Range(1, 7);
        int dice3 = 0; // bonus dice
        diceAnim.showDices();
        diceAnim.startRoll();
        if (bonusDiceActive)
        {
            dice3 = UnityEngine.Random.Range(1, 7);
            diceAnim.setResult(dice1, dice2, dice3);
        }
        else
        {
            diceAnim.setResult(dice1, dice2);
        }
        yield return new WaitForSeconds(DiceAnimation.ANIM_DURATION);
        int total = dice1 + dice2 + dice3;
        Debug.Log("Rolled: " + total);

        // This text was used before the dice animation was added
        /*if (diceResultText != null)
        {
            diceResultText.text = total.ToString();
        }
        else
        {
            Debug.LogError("DiceResultText Text component not assigned in the Inspector.");
        } */

        currentPlayer.Move(total);
        //yield return new WaitUntil(() => currentPlayer.PlayerState == PlayerStats.END_MOVING); 
        //hideAfterAnim();
    }
    public void StartRollDiceCorroutine()
    {
        rollDiceButton.SetActive(false);
        StartCoroutine(RollDice());
    }
    public void hideAfterAnim()
    {
        
        diceAnim.hideDices();
        rollDiceButton.SetActive(false);
    }
    public void showBeforeAnim()
    {
        diceAnim.BonusDiceActive = currentPlayer.Effects.HasBonusDiceNextTurn;
        diceAnim.showDices();
        rollDiceButton.SetActive(true);
    }
}