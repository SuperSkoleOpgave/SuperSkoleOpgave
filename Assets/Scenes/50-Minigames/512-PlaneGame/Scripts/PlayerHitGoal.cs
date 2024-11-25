using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitGoal : MonoBehaviour
{

    private PlaneGameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<PlaneGameManager>();

        if (gameManager == null)
        {
            Debug.LogError("cant find game manager");
        }
    }

    /// <summary>
    /// ResetGoalLoops that deactivate GoalLoops when the collide.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.CheckIfCorrect(gameObject);
            

        }
    }
}
