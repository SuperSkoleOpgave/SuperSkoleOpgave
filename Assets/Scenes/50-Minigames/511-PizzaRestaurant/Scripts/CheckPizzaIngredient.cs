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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        {

            char letterAddedToPizza = collision.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text[0];

            bool correctLetter = CheckIfCorrectLetter(letterAddedToPizza);

            if (correctLetter == false)
            {
                collision.gameObject.GetComponent<IngredientHolderPickup>().isDragable = false;
                collision.gameObject.transform.position = collision.gameObject.GetComponent<IngredientHolderPickup>().startPosition;
               
                StartCoroutine(DisplayWrongAnswerText());

            }


        }
    }


    bool CheckIfCorrectLetter(char letterAdded)
    {
        if(letterAdded==currentLetterToGuess)
        {
            manager.correctIngredientAdded();
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
