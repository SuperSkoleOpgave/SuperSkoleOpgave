using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Analytics;
using Unity.IO.LowLevel.Unsafe;
using Unity.Properties;
using UnityEngine;

public class DynamicDifficultyAdjustment : MonoBehaviour
{
    List<LanguageUnit> words;
    List<LanguageUnit> letters;
    List<Property> properties;

    List<property> nonWeightedProperties = new List<property>()
    {
        property.word
    };
    int playerLanguageLevel;

    public void GetLetter(List<string> properties) {

    }

    public void GetLetters(List<string> properties) {
        
    }

    public void GetWord(List<string> properties) {

    }

    public void GetWords(List<string> properties) {
        
    }

    /// <summary>
    /// Takes the given languageunit and adjusts the weight of its properties up or down depending on the
    /// </summary>
    /// <param name="languageUnit"></param>
    /// <param name="correct"></param>
    public void AdjustWeight(LanguageUnit languageUnit, bool correct)
    {
        if(words.Contains(languageUnit) || letters.Contains(languageUnit))
        {
            //goes through the properties of the languageunit and updates the weight of its weighted properties
            foreach(Property property in languageUnit.properties)
            {
                if(!nonWeightedProperties.Contains(property.property))
                {
                    if(correct && property.weight > 1)
                    {
                        property.weight -= 1;
                    }
                    else if(property.weight < 100)
                    {
                        property.weight += 1;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("no list contains the languageunit with identifier: " + languageUnit.identifier);
        }
        CalculateLanguageLevel();
    }

    /// <summary>
    /// Checks if the levelLock of the property is less than or equal to the playerLanguageLevel
    /// </summary>
    /// <param name="property">The property to be checked</param>
    /// <returns>Whether the player is high enough level to use the property</returns>
    public bool IsLanguageUnitTypeUnlocked(Property property)
    {
        return property.levelLock <= playerLanguageLevel;
    }

    public List<Property> GetPlayerPriority()
    {
        return new List<Property>();
    }

    private void Load()
    {

    } 

    private void Save()
    {

    }

    private void SetupLanguageUnits()
    {

    }

    private void CalculateLanguageLevel()
    {

    }
}