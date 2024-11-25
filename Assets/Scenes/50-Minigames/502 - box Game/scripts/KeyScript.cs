using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField]
    private ShelfScript shelfScript;
    void OnMouseDown()
    {
        shelfScript.VerifyWord();
    }
}
