using CORE;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneGameManager : MonoBehaviour
{

    public bool isGameOn = true;

    [SerializeField]
    private Material correctMat, wrongMat;

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
    private GameObject winScreen;

    [SerializeField]
    private GameObject letterBoxText;

    public TextMeshProUGUI letterText;

    public bool isCorrect = false;

    private string preMessage = "";

    public int point = 0;




    void Start()
    {
        letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
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
            char selectedLetter = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()[gameController.currentWordNumber];
            Debug.Log(selectedLetter);

            if (currentLetter == selectedLetter)
            {
                IsCorrect(gameObject);
            }
            else
            {
                IsWrong(gameObject);
            }
        }
    }

    private void IsCorrect(GameObject gameObj)
    {
        gameObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = correctMat;
        GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(currentWord, true);

        preMessage += currentLetter.ToString();
        letterText.text = $"Score: {preMessage}";

        gameController.currentWordNumber += 1;
        gameController.UpdateCurrentLetter();

        if (currentWord.Length <= gameController.currentWordNumber)
        {
            point += 1;
            gameController.ResetCurrentLetterNumber();
            gameController.GetWord();
            preMessage = "";

            if (point >= 5)
            {
                StartCoroutine(CheckIfYouWin());
            }

        }
    }

    private void IsWrong(GameObject gameObj)
    {
        gameObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = wrongMat;
        GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(currentWord, false);
    }

    /// <summary>
    /// Sets winscreen active and after a few seconds switches to GameWorld
    /// </summary>
    /// <returns> 2 second delay</returns>
    IEnumerator CheckIfYouWin()
    {

        winScreen.SetActive(true);
        yield return new WaitForSeconds(2);

        SwitchScenes.SwitchToMainWorld();
    }




}
