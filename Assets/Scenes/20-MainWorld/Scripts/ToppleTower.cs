using System.Collections;
using System.Collections.Generic;
using CORE;
using UnityEngine;

public class ToppleTower : MonoBehaviour
{

    public static bool topplingTower = false;

    public static bool toppled = false;

    private Vector3 destination;
    private float destinationRotation;
    private float initialDistance;
    private float initialRotation;

    [SerializeField]
    List<GameObject> bordersToBeRemoved;

    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private Transform destinationTransform;

    void Start()
    {
        destination = destinationTransform.localPosition;
        destinationRotation = destinationTransform.localEulerAngles.x;
        initialDistance = Vector3.Distance(transform.localPosition, destination);
        initialRotation = transform.localEulerAngles.x;
        if(!toppled && GameManager.Instance.dynamicDifficultyAdjustment != null && GameManager.Instance.dynamicDifficultyAdjustment.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.word))
        {
            toppled = true;
        }
        if(toppled)
        {
            transform.localPosition = destination;
            transform.localEulerAngles = destinationTransform.localEulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(topplingTower)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
            float rotation = (1 - Vector3.Distance(transform.localPosition, destination) / initialDistance) * destinationRotation + initialRotation;
            if(transform.localEulerAngles.x < destinationRotation)
            {
                transform.localEulerAngles = new Vector3(rotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            
            speed *= 1.005f;
        }
        if(transform.localPosition == destination)
        {
            topplingTower = false;
            foreach(GameObject border in bordersToBeRemoved)
            {
                border.SetActive(false);
            }
        }
    }
}
