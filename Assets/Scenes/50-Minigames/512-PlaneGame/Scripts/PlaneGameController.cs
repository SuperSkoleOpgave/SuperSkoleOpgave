using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameController : MonoBehaviour, IMinigameSetup
{

    [SerializeField]
    private string placeHolderWord = "mis";

    public int currentWordNumber = 0;

    [SerializeField]
    private string randoWord = "";

    [SerializeField]
    private char currentLetter;

    [SerializeField]
    private PlaneGameManager planeGameManager;


    List<LanguageUnit> langUnit;

    private bool kickedOut = false;


    private void Start()
    {
        langUnit = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 16);

        randoWord = GetWord();
        CurrentWord();
        UpdateCurrentLetter();
        CurrentLetter();
        planeGameManager.GameSetup();
    }

    private void Update()
    {
        if (langUnit.Count == 0 && !kickedOut)
        {
            kickedOut = true;
            SwitchScenes.SwitchToMainWorld();
        }
    }

    /// <summary>
    /// Gets a random word
    /// </summary>
    /// <returns>A random word</returns>
    public string GetWord()
    {
        string randomWord = langUnit[Random.Range(0, langUnit.Count)].identifier;

        randoWord = randomWord;

        return randoWord;
    }

    /// <summary>
    /// turns the randomWord into the current word
    /// </summary>
    /// <returns>Returns a string as the current word</returns>
    public string CurrentWord()
    {
        string currentWord = randoWord;

        return currentWord;
    }

    /// <summary>
    ///  Simply returns the current Letter
    /// </summary>
    /// <returns> a char thats the currentLetter</returns>
    public char CurrentLetter()
    {
        return currentLetter;
    }

    /// <summary>
    /// gets a random char from the LetterManager
    /// </summary>
    /// <returns>Returns a Char as a random letter</returns>
    public char GetRandomLetter()
    {
        char randoLetter = LetterManager.GetRandomLetter();

        if (randoLetter == currentLetter)
        {
            randoLetter = LetterManager.GetRandomLetter();
        }

        return randoLetter;
    }

    /// <summary>
    /// updates the currentLetter when its called.
    /// </summary>
    public void UpdateCurrentLetter()
    {
        string currentWord = CurrentWord() != null ? CurrentWord() : placeHolderWord;

        if (currentWordNumber >= 0 && currentWordNumber < currentWord.Length)
        {
            currentLetter = currentWord[currentWordNumber];

        }
        else
        {
            Debug.LogWarning("currentWordNumber is out of range for the current word.");
            currentLetter = '\0';
        }

    }

    /// <summary>
    /// Resest currentletterNumber to 0
    /// </summary>
    /// <returns>returns currentWordNumber as 0</returns>
    public int ResetCurrentLetterNumber()
    {
        currentWordNumber = 0;
        return currentWordNumber;
    }

    /// <summary>
    /// if gamemodes need to be set up.
    /// </summary>
    /// <param name="gameMode">nothing now</param>
    /// <param name="gameRules">nothing now</param>
    public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
    {
        
    }
}
