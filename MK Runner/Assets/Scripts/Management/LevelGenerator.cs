using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] platforms;
    public GameObject hazards;
    public GameObject[] obstacles;

    public Transform obstacleParent;
    public Transform platformParent;
    public Transform hazardParent;


    private int[] platformPools;
    private int[] obstaclePools;

    private bool generateNewPlatforms = true;
    private float platformLength;

    private void Start()
    {
        //initialise object pooling for gameobjects because they will get reused a  lot
        platformPools = new int[platforms.Length];
        for (int i = 0; i < platforms.Length; i++) {

            platformPools[i]= ObjectPoolManager.singleton.CreatePool(platforms[i]);
        }

        obstaclePools = new int[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++)
        {

            obstaclePools[i] = ObjectPoolManager.singleton.CreatePool(obstacles[i]);
        }




        GameEvents.OnPlayerDeath += OnPlayerDeath;
        GameEvents.OnGameStart += OnGameStart;

    }

    private void OnGameStart()
    {
        generateNewPlatforms = true;

        StartCoroutine(Generate());

    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds((Random.Range(1f, 2f)) / GameManager.singleton.currentGameSpeed);
        
        while (generateNewPlatforms)
        {

            platformLength = GeneratePlatform();
            yield return new WaitForSeconds(CalculateTime(platformLength)/2);
                            GenerateObstacle();
            yield return new WaitForSeconds(CalculateTime(platformLength)/2);
            yield return new WaitForSeconds(CalculateGapTime());
        }

    }

    //This calculates how long to wait before the object has passed and a new one can be spawned. 
    //Also adds a random gap between 1 and 3 units
    private float CalculateTime(float length)
    {
        return length / GameManager.singleton.currentGameSpeed; ;
    }

    private float CalculateGapTime()
    {
        return (Random.Range(1f, 2f)) / GameManager.singleton.currentGameSpeed;
    }

    

    private float GeneratePlatform()
    {
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPools[Random.Range(0,platformPools.Length)]);
        plat.transform.position = new Vector3(10
            , Random.Range(-2f, 0f));
        plat.transform.SetParent(platformParent);
        GameEvents.InvokeSpeedIncrease();
        return plat.GetComponent<BoxCollider2D>().size.x;
    }

    private void GenerateHazardousObstacle()
    {

    }

    private void GenerateObstacle()
    {
        GameObject obs = ObjectPoolManager.singleton.SpawnObject(obstaclePools[Random.Range(0, platformPools.Length)]);
        obs.transform.position = new Vector3(10
            , 3);
        obs.transform.SetParent(obstacleParent);
        
    }

    private void GenerateItem()
    {

    }

    private void OnPlayerDeath()
    {
        generateNewPlatforms = false;
    }

}
