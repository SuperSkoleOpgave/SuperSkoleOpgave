using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffPointThingy : MonoBehaviour
{
    public string allLettersCollected = "";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerAtack temp = other.GetComponent<PlayerAtack>();
            allLettersCollected += temp.inventory;
            temp.inventory = "";
        }
    }

}
