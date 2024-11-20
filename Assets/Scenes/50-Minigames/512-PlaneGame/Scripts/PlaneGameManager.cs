using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private int letterNumber = 0;

    [SerializeField]
    private char currentLetter;

    [SerializeField]
    private GameObject letterBoxText;

    public TextMeshProUGUI letterText;

    public bool isCorrect = false;




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
        currentLetter = gameController.CurrentLetter();
        isGameOn = true;
        createPoint.CreatePointLoops();
    }

    public void LoopHitsWall()
    {
        
        createPoint.CreatePointLoops();
        resetLoop = false;
    }

    public void CheckIfCorrect(GameObject gameObject)
    {
        if (currentWord != null && currentLetter.ToString() != null)
        {
            char selectedLetter = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()[0];
            Debug.Log(selectedLetter);
        }
    }

    

    
}
