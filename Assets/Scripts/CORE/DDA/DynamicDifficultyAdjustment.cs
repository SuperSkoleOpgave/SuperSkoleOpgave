using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.IO.LowLevel.Unsafe;
using Unity.Properties;
using UnityEditor.Purchasing;
using UnityEngine;
using Random = UnityEngine.Random;

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

    /// <summary>
    /// returns a letter, using properties given
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public LanguageUnit GetLetter(List<Property> properties)
    {
        if(letters == null)
        {
            throw new Exception("could not find any letters");
        }
        float totalweight = 0;
        foreach (LanguageUnit letter in letters)
        {
            letter.CalculateWeight();
            totalweight += letter.weight;
        }
        float rand = Random.Range(0f, totalweight);
        float cumulativeWeight = 0;
        foreach (LanguageUnit letter in letters)
        {
            cumulativeWeight += letter.weight;
            if(rand <= cumulativeWeight)
            {
                return letter;
            }
        }
        throw new Exception("could not find any letters");
    }

    /// <summary>
    /// returns a number of letters based on given properties
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<LanguageUnit> GetLetters(List<Property> properties, int count)
    {
        if(letters == null)
        {
            throw new Exception("could not find any letters");
        }
        if(count > letters.Count)
        {
            throw new Exception("Too many requested Letters");
        }
        List<LanguageUnit> returnedLetters = new List<LanguageUnit>();
        for (int i = 0; i < count; i++)
        {
            LanguageUnit languageUnit = GetLetter(properties);
            while(returnedLetters.Contains(languageUnit))
            {
                languageUnit = GetLetter(properties);
            }
            returnedLetters.Add(languageUnit);
        }
        return returnedLetters;
    }

    /// <summary>
    /// returns a word based on given properties
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public LanguageUnit GetWord(List<Property> properties)
    {
        if(words == null)
        {
            throw new Exception("could not find any words");
        }
        float totalweight = 0;
        foreach (LanguageUnit word in words)
        {
            word.CalculateWeight();
            totalweight += word.weight;
        }
        float rand = Random.Range(0f, totalweight);
        float cumulativeWeight = 0;
        foreach (LanguageUnit word in words)
        {
            cumulativeWeight += word.weight;
            if (rand <= cumulativeWeight)
            {
                return word;
            }
        }
        throw new Exception("could not find any words");
    }

    /// <summary>
    /// returns a number of words based on given properties
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<LanguageUnit> GetWords(List<Property> properties, int count)
    {
        if(words == null)
        {
            throw new Exception("could not find any wordds");
        }
        if(count > words.Count)
        {
            throw new Exception("Too many requested words");
        }
        List<LanguageUnit> returnedWords = new List<LanguageUnit>();
        for (int i = 0; i < count; i++)
        {

            LanguageUnit languageUnit = GetWord(properties);
            while(returnedWords.Contains(languageUnit))
            {
                languageUnit = GetWord(properties);
            }
            returnedWords.Add(languageUnit);
        }
        return returnedWords;
    }

    /// <summary>
    /// Takes the given languageunit and adjusts the weight of its properties up or down depending on the
    /// </summary>
    /// <param name="languageUnit">The languageUnit to have its weight adjusted</param>
    /// <param name="correct">Whether the player has answered correctly</param>
    public void AdjustWeight(LanguageUnit languageUnit, bool correct)
    {
        if(letters == null)
        {
            letters = new List<LanguageUnit>();
        }
        if(words == null)
        {
            words = new List<LanguageUnit>();
        }
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
                    else if(property.weight < 100 && !correct)
                    {
                        property.weight += 1;
                    }
                }
            }
        }
        else
        {
            //Debug.LogError("no list contains the languageunit with identifier: " + languageUnit.identifier);
            throw new Exception();
        }
        CalculateLanguageLevel();
    }

    public void AdjustWeightLetter(string letter, bool correct)
    {
        foreach(LanguageUnit languageUnit in letters)
        {
            if(languageUnit.identifier == letter)
            {
                AdjustWeight(languageUnit, correct);
                break;
            }
        }
    }

    public void AdjustWeightWord(string word, bool correct)
    {
        foreach(LanguageUnit languageUnit in words)
        {
            if(languageUnit.identifier == word)
            {
                AdjustWeight(languageUnit, correct);
                break;
            }
        }
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

    #region unitTesting
    /// <summary>
    /// Adds languageUnits to the words list. The method is intended for testing purpouses and should not be used in completed code
    /// </summary>
    /// <param name="languageUnit">the languageUnit to be added</param>
    public void AddWord(LanguageUnit languageUnit)
    {
        if(words == null)
        {
            words = new List<LanguageUnit>();
        }
        words.Add(languageUnit);
        if(properties == null)
        {
            properties = new List<Property>();
        }
        foreach(Property property in languageUnit.properties)
        {
            properties.Add(property);
        }
    }

    /// <summary>
    /// Adds languageUnits to the words list. The method is intended for testing purpouses and should not be used in completed code
    /// </summary>
    /// <param name="languageUnit">the languageUnit to be added</param>
    public void AddLetter(LanguageUnit languageUnit)
    {
        if(letters == null)
        {
            letters = new List<LanguageUnit>();
        }
        if(words == null)
        {
            words = new List<LanguageUnit>();
        }
        letters.Add(languageUnit);
        if(properties == null)
        {
            properties = new List<Property>();
        }
        foreach(Property property in languageUnit.properties)
        {
            properties.Add(property);
        }
    }

    /// <summary>
    /// Gets the list of words(For testing purpouses)
    /// </summary>
    /// <returns>the list of words</returns>
    public List<LanguageUnit> GetWords()
    {
        return words;
    }

    /// <summary>
    /// Gets the list of letters(For testing purpouses)
    /// </summary>
    /// <returns>the list of letters</returns>
    public List<LanguageUnit> GetLetters()
    {
        return letters;
    }

    /// <summary>
    /// Gets the list of properties(For testing purpouses)
    /// </summary>
    /// <returns>the list of properties</returns>
    public List<Property> GetProperties()
    {
        return properties;
    }

    /// <summary>
    /// Gets the playerLanguageLevel(For testing purpouses)
    /// </summary>
    /// <returns>the player languagelevel</returns>
    public int GetPlayerLevel()
    {
        return playerLanguageLevel;
    }

    /// <summary>
    /// Sets the playerLanguageLevel(For testing purpouses)
    /// </summary>
    /// <param name="level">the new value of playerLanguagageLevel</param>
    public void SetPlayerLevel(int level)
    {
        playerLanguageLevel = level;
    }
    #endregion
}