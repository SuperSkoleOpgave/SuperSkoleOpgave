using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardropeCus : MonoBehaviour
{
    [SerializeField] private GameObject customizationScreen;

    public void HandleCustomzation()
    {
        PlayerWorldMovement.allowedToMove = false;
        //customizationScreen.GetComponent<SetCuzCurrentColor>().OpeningScreen();
        customizationScreen.SetActive(true);
    }

    public void DisableCuz()
    {
        customizationScreen.SetActive(false);
        PlayerWorldMovement.allowedToMove = true;
    }

}
