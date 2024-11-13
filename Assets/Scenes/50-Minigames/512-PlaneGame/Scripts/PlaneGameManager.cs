using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{

    public bool isGameOn = true;


    [SerializeField]
    private PlaneGameController gameController;

    
    void Start()
    {
        isGameOn = true;
    }

    
    void Update()
    {
        
    }

    private void GameStart()
    {

    }

    
}
