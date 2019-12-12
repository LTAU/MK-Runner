using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    //Handles ingame events

    public delegate void voidDelegate();

    public static event voidDelegate OnGameEnd;
    public static void InvokeGameEnd()
    {
        OnGameEnd.Invoke();
    }

}
