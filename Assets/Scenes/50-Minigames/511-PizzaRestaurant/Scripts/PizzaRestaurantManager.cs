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

   public void correctIngredientAdded()
    {
        if (wordToGuess.Length-1 > currentLetterToGuessIndex)
        {
            currentLetterToGuessIndex++;
            ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];
        }
        else
        {
            foreach (var item in spawnedLetters)
            {
                
                Destroy(item);
                
            }
           lettersForCurrentRound = new char[3, 4];
            StartNewRound();
            
            Debug.Log("Finished Word Correctly");
        }
        
    }

    void StartNewRound()
    {


       TextMeshProUGUI text= textIngredientHolder.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        wordToGuess = testWord;
        ///ÚSE LATER WHEN IMAGES CAN BE LOADED SO WHEN THE GAME IS IN THE MAINWORLD !!!!!!
        //ImageDisplay.texture = ImageManager.GetImageFromWord(wordToGuess);
        
        
     
        currentLetterToGuessIndex = 0;
        ingredientChecker.currentLetterToGuess = wordToGuess[currentLetterToGuessIndex];
        

        int numberOfRandomPositions = wordToGuess.Length;

        int assignedCorrectLetters = 0;


        for (int i = 0; i < 12; i++)
        {
            int rndIndexX = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(0));
            int rndIndexY = UnityEngine.Random.Range(0, lettersForCurrentRound.GetLength(1));
           


           
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
