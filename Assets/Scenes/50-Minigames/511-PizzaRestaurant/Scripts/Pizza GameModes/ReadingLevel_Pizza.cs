using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingLevel_Pizza : MonoBehaviour, IPizzaGameMode
{
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

                    Debug.Log(manager.wordsForCurrentRound[x, y]);
                }
            }
        }



        // Code to actually instantiate the image corresponding to the words for the current round that have been put together in a random sequence. 
        for (int y = 0; y < numCols; y++)
        {
            for (int x = 0; x < numRows; x++)
            {
                Debug.Log(manager.wordsForCurrentRound[x, y]);

                manager.imageOnIngredientHolder.texture= ImageManager.GetImageFromWord(manager.wordsForCurrentRound[x, y]);

                if (manager.imageOnIngredientHolder.texture == null)
                {
                    Debug.Log("Null image"+ manager.wordsForCurrentRound[x, y]);
                }
                else
                {
                    Debug.Log("image not null");
                }
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
        manager.wordToGuess = WordsForImagesManager.GetRandomWordForImage();
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
