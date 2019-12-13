using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectBase : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position -= new Vector3(GameManager._instance.currentGameSpeed, 0, 0);
        if (transform.position.x < -25)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.SetParent(ObjectPoolManager.singleton.transform);

    }
}
