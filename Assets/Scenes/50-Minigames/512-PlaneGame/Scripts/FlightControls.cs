using System.Collections;

using UnityEngine;

public class FlightControls : MonoBehaviour
{

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float rotationDuration = 2f;
    private bool isSpinning = false;

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
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed * Time.deltaTime;

        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, -10f, 10f);
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 7f);

        transform.position = newPosition;

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
        float startRotationZ = playerObject.transform.rotation.eulerAngles.x;
        float targetRotationZ = startRotationZ + 360f;

        while (timeTaken < rotationDuration)
        {

            float currentZRotation = Mathf.Lerp(startRotationZ, targetRotationZ, timeTaken / rotationDuration);
            playerObject.transform.rotation = Quaternion.Euler(0, 0, currentZRotation);

            timeTaken += Time.deltaTime;
            yield return null;
        }

        playerObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        isSpinning = false;

    }

}
