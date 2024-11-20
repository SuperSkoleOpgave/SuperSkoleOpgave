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

    public string CurrentWord()
    {
        string currentWord = randoWord;

        return currentWord;
    }

    public char CurrentLetter()
    {
        return currentLetter;
    }

    public char GetRandomLetter()
    {
        char randoLetter = LetterManager.GetRandomLetter();

        if (randoLetter == currentLetter)
        {
            randoLetter = LetterManager.GetRandomLetter();
        }

        return randoLetter;
    }


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

    public int ResetCurrentLetterNumber()
    {
        currentWordNumber = 0;
        return currentWordNumber;
    }


    public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
    {
        
    }
}
