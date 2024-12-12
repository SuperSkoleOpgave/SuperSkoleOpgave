using CORE;
using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// Manages the Pizza Restaurant minigame, handling word generation, letter placement,
/// and game state for a word-guessing puzzle where players assemble letters on a pizza.
/// </summary>
public class PizzaRestaurantManager : MonoBehaviour
{
    public GameObject ingredientBoard;

    public GameObject textIngredientHolder;
    public GameObject imageIngredientHolder;


    public TextMeshProUGUI displayAnswerText;

    [SerializeField] GameObject displayAnswerImage;

    [SerializeField] GameObject answerPrefab;

    [SerializeField] GameObject coinPrefab;

    public CheckPizzaIngredient ingredientChecker;

    [SerializeField] Texture defaultImage;


    public char[,] lettersForCurrentRound = new char[3, 4];

    public string[,] wordsForCurrentRound = new string[3, 4];

    public string wordToGuess;

    public RawImage ImageDisplay;
    public int currentLetterToGuessIndex;

    public List<GameObject> spawnedIngredients = new List<GameObject>();

    public float LETTER_SPACING_X = 15f;
    public float LETTER_SPACING_Y = -15f;

    public IPizzaGameMode gameMode;

    private int numRows;
    private int numCols;

    public TextMeshProUGUI textOnIngredientHolder { get; set; }
    public RawImage textOnIngredientHolderBackGround { get; set; }
    public RawImage imageOnIngredientHolder { get; set; }

    public Dictionary<string, List<string>> ingredientWords = new Dictionary<string, List<string>>();
    [SerializeField] AudioClip backGroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound(backGroundMusic, SoundType.Music, true);

        SetupIngredientWords();

        gameMode = new ReadingLevel_Pizza();
        ingredientChecker.manager = this;
        textOnIngredientHolder = textIngredientHolder.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        textOnIngredientHolderBackGround= textIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        imageOnIngredientHolder = imageIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();

        StartNewRound();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method used when a correct letter is added to the pizza. 
    /// It then either moves on the next letter in the word or loads up a new round with a new word. 
    /// </summary>
public void CorrectIngredientAdded()
{
    if (gameMode is WritingLevel_Pizza &&currentLetterToGuessIndex < wordToGuess.Length - 1)
    {
        currentLetterToGuessIndex++;
        ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];

    }   
    else
    {
        GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(wordToGuess, true);
        PlayerEvents.RaiseGoldChanged(1);
        PlayerEvents.RaiseXPChanged(1);
        Instantiate(coinPrefab);
        spawnedIngredients.ForEach(Destroy);
        spawnedIngredients.Clear();
        lettersForCurrentRound = new char[numRows, numCols];
        wordsForCurrentRound = new string[numRows, numCols];
        StartNewRound();
    }
}

    /// <summary>
    /// Used to switch the gamemode between reading and writing. 
    /// </summary>
    public void SwitchGameMode()
    {
        spawnedIngredients.ForEach(Destroy);
        spawnedIngredients.Clear();
        lettersForCurrentRound = new char[numRows, numCols];
        wordsForCurrentRound = new string[numRows, numCols];
        ImageDisplay.texture = null;
        displayAnswerText.text = "";

        if (gameMode is WritingLevel_Pizza)
        {
            gameMode = new ReadingLevel_Pizza();
        }
        else
        {
            gameMode = new WritingLevel_Pizza();
        }
        StartNewRound();

    }



    /// <summary>
    /// Sets up a round with a new word, new coresponding image to the word and letters that can be moved over to the pizza and checked for the new word. 
    /// </summary>
    void StartNewRound()
    {

        try
        {
          
            gameMode.SetDisplayAnswer(this);
          
            int numberOfRandomPositions = wordToGuess.Length;

            // Get dimensions for clarity

                 numRows = gameMode.GetNumRows();
                 numCols = gameMode.GetNumCols();
         
            gameMode.GenerateAnswers(this, numRows, numCols);

        }
        catch (NullReferenceException ex)
        {
            Debug.LogError($"Null reference encountered: {ex.Message}");
           
        }
        catch (ArgumentException ex)
        {
            Debug.LogError($"Invalid argument: {ex.Message}");
      
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize round: {ex.Message}");
            // Fallback to a simple predefined word if random word generation fails
            wordToGuess = "pizza";
            currentLetterToGuessIndex = 0;
            ingredientChecker.currentLetterToGuess = wordToGuess[0];
            
                    // Set a default texture or placeholder image
                    if (ImageDisplay != null)
                        {
                ImageDisplay.texture = defaultImage;
                        }
            
        }
    }

    /// <summary>
    /// Setup of all the words that can be used to get a background image for the ingredients/Letters
    /// </summary>
    void SetupIngredientWords()
    {
     
        ingredientWords.Add("a", new List<string>() { "agurk", "abe", "albue", "and", "ananas", "avokado"});
        ingredientWords.Add("b", new List<string>() { "baby", "banan", "ben", "bi", "bjørn", "bønner"});
        ingredientWords.Add("c", new List<string>() { "cola", "chili", "chokolade", "cowboy"});
        ingredientWords.Add("d", new List<string>() { "delfin", "dino", "donuts", "drage", "drink"});
        ingredientWords.Add("e", new List<string>() { "egern", "enhjørning", "engel", "elefant"});
        ingredientWords.Add("f", new List<string>() { "far", "fe", "fisk", "flagermus", "flodhest", "flue", "fod", "frø", "fugl", "får"});
        ingredientWords.Add("g", new List<string>() { "ged", "giraf", "gris", "gulerod"});
        ingredientWords.Add("h", new List<string>() {"haj", "hale", "hane", "høne", "havfrue", "heks", "hest", "hindbær", "hotdog", "hund"});
        ingredientWords.Add("i", new List<string>() {"is","isbjørn","isterninger","istapper"});
        ingredientWords.Add("j", new List<string>() {"jordbær","jordegern","julemand","jæger"});
        ingredientWords.Add("k", new List<string>() { "kaffe", "kage", "kaktus", "kalkun", "kamel", "kat", "kanin", "kartoffel", "kirsebær", "kiwi", "ko", "krabbe", "kænguru", "kål"});
        ingredientWords.Add("l", new List<string>() { "lasagne", "loppe", "løg", "løve"});
        ingredientWords.Add("m", new List<string>() { "majs", "mariehøne", "marsvin", "mel", "melon", "monster", "muldvarp", "mus", "myg", "myre"});
        ingredientWords.Add("n", new List<string>() { "næsehorn", "nødder", "narhval", "næse"});
        ingredientWords.Add("o", new List<string>() { "olie", "oliven", "orm"});
        ingredientWords.Add("p", new List<string>() { "papegøje", "pingvin", "pindsvin", "palme", "panda", "peberfrugt", "popcorn", "pære", "påfugl"});
        ingredientWords.Add("r", new List<string>() { "rotte", "reje", "rensdyr", "ris", "ræv"});
        ingredientWords.Add("s", new List<string>() { "skildpadde", "slim", "salat", "sandwich", "skelet", "slange", "slikkepind", "smør", "snegl", "sommerfugl", "spøgelse"});
        ingredientWords.Add("t", new List<string>() { "tomat", "te", "tiger"});
        ingredientWords.Add("u", new List<string>() { "ugle", "ulv", "ufo"});
        ingredientWords.Add("v", new List<string>() { "vin", "vindruer", "vafler", "vand", "vandmand", "vandmelon"});
        ingredientWords.Add("y", new List<string>() { "yver"});
        ingredientWords.Add("z", new List<string>() { "zebra"});
        ingredientWords.Add("æ", new List<string>() { "æble", "æbletærte", "æbleskrog", "æg", "æsel", "ælling"});
        ingredientWords.Add("ø", new List<string>() { "øl", "østers", "øre", "ørn"});
        ingredientWords.Add("å", new List<string>() { "ål", "åkande"});

       

    }

}
