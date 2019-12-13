using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;
    

    public float startingGameSpeed;
    [HideInInspector]
    public float currentGameSpeed;
    public int score;
    bool playerIsAlive = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (singleton == null)
        {

            singleton = this;
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
        GameEvents.OnSpeedIncrease += SpeedIncrease;
    }

   IEnumerator RunSession()
    {
        while (playerIsAlive)
        {
            score += 1;
            yield return new WaitForSeconds(.5f);


        }

        


    }

    private void SpeedIncrease()
    {
        currentGameSpeed += .01f;
        if (currentGameSpeed > .25)
        {
            currentGameSpeed = .25f;
        }
    }

    private void PlayerDeath()
    {
        playerIsAlive = false;
    }
}
