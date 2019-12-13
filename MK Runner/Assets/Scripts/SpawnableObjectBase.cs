using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectBase : MonoBehaviour
{
    bool moving = true;

    private void Start()
    {
        GameEvents.OnPlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        if (moving)
        {
            transform.position -= new Vector3(GameManager.singleton.currentGameSpeed*Time.deltaTime, 0, 0);
            if (transform.position.x < -25)
            {
                Despawn();
            }
        }
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.SetParent(ObjectPoolManager.singleton.transform);

    }

    private void OnPlayerDeath()
    {
        moving = false;
    }
}
