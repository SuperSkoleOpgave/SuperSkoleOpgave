using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePropellerSpin : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1000f;

    /// <summary>
    /// Spins the propeller on the plane
    /// </summary>
    void Update()
    {
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
