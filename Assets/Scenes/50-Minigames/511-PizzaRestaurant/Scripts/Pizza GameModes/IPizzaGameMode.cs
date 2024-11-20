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
    public void GetDisplayAnswer(PizzaRestaurantManager manager);


    /// <summary>
    ///will pick the correct prefab from prefabs in the PizzaRestaurantManager, then set that to be the PizzaRestaurantManager AnswerHolderPrefab
    /// </summary>
    /// <param name="manager"></param>
    public void SetAnswerPrefab(PizzaRestaurantManager manager);

    /// <summary>
    /// Generates the wrong answers 
    /// </summary>
    /// <param name="count"></param>
    public void GenerateAnswers(PizzaRestaurantManager manager, int numRows, int numCols);

   
}
