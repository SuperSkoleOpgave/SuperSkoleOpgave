using CORE.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PizzaRestaurantManager : MonoBehaviour
{
    [SerializeField] GameObject ingredientBoard;

    [SerializeField] GameObject textIngredientHolder;

    [SerializeField] CheckPizzaIngredient ingredientChecker;
    string testWord = "abe";

    char[,] lettersForCurrentRound = new char[3,4];
    private string wordToGuess;

    [SerializeField] RawImage ImageDisplay;
    private int currentLetterToGuessIndex;

    private List<GameObject> spawnedLetters = new List<GameObject>();




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


        TextMeshProUGUI text= textIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        wordToGuess = WordsForImagesManager.GetRandomWordForImage();
        ///ÚSE LATER WHEN IMAGES CAN BE LOADED SO WHEN THE GAME IS IN THE MAINWORLD !!!!!!
        ImageDisplay.texture = ImageManager.GetImageFromWord(wordToGuess);
        
        
     
        currentLetterToGuessIndex = 0;
        ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];
        

        int numberOfRandomPositions = wordToGuess.Length;

        int assignedCorrectLetters = 0;


        for (int i = 0; i < 12; i++)
        {
            int rndIndexX = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(0));
            int rndIndexY = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(1));
           


           //Code to assign random positions for the letters of the correct word on the ingredientboard. 
            if (assignedCorrectLetters<numberOfRandomPositions)
            {
                while (lettersForCurrentRound[rndIndexX, rndIndexY] != '\0')
                {
                    rndIndexX = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(0));
                    rndIndexY = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(1));

                }
                lettersForCurrentRound[rndIndexX,rndIndexY] = wordToGuess[assignedCorrectLetters];
                assignedCorrectLetters++;
              
            }
            else
            {
                //Code to assign random letters places on the ingredientboard. 
                for (int x = 0; x < lettersForCurrentRound.GetLength(0); x++)
                {
                    for (int y = 0; y < lettersForCurrentRound.GetLength(1); y++)
                    {
                           
                        if (lettersForCurrentRound[x,y]== '\0')
                        {
                            lettersForCurrentRound[x,y] = LetterManager.GetRandomLetter();
                           
                            break;
                        }
                    }
                   
                }
            }
        }


        

        // Code to actually instantiate the letters for the current round that have been put together in a random sequence. 
        for (int y = 0; y < lettersForCurrentRound.GetLength(1); y++)
        {
            for (int x = 0; x < lettersForCurrentRound.GetLength(0); x++)
            {
                text.text = lettersForCurrentRound[x,y].ToString();


                Vector3 pos = new Vector3(12 *x, y*-12, 0);


               
                GameObject instObject = Instantiate(textIngredientHolder, ingredientBoard.transform);

                instObject.transform.position += pos;
                instObject.GetComponent<IngredientHolderPickup>().startPosition = instObject.transform.position;
                instObject.GetComponent<IngredientHolderPickup>().ingredientChecker = ingredientChecker;
                spawnedLetters.Add(instObject);
            }
            
        }


        

    }
}
