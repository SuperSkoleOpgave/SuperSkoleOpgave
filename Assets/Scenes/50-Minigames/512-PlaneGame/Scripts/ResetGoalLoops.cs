using UnityEngine;

public class ResetGoalLoops : MonoBehaviour
{


    /// <summary>
    /// ResetGoalLoops that deactivate GoalLoops when the collide.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoalLoop"))
        {
            other.gameObject.SetActive(false);
            
        }
    }


}
