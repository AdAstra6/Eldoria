using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnscript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject gameBoard;
    public Sprite[] images;
    bool unplayed = true; 


    private void Start(){
        spriteRenderer.sprite = null;
    }

    private void OnMouseDown(){
        if(unplayed){

         int index = gameBoard.GetComponent<XO_game_Script>().PlayerTurn();
         spriteRenderer.sprite = images[index];
         unplayed = false; // this is to make sure the player can only play once in a box
        }
  
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameBoard = GameObject.Find("xo_background");
    }
}


