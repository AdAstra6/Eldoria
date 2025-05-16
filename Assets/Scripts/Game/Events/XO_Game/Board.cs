using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
[Header("Input Settings : ")]
[SerializeField] private LayerMask boxesLayerMask;
[SerializeField] private float touchRadius;

[Header("Mark Sprite : ")]
[SerializeField] private Sprite SpriteX;
[SerializeField] private Sprite SpriteO;

[Header ("Mark Color : ")]
[SerializeField] private Color colorX;
[SerializeField] private Color colorO;

public Mark[] marks ; 
private Camera cam; 
private Mark currentMark;
    private void Start(){
     cam = Camera.main; 
     currentMark = Mark.X; // Start with X
     marks = new Mark[9]; // Initialize the marks array
    }

    private void Update(){
        if (Input.GetMouseButtonUp(0)){
         Vector2 touchPos = cam.ScreenToWorldPoint(Input.mousePosition);
         Collider2D hit = Physics2D.OverlapCircle(touchPos, touchRadius, boxesLayerMask);
             if (hit)
            HitBox(hit.GetComponent <Box>());


        }
    }


    private void HitBox (Box box){
        if (!box.isMarked){
           marks [ box.index ] = currentMark; // Set the mark in the array
           box.SetAsMarked(GetSprite(), currentMark, Getcolor());

           SwitchPlayer(); 


        }
    }

    private void SwitchPlayer(){
        currentMark =  (currentMark==Mark.X) ? Mark.O : Mark.X; 
    }

    private Color Getcolor(){
        return currentMark == Mark.X ? colorX : colorO;
    }

     private Sprite GetSprite(){
        return currentMark == Mark.X ? SpriteX : SpriteO;
    }


}
