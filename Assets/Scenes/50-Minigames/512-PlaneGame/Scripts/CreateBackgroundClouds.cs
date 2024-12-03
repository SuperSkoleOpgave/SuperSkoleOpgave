using UnityEngine;

public class CreateBackgroundClouds : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnPointA, spawnPointB, spawnPointC, spawnPointD, spawnPointE, spawnPointF;



    [SerializeField]
    private PlaneGameManager planeGameManager;

    [SerializeField]
    private PlaneGameController planeGameController;

    int randoNum = 0;

    [SerializeField]
    private LoopObjecPool objectLoopPool;
    // Creates clouds that fly in the background
    public void CreateCloud()
    {
        Debug.Log("CloudSpawn");
        randoNum = Random.Range(0, 5);

        GameObject loopObject = objectLoopPool.GetPooledObject();

        loopObject.transform.position = spawnPointA.transform.position;
        loopObject.SetActive(true);

        if (loopObject != null && randoNum == 0)
        {
            loopObject.transform.position = spawnPointA.transform.position;
            loopObject.SetActive(true);
        }
        if (loopObject != null && randoNum == 1)
        {
            loopObject.transform.position = spawnPointB.transform.position;
            loopObject.SetActive(true);
        }
        if (loopObject != null && randoNum == 2)
        {
            loopObject.transform.position = spawnPointC.transform.position;
            loopObject.SetActive(true);
        }
        if (loopObject != null && randoNum == 3)
        {
            loopObject.transform.position = spawnPointD.transform.position;
            loopObject.SetActive(true);
        }
        if (loopObject != null && randoNum == 4)
        {
            loopObject.transform.position = spawnPointE.transform.position;
            loopObject.SetActive(true);
        }
        if (loopObject != null && randoNum == 5)
        {
            loopObject.transform.position = spawnPointF.transform.position;
            loopObject.SetActive(true);
        }

    }
}
