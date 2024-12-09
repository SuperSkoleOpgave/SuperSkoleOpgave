using UnityEngine;

public class SkyAtmosphere : MonoBehaviour
{

    public LoopObjecPool objecPool;


    // Makes every object affected by this script fly towards screen.
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position += Vector3.back * objecPool.speed * Time.deltaTime;
        }
        
    }
}
