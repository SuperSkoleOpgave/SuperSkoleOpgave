using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPizzaIngredient : MonoBehaviour
{
    // Start is called before the first frame update

    public PizzaRestaurantManager manager;
    public char currentLetterToGuess;

    [SerializeField] GameObject wrongAnswerText;
    public bool checkLetter;
    private bool correctLetter=false;

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
                bool isPreviousCorrectLetter = collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrectLetter;


                if (isPreviousCorrectLetter == false)
                {

                    char letterAddedToPizza = collision.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text[0];
                 
                    correctLetter = CheckIfCorrectLetter(letterAddedToPizza);

                 
                    //Moves on to next letter if the answer is correct and displays a incorret text if its not the right letter added. 
                    if (correctLetter == false && isPreviousCorrectLetter == false)
                    {
                        collision.gameObject.GetComponent<IngredientHolderPickup>().isDragable = false;

                        collision.gameObject.transform.position = collision.gameObject.GetComponent<IngredientHolderPickup>().startPosition;

                        StartCoroutine(DisplayWrongAnswerText());

                    }
                    else if (correctLetter == true && isPreviousCorrectLetter == false)
                    {
                        collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrectLetter = true;
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
        if(letterAdded==currentLetterToGuess)
        {
          
            return true;
        }
        else
        {
            return false;
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
