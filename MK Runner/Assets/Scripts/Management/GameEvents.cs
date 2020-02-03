using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{

    public static int HIGHSCORE = 0;

    //Handles ingame events
    public delegate void voidDelegate();
    public delegate void intDelegate(int i);
    public delegate void stringDelegate(string s);

    public static event voidDelegate OnGameOver;
    public static void InvokeGameOver()
    {
        OnGameOver.Invoke();
    }

    public static event voidDelegate OnPlayerDeath;
    public static void InvokePlayerDeath()
    {
        OnPlayerDeath.Invoke();
    }

    public static event voidDelegate OnSpeedIncrease;
    public static void InvokeSpeedIncrease()
    {
        OnSpeedIncrease.Invoke();
    }

    public static event voidDelegate OnGameStart;
    public static void InvokeGameStart()
    {
        OnGameStart.Invoke();
    }

    public static event intDelegate OnScoreChanged;
    public static void InvokeScoreChange(int i)
    {
        OnScoreChanged.Invoke(i);
    }

    public static event intDelegate OnMaxSpeedChanged;
    public static void InvokeMaxSpeedChange(int i)
    {
        OnScoreChanged.Invoke(i);
    }

    public static event stringDelegate OnInfoText;
    public static void InvokeOnInfoText(string s)
    {
        OnInfoText.Invoke(s);
    }
}
