using System.Collections;
using UnityEngine;

public class CreatePointLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnPointA;

    [SerializeField]
    private GameObject spawnPointB;


    [SerializeField]
    private LoopObjecPool objectLoopPool;


    public bool isOn = true;

    void Start()
    {
        StartCoroutine(WaitForSeconds());
    }

    /// <summary>
    /// Creates loop answers the player can fly through
    /// </summary>
    private void CreatePointLoops()
    {

        GameObject loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {


            loopObject.transform.position = spawnPointA.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {


            loopObject.transform.position = spawnPointB.transform.position;
            loopObject.SetActive(true);
        }
    }

    /// <summary>
    /// Waits x amount of seconds...
    /// </summary>
    IEnumerator WaitForSeconds()
    {
        while (true)
        {


            // Wait for 4 seconds
            yield return new WaitForSeconds(8);

            if (isOn)
            {
                CreatePointLoops();
            }
        }
    }
}
