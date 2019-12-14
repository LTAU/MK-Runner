using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Script to control the UI
    public Text title;
    public Text score;
    public Text highScore;
    public GameObject menuUI;
    public Text info;

    private void Start()
    {
        GameEvents.OnScoreChanged += UpdateScore;
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnInfoText += UpdateInfo;
    }

   

    private void UpdateScore(int value)
    {
        score.text = value.ToString();
        if (value > GameEvents.HIGHSCORE)
        {
            GameEvents.HIGHSCORE = value;
            highScore.text = value.ToString();
        }
    }



    public void PlayPressed()
    {
        GameEvents.InvokeGameStart();
    }

    public void QuitPress()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void UpdateInfo(string _text)
    {
        info.gameObject.SetActive(true);
        info.text = _text;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3f);
        info.gameObject.SetActive(false);

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
