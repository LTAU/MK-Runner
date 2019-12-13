using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;
    

    public float startingGameSpeed;
    [HideInInspector]
    public float currentGameSpeed;
    public int score;
    bool playerIsAlive = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        currentGameSpeed = startingGameSpeed;
        GameEvents.OnPlayerDeath += PlayerDeath;
    }

   IEnumerator RunSession()
    {
        while (playerIsAlive)
        {
            score += 1;
            yield return new WaitForSeconds(.5f);


        }

        


    }


    private void PlayerDeath()
    {
        playerIsAlive = false;
    }
}
