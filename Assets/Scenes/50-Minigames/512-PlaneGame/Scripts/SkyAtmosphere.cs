using UnityEngine;

public class SkyAtmosphere : MonoBehaviour
{
    [SerializeField]
    public float speed = 3f;

    // Makes every object affected by this script fly towards screen.
    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
