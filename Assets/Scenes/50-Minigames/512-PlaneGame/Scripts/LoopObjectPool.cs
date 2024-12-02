using System.Collections.Generic;
using UnityEngine;

public class LoopObjecPool : MonoBehaviour
{

    public List<GameObject> pooledObjects = new List<GameObject>();

    public int amountToPool = 10;

    public float speed = 3f;

    public int amountSpawned;

    //[SerializeField]
    //private PlaneGameManager planeGameManager;

    [SerializeField]
    private GameObject loopPrefab;

    // Tells the list what kinda objects we want to pool.
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(loopPrefab, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Gets the object that should pool.
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                amountSpawned++;
                return pooledObjects[i];
            }
        }
        GameObject obj = Instantiate(loopPrefab, transform);
        obj.GetComponent<SkyAtmosphere>().objecPool = this;
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}
