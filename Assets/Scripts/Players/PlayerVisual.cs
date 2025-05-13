using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{

    [SerializeField] private Animator animator;
    private float movementAnimationDuration = 0.04f;
    public float MovementAnimationDuration
    {
        get { return movementAnimationDuration; }
        set { movementAnimationDuration = value; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMovementDirection(PlayerMovementDirection direction)
    {
        int directionIndex = direction switch
        {
            PlayerMovementDirection.DOWN => 0,
            PlayerMovementDirection.LEFT => 1,
            PlayerMovementDirection.UP => 2,
            PlayerMovementDirection.RIGHT => 3,
            _ => -1 // NONE or undefined
        };

        if (directionIndex >= 0)
        {
            animator.SetFloat("Direction", directionIndex);
        }

    }
    public void SetMoving(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }
    public static Sprite GetVisual(int n)
    { 
        string path = Path.Combine("Sprites", "bigCharacter", "Player"+n.ToString() );
        return Resources.Load<Sprite>(path);


    }
}
