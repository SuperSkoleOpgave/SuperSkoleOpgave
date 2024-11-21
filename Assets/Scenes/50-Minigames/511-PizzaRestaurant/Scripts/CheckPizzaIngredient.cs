using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPizzaIngredient : MonoBehaviour
{
    // Start is called before the first frame update

    public PizzaRestaurantManager manager;
    public char currentLetterToGuess;
    public string currentWordToGuess;

    [SerializeField] GameObject wrongAnswerText;
    public bool checkLetter;
    private bool correctIngredient=false;
    public string wordToCheck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        //Checks if the collision with an ingredient is colliding with the pizza and if it should check the letter if its correct. 
        if (collision.gameObject.tag == "Ingredient")
        {
          
            if (checkLetter == true)
            {
                
                    // Checks if the letter is a previous correct letter. 
                    bool isPreviousCorrect = collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrect;


                    if (isPreviousCorrect == false)
                    {

                        correctIngredient = manager.gameMode.CheckIngredient(collision, this);
                        //Moves on to next letter if the answer is correct and displays a incorret textOnIngredientHolder if its not the right ingredient added. 
                        if (correctIngredient == false && isPreviousCorrect == false)
                        {
                            collision.gameObject.GetComponent<IngredientHolderPickup>().isDragable = false;

                            collision.gameObject.transform.position = collision.gameObject.GetComponent<IngredientHolderPickup>().startPosition;

                            StartCoroutine(DisplayWrongAnswerText());

                        }
                        else if (correctIngredient == true && isPreviousCorrect == false)
                        {
                            collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrect = true;
                            manager.CorrectIngredientAdded();
                        }




                        checkLetter = false;
                    }
                
            }

        }
    }

    /// <summary>
    /// Checks if the added letters is the current letter to guess and returns a true or false. 
    /// </summary>
    /// <param name="letterAdded"></param>
    /// <returns></returns>
    bool CheckIfCorrectLetter(char letterAdded)
    {


        return letterAdded == currentLetterToGuess;
       


    }

    /// <summary>
    /// Checks if the added wordImage is the current word to guess and returns a true or false. 
    /// </summary>
    /// <param name="wordAdded"></param>
    /// <returns></returns>
    bool CheckIfCorrectWord(string wordAdded)
    {
        if (string.IsNullOrEmpty(wordAdded))
        {
            Debug.LogWarning("wordAdded is null or empty.");
            return false;
        }

        wordToCheck = wordAdded.Split(" ")[0];

        wordToCheck= wordToCheck.Replace("(aa)","\u00e5");
        wordToCheck=wordToCheck.Replace("(ae)","\u00e6");
        wordToCheck=wordToCheck.Replace("(oe)", "\u00F8");
        
        Debug.Log(wordToCheck + "==" + currentWordToGuess);

        return wordToCheck == currentWordToGuess;
        

    }

    /// <summary>
    /// Used to activate and deactivate the "Wrong answer" text after a certain amount of time. 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayWrongAnswerText()
    {
        wrongAnswerText.SetActive(true);

        yield return new WaitForSeconds(3);

        wrongAnswerText.SetActive(false);


    }

}
