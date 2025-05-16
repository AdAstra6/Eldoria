using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentPlayer = 0;
    public int totalPlayers = 2; // Change this based on your game

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void EndTurn()
    {
        currentPlayer = (currentPlayer + 1) % totalPlayers;
        Debug.Log("Now it's Player " + (currentPlayer + 1) + "'s turn!");
    }
}
