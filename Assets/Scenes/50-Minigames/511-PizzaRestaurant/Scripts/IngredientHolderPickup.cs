using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class IngredientHolderPickup : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 startPosition;
    public CheckPizzaIngredient ingredientChecker;

    Vector3 mouseAndIngredientPosDif;
    public bool isDragable = true;
    public bool isCorrect=false;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    /// <summary>
    /// Makes the ingredient dragable after a certain amount of time. 
    /// </summary>
    /// <returns></returns>
    IEnumerator MakeDragableAgainAfterCoolDown()
    {
        yield return new WaitForSeconds(4);

        isDragable = true;

    }

    
    /// <summary>
    /// Returns the current position of the ingredient in screen space. 
    /// </summary>
    /// <returns></returns>
    private Vector3 GetIngredientPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    
    
    
    /// <summary>
    /// When the mouse button is pressed down over an ingredientholder the mouse position in relation to the ingredient in screen space before dragging is updated 
    /// </summary>
    private void OnMouseDown()
    {
        if (isDragable == true)
        {
            mouseAndIngredientPosDif = Input.mousePosition - GetIngredientPos();
        }
    }
    /// <summary>
    /// when the mouse button is held down over and ingredient and dragged the position of the ingredient is updated based on the mouse pos and the mouse-ingredient position relation. 
    /// </summary>
    private void OnMouseDrag()
    {
        if (isDragable == true)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mouseAndIngredientPosDif);
        }
    }

    /// <summary>
    /// When the mouse button is up the ingredient on the pizza is checked and it is made dragable again and potentially not depending on wether or not the answer is correct.
    /// If the letter has already been added and checked as a correct letter the check letter does not happen again. 
    /// </summary>
    private void OnMouseUp()
    {
        if (isCorrect == false)
        {
            ingredientChecker.checkLetter = true;
            StartCoroutine(MakeDragableAgainAfterCoolDown());
        }
    }


}
