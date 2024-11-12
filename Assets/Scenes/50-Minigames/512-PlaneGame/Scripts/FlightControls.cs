using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private GameObject playerObject;

    private float speed = 5;
    private float rotationDuration = 2f;
    private bool isSpinning = false;
    


    void Update()
    {
        playerFlight();
    }


    /// <summary>
    /// diffrent direction the player charector can fly With WASD buttons, aswell as barrel roll via Spacebar.
    /// </summary>
    private void playerFlight()
    {
       
        if (Input.GetKey(KeyCode.W))
        {
            playerObject.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
   
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerObject.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

        }


        if (Input.GetKey(KeyCode.A))
        {
            playerObject.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

        }
        if (Input.GetKey(KeyCode.D))
        {
            playerObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        }

        if (Input.GetKey(KeyCode.Space) && !isSpinning)
        {
            StartCoroutine(SpinPlayer());
        }

    }

    /// <summary>
    /// spins the player on the x axis 360 degrees during the courotine like a barrel roll
    /// </summary>
    /// <returns>player spin and then return to inital state.</returns>
    private IEnumerator SpinPlayer()
    {
        float timeTaken = 0f;
        float startRotationX = playerObject.transform.rotation.eulerAngles.x;
        float targetRotationX = startRotationX + 360f;

        while (timeTaken < rotationDuration)
        {
            
            float currentXRotation = Mathf.Lerp(startRotationX, targetRotationX, timeTaken / rotationDuration);
            playerObject.transform.rotation = Quaternion.Euler(currentXRotation, 0, 0);

            timeTaken += Time.deltaTime;
            yield return null;
        }

        playerObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        isSpinning = false;

    }

}
