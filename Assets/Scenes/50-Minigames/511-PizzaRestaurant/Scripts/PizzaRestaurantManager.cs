using CORE.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the Pizza Restaurant minigame, handling word generation, letter placement,
/// and game state for a word-guessing puzzle where players assemble letters on a pizza.
/// </summary>
public class PizzaRestaurantManager : MonoBehaviour
{
    [SerializeField] GameObject ingredientBoard;

    [SerializeField] GameObject textIngredientHolder;

    [SerializeField] CheckPizzaIngredient ingredientChecker;

    [SerializeField] Texture defaultImage;
    string testWord = "abe";

    char[,] lettersForCurrentRound = new char[3,4];
    private string wordToGuess;

    [SerializeField] RawImage ImageDisplay;
    private int currentLetterToGuessIndex;

    private List<GameObject> spawnedLetters = new List<GameObject>();

    private const float LETTER_SPACING_X = 12f;
    private const float LETTER_SPACING_Y = -12f;




    // Start is called before the first frame update
    void Start()
    {
        ingredientChecker.manager = this;

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
    if (currentLetterToGuessIndex < wordToGuess.Length - 1)
    {
        currentLetterToGuessIndex++;
        ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];
    }
    else
    {
        spawnedLetters.ForEach(Destroy);
        spawnedLetters.Clear();
        lettersForCurrentRound = new char[3, 4];
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
            TextMeshProUGUI text = textIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        

            wordToGuess = WordsForImagesManager.GetRandomWordForImage();
            if (string.IsNullOrEmpty(wordToGuess))
            {
                Debug.LogError("Received an invalid word to guess.");
                    
                   
                    return;
            }
            ImageDisplay.texture = ImageManager.GetImageFromWord(wordToGuess);



            currentLetterToGuessIndex = 0;
            ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];


            int numberOfRandomPositions = wordToGuess.Length;


            // Get dimensions for clarity
            int numRows = lettersForCurrentRound.GetLength(0);
            int numCols = lettersForCurrentRound.GetLength(1);

            // Randomly assign the letters of 'wordToGuess' into the grid
            int assignedCorrectLetters = 0;
            while (assignedCorrectLetters < wordToGuess.Length)
            {
                int rndIndexX = UnityEngine.Random.Range(0, numRows);
                int rndIndexY = UnityEngine.Random.Range(0, numCols);

                if (lettersForCurrentRound[rndIndexX, rndIndexY] == '\0')
                {
                    lettersForCurrentRound[rndIndexX, rndIndexY] = wordToGuess[assignedCorrectLetters];
                    assignedCorrectLetters++;
                }
            }

            // Fill the remaining empty positions with random letters
            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    if (lettersForCurrentRound[x, y] == '\0')
                    {
                        lettersForCurrentRound[x, y] = LetterManager.GetRandomLetter();
                    }
                }
            }

            

        
     





            // Code to actually instantiate the letters for the current round that have been put together in a random sequence. 
            for (int y = 0; y < lettersForCurrentRound.GetLength(1); y++)
            {
                for (int x = 0; x < lettersForCurrentRound.GetLength(0); x++)
                {
                    text.text = lettersForCurrentRound[x, y].ToString();


                    Vector3 pos = new Vector3(LETTER_SPACING_X * x, y * LETTER_SPACING_Y, 0);



                    GameObject instObject = Instantiate(textIngredientHolder, ingredientBoard.transform);

                    instObject.transform.position += pos;
                    instObject.GetComponent<IngredientHolderPickup>().startPosition = instObject.transform.position;
                    instObject.GetComponent<IngredientHolderPickup>().ingredientChecker = ingredientChecker;
                    spawnedLetters.Add(instObject);
                }

            }
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
