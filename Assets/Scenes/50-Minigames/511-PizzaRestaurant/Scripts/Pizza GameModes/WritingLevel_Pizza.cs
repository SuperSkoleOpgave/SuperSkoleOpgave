using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WritingLevel_Pizza : MonoBehaviour, IPizzaGameMode
{



    public void GenerateAnswers(PizzaRestaurantManager manager, int numRows,int numCols)
    {
        // Randomly assign the letters of 'wordToGuess' into the grid
        int assignedCorrectLetters = 0;
        while (assignedCorrectLetters < manager.wordToGuess.Length)
        {
            int rndIndexX = UnityEngine.Random.Range(0, numRows);
            int rndIndexY = UnityEngine.Random.Range(0, numCols);

            if (manager.lettersForCurrentRound[rndIndexX, rndIndexY] == '\0')
            {
                manager.lettersForCurrentRound[rndIndexX, rndIndexY] = manager.wordToGuess[assignedCorrectLetters];
                assignedCorrectLetters++;
            }
        }

        // Fill the remaining empty positions with random letters
        for (int x = 0; x < numRows; x++)
        {
            for (int y = 0; y < numCols; y++)
            {
                if (manager.lettersForCurrentRound[x, y] == '\0')
                {
                    manager.lettersForCurrentRound[x, y] = LetterManager.GetRandomLetter();
                }
            }
        }



        // Code to actually instantiate the letters for the current round that have been put together in a random sequence. 
        for (int y = 0; y < numCols; y++)
        {
            for (int x = 0; x < numRows; x++)
            {
                manager.textOnIngredientHolder.text = manager.lettersForCurrentRound[x, y].ToString();


                Vector3 pos = new Vector3(manager.LETTER_SPACING_X * x, y * manager.LETTER_SPACING_Y, 0);



                GameObject instObject = Instantiate(manager.textIngredientHolder, manager.ingredientBoard.transform);

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
        manager.currentLetterToGuessIndex = 0;
        manager.ingredientChecker.currentLetterToGuess = manager.wordToGuess[manager.currentLetterToGuessIndex];

        if (string.IsNullOrEmpty(manager.wordToGuess))
        {
            Debug.LogError("Received an invalid word to guess.");


            return;
        }
        manager.ImageDisplay.texture = ImageManager.GetImageFromWord(manager.wordToGuess);

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
