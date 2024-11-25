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
                
                    // Checks if the ingridient is a previous correct ingredient. 
                    bool isPreviousCorrect = collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrect;


                    if (isPreviousCorrect == false)
                    {

                    if (manager != null && manager.gameMode != null)
                    {
                        correctIngredient = manager.gameMode.CheckIngredient(collision, this);
                    }
                    else
                    {

                        Debug.LogError("Manager or GameMode is null.");
                        return; // Exit the method to prevent further processing.
                    }
                 
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
