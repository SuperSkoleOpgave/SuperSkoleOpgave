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

        if (collision.gameObject.tag == "Ingredient")
        {
          
            if (checkLetter == true)
            {
                
                bool isPreviousCorrectLetter = collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrectLetter;

                if (isPreviousCorrectLetter == false)
                {
                    char letterAddedToPizza = collision.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text[0];
                    Debug.Log(letterAddedToPizza.ToString());
                    correctLetter = CheckIfCorrectLetter(letterAddedToPizza);

                    Debug.Log("CorrectLetter:" + correctLetter);
                    Debug.Log("isPreviousCorrectLetter:" + isPreviousCorrectLetter);

                    if (correctLetter == false && isPreviousCorrectLetter == false)
                    {
                        collision.gameObject.GetComponent<IngredientHolderPickup>().isDragable = false;

                        collision.gameObject.transform.position = collision.gameObject.GetComponent<IngredientHolderPickup>().startPosition;

                        StartCoroutine(DisplayWrongAnswerText());

                    }
                    else if (correctLetter == true && isPreviousCorrectLetter == false)
                    {
                        collision.gameObject.GetComponent<IngredientHolderPickup>().isCorrectLetter = true;
                        manager.correctIngredientAdded();
                    }




                    checkLetter = false;
                }
            }

        }
    }


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

    IEnumerator DisplayWrongAnswerText()
    {
        wrongAnswerText.SetActive(true);

        yield return new WaitForSeconds(3);

        wrongAnswerText.SetActive(false);


    }

}
