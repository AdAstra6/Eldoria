using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class GameplayManager : MonoBehaviour
{
    public const int MAX_PLAYERS = 4;
    private readonly Map map;
    private readonly Player[] players = new Player[MAX_PLAYERS];
}
