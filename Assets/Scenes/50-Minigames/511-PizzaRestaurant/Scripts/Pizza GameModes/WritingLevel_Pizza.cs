using CORE;
using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WritingLevel_Pizza : MonoBehaviour, IPizzaGameMode
{
    private string letterToDisplay;

    /// <summary>
    /// Checks if the added letters is the current letter to guess and returns a true or false. 
    /// </summary>
    public bool CheckIngredient(Collider2D collision, CheckPizzaIngredient checker)
    {
        char letterAddedToPizza = collision.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text[0];

       return letterAddedToPizza == checker.currentLetterToGuess;
    }

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
                letterToDisplay= manager.lettersForCurrentRound[x, y].ToString();

                while(letterToDisplay.ToLower()=="q" ||letterToDisplay.ToLower()=="x" || letterToDisplay=="w")
                {
                    letterToDisplay = LetterManager.GetRandomLetter().ToString();
                }
                manager.lettersForCurrentRound[x, y] = letterToDisplay[0];

                manager.textOnIngredientHolder.text = letterToDisplay;

                List<string> ingredientsToDisplay = manager.ingredientWords[letterToDisplay.ToLower()];
              
                manager.textOnIngredientHolderBackGround.texture = ImageManager.GetImageFromWord(ingredientsToDisplay[UnityEngine.Random.Range(0,ingredientsToDisplay.Count)].ToLower());

                Vector3 pos = new Vector3(manager.LETTER_SPACING_X * x, y * manager.LETTER_SPACING_Y, 0);



                GameObject instObject = Instantiate(manager.textIngredientHolder, manager.ingredientBoard.transform);

                instObject.transform.position += pos;
                instObject.GetComponent<IngredientHolderPickup>().startPosition = instObject.transform.position;
                instObject.GetComponent<IngredientHolderPickup>().ingredientChecker = manager.ingredientChecker;
                manager.spawnedIngredients.Add(instObject);
            }

        }
    }



        public void SetDisplayAnswer(PizzaRestaurantManager manager)
    {
        manager.wordToGuess = GameManager.Instance.dynamicDifficultyAdjustment.GetWord(new List<LanguageUnitProperty>()).identifier;
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
