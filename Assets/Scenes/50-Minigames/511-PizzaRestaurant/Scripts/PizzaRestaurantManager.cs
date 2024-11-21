using CORE;
using CORE.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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

    

    public CheckPizzaIngredient ingredientChecker;

    [SerializeField] Texture defaultImage;
    string testWord = "abe";

    public char[,] lettersForCurrentRound = new char[3,4];

    public string[,] wordsForCurrentRound = new string[3,4];

    public string wordToGuess;

    public RawImage ImageDisplay;
    public int currentLetterToGuessIndex;

    public List<GameObject> spawnedIngredients = new List<GameObject>();

    public  float LETTER_SPACING_X = 15f;
    public  float LETTER_SPACING_Y = -15f;

    public IPizzaGameMode gameMode;
    private IPizzaGameMode[] gameModes = new IPizzaGameMode[] { new WritingLevel_Pizza() };
    private int numRows;
    private int numCols;

    public TextMeshProUGUI textOnIngredientHolder { get; set; }
    public RawImage imageOnIngredientHolder { get; set; }


    // Start is called before the first frame update
    void Start()
    {


        //gameMode = gameModes[UnityEngine.Random.Range(0, gameModes.Length)];

        gameMode = new ReadingLevel_Pizza();
        ingredientChecker.manager = this;
        textOnIngredientHolder = textIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        imageOnIngredientHolder = imageIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();

        Debug.Log("textOnIngredientHolder:"+textOnIngredientHolder);
        Debug.Log("imageOnIngredientHolder:"+imageOnIngredientHolder);
        
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
        spawnedIngredients.ForEach(Destroy);
        spawnedIngredients.Clear();
        lettersForCurrentRound = new char[numRows, numCols];
        wordsForCurrentRound = new string[numRows, numCols];
        StartNewRound();
    }
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
}
