using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] platforms;
    public GameObject[] hazards;
    public GameObject[] obstacles;
    public GameObject item;

    public Transform obstacleParent;
    public Transform platformParent;
    public Transform hazardParent;


    private int[] platformPools;
    private int[] obstaclePools;
    private int[] hazardPools;
    private int itemPool;
    private int OFFSET = 10;

    private bool generateNewPlatforms = true;
    private float platformLength;
    private float currentHeight;

    private float platformTimer;
    private float spawnChance;
    

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

        hazardPools = new int[hazards.Length];
        for (int i = 0; i < hazards.Length; i++)
        {

            hazardPools[i] = ObjectPoolManager.singleton.CreatePool(hazards[i]);
        }

        itemPool = ObjectPoolManager.singleton.CreatePool(item);


        GameEvents.OnPlayerDeath += OnPlayerDeath;
        GameEvents.OnGameStart += OnGameStart;

    }

    private void OnGameStart()
    {
        generateNewPlatforms = true;
        platformTimer = 0f;
        StartCoroutine(Generate());

    }

    IEnumerator Generate()
    {
        
        
        while (generateNewPlatforms)
        {
            if (platformTimer <= 0f)
            {

                yield return new WaitForSeconds(CalculateGapTime());
                currentHeight = Random.Range(-1f, 1f);
                platformLength = GeneratePlatform();
                platformTimer = CalculateTime(platformLength);
            }
            spawnChance = Random.value;
            if (spawnChance <= .15f && spawnChance> .05f)
            {
                GenerateObstacle();

            }
            else if (spawnChance <= .05f && spawnChance > .025f)
            {
                GenerateHazardousObstacle();

            }
            else if (spawnChance <= .025f)
            {
                GenerateItem();
            }


            yield return new WaitForSeconds(0.1f);
            platformTimer -= .1f;

        }

    }

    //This calculates how long to wait before the object has passed and a new one can be spawned. 
    //Also adds a random gap between 1 and 3 units
    private float CalculateTime(float length)
    {
        return length / GameManager.singleton.currentGameSpeed; 
    }

    private float CalculateGapTime()
    {
        return (Random.Range(1f, 2f)) / GameManager.singleton.currentGameSpeed;
    }

    

    private float GeneratePlatform()
    {
        
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPools[Random.Range(0,platformPools.Length)]);
        plat.transform.position = new Vector3(OFFSET
            , currentHeight);
        plat.transform.SetParent(platformParent);
        return plat.GetComponent<BoxCollider2D>().size.x;
    }

    private void GenerateHazardousObstacle()
    {
        GameObject haz = ObjectPoolManager.singleton.SpawnObject(hazardPools[Random.Range(0, hazardPools.Length)]);
        haz.transform.position = new Vector3(OFFSET
            , currentHeight);
        haz.transform.SetParent(hazardParent);
    }

    private void GenerateObstacle()
    {
        GameObject obs = ObjectPoolManager.singleton.SpawnObject(obstaclePools[Random.Range(0, obstaclePools.Length)]);
        obs.transform.position = new Vector3(OFFSET
            , currentHeight);
        obs.transform.SetParent(obstacleParent);
        
    }

    private void GenerateItem()
    {
        GameObject itm = ObjectPoolManager.singleton.SpawnObject(itemPool);
        itm.transform.position = new Vector3(OFFSET
            , currentHeight);
        itm.transform.SetParent(obstacleParent);
    }

    private void OnPlayerDeath()
    {
        generateNewPlatforms = false;
    }

}
