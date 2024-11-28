using System.Collections;

using UnityEngine;

public class FlightControls : MonoBehaviour
{

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float tiltAngle = 15f;  
    [SerializeField]
    private float rotationDuration = 2f;

    private bool isSpinning = false;
    private float lastHorizontalInput = 0f;  

    private void Start()
    {
        if (playerObject == null)
        {
            Debug.LogError($"{nameof(FlightControls)}: Player object reference is required!");
        }
    }

    void Update()
    {
        if (playerObject != null)
        {
            ProcessFlightControls();
        }
    }


    /// <summary>
    /// Diffrent directions the player charecter can fly With WASD keys, as well as barrel roll via Spacebar.
    /// </summary>
    private void ProcessFlightControls()
    {
        Transform transform = playerObject.transform;

        float horizontal = Input.GetAxis("Horizontal");  // A/D
        float vertical = Input.GetAxis("Vertical");      // W/S 

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        Vector3 newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, -10f, 10f); 
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 7f);    
        transform.position = newPosition;

        float pitch = -vertical * tiltAngle;
        float roll = horizontal * tiltAngle;

        transform.rotation = Quaternion.Euler(roll, 90, -3 + pitch);

        if (horizontal != 0f)
        {
            lastHorizontalInput = -horizontal;
        }

        if (Input.GetKey(KeyCode.Space) && !isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinPlayer());
        }

    }

    /// <summary>
    /// Spins the player on the x axis 360 degrees during the courotine like a barrel roll
    /// </summary>
    /// <returns>player spin and then return to inital state.</returns>
    private IEnumerator SpinPlayer()
    {
        float timeTaken = 0f;
        float startRotationX = playerObject.transform.rotation.eulerAngles.x;
        float targetRotationX = startRotationX + (lastHorizontalInput > 0 ? -360f : 360f);

        while (timeTaken < rotationDuration)
        {

            float currentXRotation = Mathf.Lerp(startRotationX, targetRotationX, timeTaken / rotationDuration);
            playerObject.transform.rotation = Quaternion.Euler(currentXRotation, 90, -5);

            timeTaken += Time.deltaTime;
            yield return null;
        }

        playerObject.transform.rotation = Quaternion.Euler(0, 90, -5);

        isSpinning = false;

    }

}
