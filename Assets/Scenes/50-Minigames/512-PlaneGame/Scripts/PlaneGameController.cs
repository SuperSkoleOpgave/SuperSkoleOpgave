using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameController : MonoBehaviour, IMinigameSetup
{

    [SerializeField]
    private string placeHolderWord = "Kat";

    public int currentWordNumber = 0;

    [SerializeField]
    private char currentLetter;

    void Start()
    {
        UpdateCurrentLetter();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Gets a random word
    /// </summary>
    /// <returns>A random word</returns>
    public string GetWord()
    {
        string randomWord = WordsForImagesManager.GetRandomWordForImage();

        return randomWord;
    }

    public string CurrentWord()
    {
        string currentWord = GetWord();

        return currentWord;
    }

    public char CurrentLetter()
    {
        UpdateCurrentLetter();

        return currentLetter;
    }


    private void UpdateCurrentLetter()
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

        Debug.Log("Current Letter: " + currentLetter);

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
