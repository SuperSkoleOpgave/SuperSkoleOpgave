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
    private float maxTime = 480;

    private float remainingTime;

    [SerializeField]
    private int cloudCount = 0;

    [SerializeField]
    private Image targetCanvas;  
    private Color green = Color.green; 
    private Color white = Color.white;  
    private float blinkDuration = 3f;    
    private float blinkInterval = 0.3f;

    private bool isBlinking = false;

    public bool isTutorialOver = false;

    [SerializeField] private AudioClip wrongBuzz, correctBuzz, backgroundAmbience;


    void Start()
    {
        letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
        CloudHitsWall();
        AudioManager.Instance.PlaySound(backgroundAmbience, SoundType.Music, true);
    }

    
    void Update()
    {

        if (resetCloud)
        {
            CloudHitsWall();
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


    /// <summary>
    /// If cloud connects with the wall it will create other clouds and if theres less than 8 cloud it will make more.
    /// </summary>
    public void CloudHitsWall()
    {
        backgroundClouds.CreateCloud();
        cloudCount += 1;

        StartCoroutine(SpawnCloudWithDelay());

        if (cloudCount <= 8)
        {
            backgroundClouds.CreateCloud();
            cloudCount += 1;
        }

        cloudCount -= 1;
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
                isTutorialOver = true;
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
            remainingTime += 60;
            point += 1;
            gameController.ResetCurrentLetterNumber();
            gameController.GetWord();
            gameController.UpdateCurrentLetter();
            currentWord = gameController.CurrentWord();
            letterNumber = gameController.currentWordNumber;
            currentLetter = gameController.CurrentLetter();
            currentImage.DisplayImage();

            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
            
            skySpeed.speed += 2;
            backgroundClouds.CreateCloud();



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
        AudioManager.Instance.PlaySound(wrongBuzz, SoundType.SFX);
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
    /// Coroutine that spawns clouds with a delay
    /// </summary>
    private IEnumerator SpawnCloudWithDelay()
    {
        yield return new WaitForSeconds(2f); 

        if (cloudCount < 8) 
        {
            backgroundClouds.CreateCloud(); 
            cloudCount += 1;
        }
    }

    /// <summary>
    /// makes the canvas blink when you do a correct word.
    /// </summary>
    /// <returns>delay while the win blink</returns>
    private IEnumerator Blink()
    {
        isBlinking = true;
        float elapsedTime = 0f;

        AudioManager.Instance.PlaySound(correctBuzz, SoundType.SFX);

        while (elapsedTime < blinkDuration)
        {
            targetCanvas.color = green; 
            yield return new WaitForSeconds(blinkInterval);

            targetCanvas.color = white; 
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2; // Tæl tiden
        }

        targetCanvas.color = white;
        preMessage = "";
        letterText.text = $"Score: {preMessage}";
        isBlinking = false;
    }




}
