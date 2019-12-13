using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject platforms;
    public GameObject hazards;

    private int platformPool;

    private void Start()
    {
        //initialise object pooling for gameobjects because they will get reused a  lot
        platformPool = ObjectPoolManager.singleton.CreatePool(platforms);
        StartCoroutine(Generate());
    }


    IEnumerator Generate()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(3f);
            GeneratePlatform();
        }

    }



    private void GeneratePlatform()
    {
        GameObject plat = ObjectPoolManager.singleton.SpawnObject(platformPool);
        plat.transform.position = new Vector3(25, Random.Range(-2f, 2f));
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



}
