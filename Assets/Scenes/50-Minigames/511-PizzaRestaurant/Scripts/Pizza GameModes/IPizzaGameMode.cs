using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPizzaGameMode : IGenericGameMode
{
    public int GetNumRows();
    public int GetNumCols();


    /// <summary>
    /// Will be called by the Pizza Restaurant Manager to get the correct answer. 
    /// </summary>
    public void SetCorrectAnswer(string str, PizzaRestaurantManager manager);

    /// <summary>
    /// Sets the displayanswer so the player knows what to put on the pizza.  
    /// </summary>
    /// <param name="str"></param>
    /// <param name="manager"></param>
    public void SetDisplayAnswer(PizzaRestaurantManager manager);


    /// <summary>
    /// Sets the appropriate answer prefab for the current game mode.
    /// </summary>
    /// <param name="manager"></param>
    public void SetAnswerPrefab(PizzaRestaurantManager manager);

    /// <summary>
    /// Generates the wrong answers 
    /// </summary>
    /// <param name="count"></param>
    public void GenerateAnswers(PizzaRestaurantManager manager, int numRows, int numCols);


   /// <summary>
   /// Checks the ingredient if it's the correct answer. 
   /// </summary>
   /// <param name="collision">The collision to check</param>
   /// <param name="checker">The ingredient checker instance</param>
    public bool CheckIngredient(Collider2D collision, CheckPizzaIngredient checker);
   


   
}
