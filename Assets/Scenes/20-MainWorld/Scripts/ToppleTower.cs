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

    /// <summary>
    /// Sets up various toppling related variables and checks if the tower has allready been toppled
    /// </summary>
    void Start()
    {
        destination = destinationTransform.localPosition;
        destinationRotation = destinationTransform.localEulerAngles.x;
        initialDistance = Vector3.Distance(transform.localPosition, destination);
        initialRotation = transform.localEulerAngles.x;
        if(IsTowerToppled())
        {
            toppled = true;
        }
        if(toppled)
        {
            transform.localPosition = destination;
            transform.localEulerAngles = destinationTransform.localEulerAngles;
        }
    }

    /// <summary>
    /// Checks if the tower has been toppled previously on the character
    /// </summary>
    /// <returns>whether the tower has been toppled previously</returns>
    bool IsTowerToppled()
    {
        if(topplingTower || toppled)
        {
            return false;
        }
        if(GameManager.Instance.dynamicDifficultyAdjustment == null)
        {
            return false;
        }
        return GameManager.Instance.dynamicDifficultyAdjustment.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.word);
    }

    /// <summary>
    /// Moves and rotates the tower if it is in the procces of being toppled
    /// </summary>
    void Update()
    {
        //Updates location and rotation of the tower
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
        //Stops the toppling once it has reached the desired position and opens up the borders for the player to walk onto the tower
        if(transform.localPosition == destination)
        {
            topplingTower = false;
            toppled = true;
            foreach(GameObject border in bordersToBeRemoved)
            {
                border.SetActive(false);
            }
        }
    }
}
