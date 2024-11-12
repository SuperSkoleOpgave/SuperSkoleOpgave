using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAtmosphere : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    // Makes every object affected by this script fly left.
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
