using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XO_game_Script : MonoBehaviour
{
    int spriteIndex = -1;

    public int PlayerTurn(){
        spriteIndex++;
       return spriteIndex % 2 ; // i am determinign the turn so i can decide the sprite between o and X 

    }
}
