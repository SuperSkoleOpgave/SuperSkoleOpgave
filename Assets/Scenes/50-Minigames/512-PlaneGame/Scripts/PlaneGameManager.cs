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
        isGameOn = true;

        createPoint.CreatePointLoops();

        currentWord = gameController.CurrentWord();
    }

    
    void Update()
    {
        if (resetLoop)
        {
            LoopHitsWall();
        }
    }

    public void LoopHitsWall()
    {
        
        createPoint.CreatePointLoops();
        resetLoop = false;
    }

    

    
}
