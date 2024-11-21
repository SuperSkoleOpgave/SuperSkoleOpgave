using CORE;
using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadingLevel_Pizza : MonoBehaviour, IPizzaGameMode
{

    /// <summary>
    /// Checks if the added wordImage is the current word to guess and returns a true or false. 
    /// </summary>
    public bool CheckIngredient(Collider2D collision, CheckPizzaIngredient checker)
    {
        string wordAdded = collision.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture.name;

        if (string.IsNullOrEmpty(wordAdded))
        {
            Debug.LogWarning("wordAdded is null or empty.");
            return false;
        }

        string wordToCheck = wordAdded.Split(" ")[0];

        wordToCheck = wordToCheck.Replace("(aa)", "\u00e5");
        wordToCheck = wordToCheck.Replace("(ae)", "\u00e6");
        wordToCheck = wordToCheck.Replace("(oe)", "\u00F8");


        return wordToCheck == checker.currentWordToGuess;
    }

    public void GenerateAnswers(PizzaRestaurantManager manager, int numRows,int numCols)
    {
        // Randomly assign the 'wordToGuess' into the grid
         
            int rndIndexX = UnityEngine.Random.Range(0, numRows);
            int rndIndexY = UnityEngine.Random.Range(0, numCols);

            if (manager.wordsForCurrentRound[rndIndexX, rndIndexY] == null)
            {
                manager.wordsForCurrentRound[rndIndexX, rndIndexY] = manager.wordToGuess;
                
            }
        

        // Fill the remaining empty positions with random words
        for (int x = 0; x < numRows; x++)
        {
            for (int y = 0; y < numCols; y++)
            {
                
                if (manager.wordsForCurrentRound[x, y]==null)
                {
                    manager.wordsForCurrentRound[x, y] = WordsForImagesManager.GetRandomWordForImage();

                    
                }
            }
        }



        // Code to actually instantiate the image corresponding to the words for the current round that have been put together in a random sequence. 
        for (int y = 0; y < numCols; y++)
        {
            for (int x = 0; x < numRows; x++)
            {
               

                manager.imageOnIngredientHolder.texture= ImageManager.GetImageFromWord(manager.wordsForCurrentRound[x, y]);

               
             Vector3 pos = new Vector3(manager.LETTER_SPACING_X * x, y * manager.LETTER_SPACING_Y, 0);



                GameObject instObject = Instantiate(manager.imageIngredientHolder, manager.ingredientBoard.transform);

                instObject.transform.position += pos;
                instObject.GetComponent<IngredientHolderPickup>().startPosition = instObject.transform.position;
                instObject.GetComponent<IngredientHolderPickup>().ingredientChecker = manager.ingredientChecker;
                manager.spawnedIngredients.Add(instObject);
            }

        }
    }



        public void GetDisplayAnswer(PizzaRestaurantManager manager)
    {
        manager.wordToGuess = GameManager.Instance.dynamicDifficultyAdjustment.GetWord(new List<LanguageUnitProperty>()).identifier; 
        manager.ingredientChecker.currentWordToGuess = manager.wordToGuess;
        if (string.IsNullOrEmpty(manager.wordToGuess))
        {
            Debug.LogError("Received an invalid word to guess.");


            return;
        }
        manager.displayAnswerText.text = manager.wordToGuess;

    }

    public int GetNumCols()
    {
        return 4;
    }

    public int GetNumRows()
    {
        return 3;
    }

    public void SetAnswerPrefab(PizzaRestaurantManager manager)
    {
        throw new System.NotImplementedException();
    }

    public void SetCorrectAnswer(string str, PizzaRestaurantManager manager)
    {
        throw new System.NotImplementedException();
    }
}
