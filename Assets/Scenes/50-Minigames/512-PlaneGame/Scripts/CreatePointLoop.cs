using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

public class CreatePointLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnPointA, spawnPointB, spawnPointC, spawnPointD;

    

    [SerializeField]
    private PlaneGameManager planeGameManager;

    [SerializeField]
    private PlaneGameController planeGameController;

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

           // loopObject.transform.Find("Sign/Canvas").GetComponent<GoalLoop>().GetLetter(planeGameController.CurrentLetter().ToString());
            loopObject.transform.position = spawnPointA.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {


            loopObject.transform.position = spawnPointB.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {


            loopObject.transform.position = spawnPointC.transform.position;
            loopObject.SetActive(true);
        }

        loopObject = objectLoopPool.GetPooledObject();

        if (loopObject != null)
        {


            loopObject.transform.position = spawnPointD.transform.position;
            loopObject.SetActive(true);
        }
    }

    /// <summary>
    /// Waits x amount of seconds...
    /// </summary>
    IEnumerator WaitForSeconds()
    {

        while (planeGameManager.isGameOn)
        {


            // Wait for 8 seconds
            yield return new WaitForSeconds(8);

            if (isOn)
            {
                CreatePointLoops();
            }
        }
    }
}
