using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Script to control the UI
    public Text title;
    public Text score;
    public GameObject menuUI;

    private void Start()
    {
        GameEvents.OnScoreChanged += UpdateScore;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void UpdateScore(int value)
    {
        score.text = value.ToString();
    }



    public void PlayPressed()
    {
        GameEvents.InvokeGameStart();
    }

    public void QuitPress()
    {

    }

    private void OnGameOver()
    {
        title.text = "Game Over";
        EnableMenu();
    }

    private void EnableMenu()
    {
        menuUI.SetActive(true);
    }
}
