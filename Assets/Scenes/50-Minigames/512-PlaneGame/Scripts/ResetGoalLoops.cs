using UnityEngine;

public class ResetGoalLoops : MonoBehaviour
{
    [SerializeField]
    private Material defaultContactMat;

    [SerializeField]
    private PlaneGameManager planeManager;

    /// <summary>
    /// ResetGoalLoops that deactivate GoalLoops when the collide.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoalLoop"))
        {

            other.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultContactMat;
            other.gameObject.SetActive(false);
            planeManager.resetLoop = true;
            
        }

        if (other.gameObject.CompareTag("Enviorment"))
        {

            other.gameObject.SetActive(false);
            planeManager.resetCloud = true;

        }
    }


}
