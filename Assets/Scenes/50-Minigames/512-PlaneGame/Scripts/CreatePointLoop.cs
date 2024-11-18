using UnityEngine;

public class CreatePointLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnPointA, spawnPointB, spawnPointC, spawnPointD;



    [SerializeField]
    private PlaneGameManager planeGameManager;

    [SerializeField]
    private PlaneGameController planeGameController;

    int randoNum = 0;

    [SerializeField]
    private LoopObjecPool objectLoopPool;

    /// <summary>
    /// Creates loop answers the player can fly through
    /// </summary>
    public void CreatePointLoops()
    {
        randoNum = Random.Range(0, 3);
        

        GameObject loopObject = objectLoopPool.GetPooledObject();
        if (loopObject != null)
        {
            if (randoNum == 0)
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.CurrentLetter().ToString());
            }
            else
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.GetRandomLetter().ToString());
            }
            loopObject.transform.position = spawnPointA.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {
            if (randoNum == 1)
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.CurrentLetter().ToString());
            }
            else
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.GetRandomLetter().ToString());
            }

            loopObject.transform.position = spawnPointB.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {
            if (randoNum == 2)
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.CurrentLetter().ToString());
            }
            else
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.GetRandomLetter().ToString());
            }

            loopObject.transform.position = spawnPointC.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {
            if (randoNum == 3)
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.CurrentLetter().ToString());
            }
            else
            {
                loopObject.transform.GetComponentInChildren<GoalLoop>().GetLetter(planeGameController.GetRandomLetter().ToString());
            }

            loopObject.transform.position = spawnPointD.transform.position;
            loopObject.SetActive(true);
        }
    }
}
