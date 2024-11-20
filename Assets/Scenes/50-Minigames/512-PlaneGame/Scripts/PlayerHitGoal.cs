using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitGoal : MonoBehaviour
{
    [SerializeField]
    private Material playerContactMat;

    [SerializeField]
   private PlaneGameManager gameManager;

    /// <summary>
    /// ResetGoalLoops that deactivate GoalLoops when the collide.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.CheckIfCorrect(gameObject);
            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = playerContactMat;

        }
    }
}
