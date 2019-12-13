using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] platforms;
    public GameObject hazards;

    private int[] platformPools;
    private bool generateNewPlatforms = true;
    private float platformLength;

    private void Start()
    {
        //initialise object pooling for gameobjects because they will get reused a  lot
        platformPools = new int[platforms.Length];
        for (int i = 0; i < platforms.Length; i++) {

            platformPools[i]= ObjectPoolManager.singleton.CreatePool(platforms[i]);
        }




        GameEvents.OnPlayerDeath += OnPlayerDeath;
        GameEvents.OnGameStart += OnGameStart;

    }

    private void OnGameStart()
    {

        StartCoroutine(Generate());

    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(2f);
        
        while (generateNewPlatforms)
        {

            platformLength = GeneratePlatform();
            yield return new WaitForSeconds(CalculateTime(platformLength));
        }

    }

    //This calculates how long to wait before the object has passed and a new one can be spawned. 
    //Also adds a random gap between 1 and 3 units
    private float CalculateTime(float length)
    {
        float x = length / GameManager.singleton.currentGameSpeed;
        x += (Random.Range(1f, 2.5f) )/ GameManager.singleton.currentGameSpeed;
        return x;
    }

    

    private float GeneratePlatform()
    {
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPools[Random.Range(0,platformPools.Length)]);
        plat.transform.position = new Vector3(10
            , Random.Range(-2f, 0f));
        GameEvents.InvokeSpeedIncrease();
        return plat.GetComponent<BoxCollider2D>().size.x;
    }

    private void GenerateHazardousObstacle()
    {

    }

    private void GenerateObstacle()
    {

    }

    private void GenerateItem()
    {

    }

    private void OnPlayerDeath()
    {
        generateNewPlatforms = false;
    }

}
