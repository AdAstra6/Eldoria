using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects
{
    private bool hasBonusDiceNextTurn = false;
    public bool HasBonusDiceNextTurn
    {
        get { return hasBonusDiceNextTurn; }
        set { hasBonusDiceNextTurn = value; }
    }
    private bool healthProtection = false; // Set to false in gameplayManager in QuestionAnswered method
    public bool HasHealthProtection
    {
        get { return healthProtection; }
        set { healthProtection = value; }
    }

}
