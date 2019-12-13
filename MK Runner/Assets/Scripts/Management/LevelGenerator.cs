using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject platforms;
    public GameObject hazards;

    private int platformPool;
    private bool generateNewPlatforms = true;

    private void Start()
    {
        //initialise object pooling for gameobjects because they will get reused a  lot
        platformPool = ObjectPoolManager.singleton.CreatePool(platforms);
        GameEvents.OnPlayerDeath += OnPlayerDeath;
        StartCoroutine(Generate());

    }


    IEnumerator Generate()
    {
        yield return new WaitForSeconds(2f);
        
        while (generateNewPlatforms)
        {
            print(CalculateTime().ToString());

            GeneratePlatform();
            yield return new WaitForSeconds(CalculateTime());
        }

    }

    //This calculates how long to wait before the object has passed and a new one can be spawned. 
    //Also adds a random gap between 1 and 3 units
    private float CalculateTime()
    {
        float x = 6 / GameManager.singleton.currentGameSpeed;
        x += (Random.Range(1f, 3f) )/ GameManager.singleton.currentGameSpeed;
        return x;
    }

    

    private void GeneratePlatform()
    {
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPool);
        plat.transform.position = new Vector3(10
            , Random.Range(-2f, 0f));
        GameEvents.InvokeSpeedIncrease();
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
