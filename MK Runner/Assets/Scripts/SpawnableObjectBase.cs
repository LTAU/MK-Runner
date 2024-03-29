﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectBase : MonoBehaviour
{
    //Script for all objects the player can interact with
    private bool moving = true;

    private void Start()
    {
        GameEvents.OnPlayerDeath += OnPlayerDeath;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        Despawn();
    }

    private void Update()
    {
        if (moving)
        {
            transform.position -= new Vector3(GameManager.singleton.currentGameSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < -25 || transform.position.y < -10)
            {
                Despawn();
            }
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.SetParent(ObjectPoolManager.singleton.transform);
    }

    private void OnPlayerDeath()
    {
        moving = false;
    }

    private void OnEnable()
    {
        moving = true;
    }
}
