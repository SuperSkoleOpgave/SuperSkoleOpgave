using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{

    public bool isGameOn = true;


    [SerializeField]
    private PlaneGameController gameController;

    [SerializeField]
    private CreatePointLoop createPoint;

    public bool resetLoop = false;
    [SerializeField]
    private string currentWord;

    

    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (resetLoop)
        {
            LoopHitsWall();
        }
    }

    public void GameSetup()
    {
        currentWord = gameController.CurrentWord();
        isGameOn = true;
        createPoint.CreatePointLoops();
    }

    public void LoopHitsWall()
    {
        
        createPoint.CreatePointLoops();
        resetLoop = false;
    }

    

    
}
