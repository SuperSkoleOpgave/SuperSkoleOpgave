using CORE;
using CORE.Scripts;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneGameManager : MonoBehaviour
{

    public bool isGameOn = true;

    [SerializeField]
    private Material correctMat, wrongMat;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private PlaneGameController gameController;

    [SerializeField]
    private CreatePointLoop createPoint;

    [SerializeField]
    private CreateBackgroundClouds backgroundClouds;

    public bool resetLoop = false;

    public bool resetCloud = false;

    [SerializeField]
    private string currentWord;

    [SerializeField]
    private int letterNumber = 0;

    [SerializeField]
    private char currentLetter;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject lossScreen;

    [SerializeField]
    private GameObject letterBoxText;

    public TextMeshProUGUI letterText;

    public bool isCorrect = false;

    private string preMessage = "";

    public int point = 0;

    [SerializeField]
    private DisplayCurrentImage currentImage;

    [SerializeField]
    private LoopObjecPool skySpeed;
    private bool won = false;

    [SerializeField]
    private Scrollbar timerBar;
    [SerializeField]
    private float maxTime = 120;

    private float remainingTime;

    [SerializeField]
    private int cloudCount = 0;


    void Start()
    {
        letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
        SpawnClouds();
    }

    
    void Update()
    {

        if (resetCloud)
        {
            SpawnClouds();
        }
        if (resetLoop)
        {
            LoopHitsWall();
        }
        if(!won && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerBar.size = remainingTime / maxTime;
        }
        else if(!won && remainingTime <= 0)
        {
            StartCoroutine(CheckIfYouLose());
        }
    }

    /// <summary>
    /// Sets up the gamemode by getting current word, letter.
    /// </summary>
    public void GameSetup()
    {
        currentWord = gameController.CurrentWord();
        letterNumber = gameController.currentWordNumber;
        currentLetter = gameController.CurrentLetter();
        isGameOn = true;
        createPoint.CreatePointLoops();
        remainingTime = maxTime;
        
    }

    /// <summary>
    /// if the goalLoop hits the DeleteWall
    /// </summary>
    public void LoopHitsWall()
    {
        
        createPoint.CreatePointLoops();
        resetLoop = false;
    }

    public void CloudHitsWall()
    {
        backgroundClouds.CreateCloud();
        resetCloud = false;
    }

    /// <summary>
    /// Checks if GameObject that collided with player has the correct TexT
    /// </summary>
    /// <param name="gameObject">is the gameObject that collided with the player</param>
    public void CheckIfCorrect(GameObject gameObject)
    {

        gameController.UpdateCurrentLetter();
        currentLetter = gameController.CurrentWord()[gameController.currentWordNumber];
        if (currentWord != null && currentLetter.ToString() != null)
        {
            char selectedLetter = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text.ToLower()[0];
           // Debug.Log(selectedLetter);

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

    /// <summary>
    /// if letterContent is correct execute this context, assert points and resets word and letters the word is completed.
    /// </summary>
    /// <param name="gameObj">the gameObject that player collided with</param>
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
            gameController.UpdateCurrentLetter();
            currentWord = gameController.CurrentWord();
            letterNumber = gameController.currentWordNumber;
            currentLetter = gameController.CurrentLetter();
            currentImage.DisplayImage();
            preMessage = "";
            skySpeed.speed += 3;




            if (point >= 3)
            {
                StartCoroutine(CheckIfYouWin());
                point = 0;
            }

        }
    }

    /// <summary>
    /// if letterContent is wrong execute this context.
    /// </summary>
    /// <param name="gameObj"></param>
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
        won = true;
        PlayerEvents.RaiseGoldChanged(1);
        PlayerEvents.RaiseXPChanged(1);
        Instantiate(coinPrefab);
        winScreen.SetActive(true);
        yield return new WaitForSeconds(2);

        SwitchScenes.SwitchToMainWorld();
    }

    /// <summary>
    /// Sets winscreen active and after a few seconds switches to GameWorld
    /// </summary>
    /// <returns> 2 second delay then proceed to return to MainWorld</returns>
    IEnumerator CheckIfYouLose()
    {
        won = true;
        lossScreen.SetActive(true);
        yield return new WaitForSeconds(2);

        SwitchScenes.SwitchToMainWorld();
    }

    /// <summary>
    /// Spawns clouds
    /// </summary>
    /// <returns>spawns a cloud if theres less than 8 spawns more.</returns>
    IEnumerator SpawnClouds()
    {
        backgroundClouds.CreateCloud();
        cloudCount += 1;
        

        if (cloudCount <= 8)
        {
            backgroundClouds.CreateCloud();
        }
        yield return new WaitForSeconds(3);


    }




}
