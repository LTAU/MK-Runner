using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platforms
    , hazards
    , obstacles;

    [SerializeField]
    private GameObject item;

    [SerializeField]
    private Transform obstacleParent
    , platformParent
    , hazardParent;

    [SerializeField]
    public float offset;
    private int[] platformPools
    , obstaclePools
    , hazardPools;

    private int itemPool;
    private bool generateNewPlatforms = true;
    private float 
      platformTimer
    , obstacleTimer
    , itemTimer
    , hazardTimer
    , spawnChance
    , platformLength
    , currentHeight;

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
        offset = -2f;
        //initialise the first three platforms and generate spawning offset so that future platforms spawn offstage
        offset += GeneratePlatform() + 1;
        offset += GeneratePlatform() + 1;
        offset += GeneratePlatform();
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
            if (spawnChance <= .50f && spawnChance> .2f && obstacleTimer<=0)
            {
                GenerateObstacle();
                obstacleTimer = 2f;
            }
            else if (spawnChance <= .05f && spawnChance > .025f && hazardTimer <= 0)
            {
                GenerateHazardousObstacle();
                hazardTimer = 4f;
            }
            else if (spawnChance <= .1f && itemTimer <= 0)
            {
                GenerateItem();
                itemTimer = 3f;
            }
            yield return new WaitForSeconds(0.1f);
            platformTimer -= .1f;
            obstacleTimer -= .1f;
            hazardTimer -= .1f;
            itemTimer -= .1f;
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
        return (Random.Range(.25f, 1.25f)) / GameManager.singleton.currentGameSpeed;
    }

    private float GeneratePlatform()
    {
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPools[Random.Range(0,platformPools.Length)]);
        plat.transform.position = new Vector3(offset
            , currentHeight);
        plat.transform.SetParent(platformParent);
        return plat.GetComponent<BoxCollider2D>().size.x;
    }

    private void GenerateHazardousObstacle()
    {
        GameObject haz = ObjectPoolManager.singleton.SpawnObject(hazardPools[Random.Range(0, hazardPools.Length)]);
        haz.transform.position = new Vector3(offset
            , currentHeight);
        haz.transform.SetParent(hazardParent);
    }

    private void GenerateObstacle()
    {
        GameObject obs = ObjectPoolManager.singleton.SpawnObject(obstaclePools[Random.Range(0, obstaclePools.Length)]);
        obs.transform.position = new Vector3(offset
            , currentHeight);
        obs.transform.SetParent(obstacleParent);
    }

    private void GenerateItem()
    {
        GameObject itm = ObjectPoolManager.singleton.SpawnObject(itemPool);
        itm.transform.position = new Vector3(offset, currentHeight);
        itm.transform.SetParent(obstacleParent);
    }

    private void OnPlayerDeath()
    {
        generateNewPlatforms = false;
    }
}
