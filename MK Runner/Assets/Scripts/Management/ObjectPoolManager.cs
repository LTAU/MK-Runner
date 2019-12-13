using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    //Object pooling managment, especially useful for respawning objects frequently such as platforms and obstacles

    public static ObjectPoolManager singleton;

    public List<ObjectPool> pools = new List<ObjectPool>();

    public int numberOfCopies;

    public void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);
    }

    public int CreatePool(GameObject go)
    {
        ObjectPool instance = new ObjectPool(go);
        for (int i = 0; i <= numberOfCopies; i++)
        {
            GameObject goInstance = Instantiate(go, transform);
            goInstance.SetActive(false);
            instance.pool.Enqueue(goInstance);
        }

        pools.Add(instance);

        return pools.Count - 1;
    }


    public GameObject SpawnObject(int poolToPullFrom )
    {



        GameObject go = pools[poolToPullFrom].pool.Dequeue();

        if (go.activeSelf)
        {
            pools[poolToPullFrom].pool.Enqueue(go);
            go = Instantiate(pools[poolToPullFrom].prefab, transform);
            pools[poolToPullFrom].pool.Enqueue(go);
            
            return go;
        }
        else
        {
            pools[poolToPullFrom].pool.Enqueue(go);
            go.SetActive(true);
           

            return go;
        }

    }




}

public class ObjectPool
{
    public GameObject prefab;
    public Queue<GameObject> pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }
}

