using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    public const float ANIM_DURATION = 1.5f;
    [SerializeField] private Animator dice1;
    [SerializeField] private Animator dice2;
    [SerializeField] private Animator dice3; // bonus dice
    [SerializeField] private bool bonusDiceActive = false;
    public bool BonusDiceActive
    {
        get { return bonusDiceActive; }
        set { bonusDiceActive = value; }
    }
    [SerializeField] private GameObject instance;


    public void startRoll()
    {
        dice1.SetTrigger("DoRoll");
        dice2.SetTrigger("DoRoll");
        if (bonusDiceActive)
        {
            dice3.SetTrigger("DoRoll");
        }
    }
    public void setResult(int result1,int result2)
    {
        dice1.SetFloat("Result",result1);
        dice2.SetFloat("Result",result2);
    }
    public void setResult(int result1, int result2,int result3)
    {
        setResult(result1, result2);
        dice3.SetFloat("Result", result3);
    }
    public void hideDices()
    {
        dice1.SetTrigger("Idle");
        dice2.SetTrigger("Idle");
        dice3.SetTrigger("Idle");
        dice1.gameObject.SetActive(false);
        dice2.gameObject.SetActive(false);
        dice3.gameObject.SetActive(false);
    }
    public void showDices()
    {
        dice1.gameObject.SetActive(true);
        dice2.gameObject.SetActive(true);
        if (bonusDiceActive)
        {
            dice3.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (dice1 == null || dice2 == null || dice3 == null)
        {
            Debug.LogError("One or more dice animators are not assigned in the Inspector.");
        }
        hideDices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
