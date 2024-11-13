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
    public LanguageUnit GetLetter(List<property> properties)
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
    public List<LanguageUnit> GetLetters(List<property> properties, int count)
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
    public LanguageUnit GetWord(List<property> properties)
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
    public List<LanguageUnit> GetWords(List<property> properties, int count)
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
            foreach(property property in languageUnit.properties)
            {
                if(!nonWeightedProperties.Contains(property))
                {
                    Property foundProperty = null;
                    foreach(Property p in properties)
                    {
                        if(p.property == property)
                        {
                            foundProperty = p;
                            break;
                        }
                    }
                    if(foundProperty == null)
                    {
                        foundProperty = new Property();
                        foundProperty.property = property;
                        foundProperty.levelLock = 0;
                        foundProperty.weight = 50;
                        properties.Add(foundProperty);
                    }
                    
                    if(correct && foundProperty.weight > 1)
                    {
                        foundProperty.weight -= 1;
                    }
                    else if(foundProperty.weight < 100 && !correct)
                    {
                        foundProperty.weight += 1;
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
    /// Adjust the weight of a language unit in the letters list based on its identifier
    /// </summary>
    /// <param name="letter">The identifer of the language unit to adjust</param>
    /// <param name="correct">Whether the player did something correct</param>
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

    /// <summary>
    /// Adjust the weight of a language unit in the words list based on its identifier
    /// </summary>
    /// <param name="word">The identifer of the language unit to adjust</param>
    /// <param name="correct">Whether the player did something correct</param>
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
    public bool IsLanguageUnitTypeUnlocked(property property)
    {
        Property foundProperty = null;
        foreach(Property p in properties)
        {
            if(property == p.property)
            {
                foundProperty = p;
                break;
            }
        }
        if(foundProperty == null)
        {
            foundProperty = new Property();
            foundProperty.property = property;
            foundProperty.weight = 50;
            foundProperty.levelLock = 0;
            properties.Add(foundProperty);
        }
        return foundProperty.levelLock <= playerLanguageLevel;
    }

    /// <summary>
    /// Not implemented yet
    /// </summary>
    /// <returns>an empty list</returns>
    public List<property> GetPlayerPriority()
    {
        return new List<property>();
    }

    /// <summary>
    /// Gets the weight of the given property
    /// </summary>
    /// <param name="property">The property to get the weight of</param>
    /// <returns>The weight of the property</returns>
    public float GetPropertyWeight(property property)
    {
        Property foundProperty = null;
        foreach(Property p in properties)
        {
            if(property == p.property)
            {
                foundProperty = p;
                break;
            }
        }
        if(foundProperty == null)
        {
            foundProperty = new Property();
            foundProperty.property = property;
            foundProperty.weight = 50;
            foundProperty.levelLock = 0;
            properties.Add(foundProperty);
        }
        return foundProperty.weight;
    }

    private void Load()
    {

    } 

    private void Save()
    {

    }

    public void SetupLanguageUnits(List<LanguageUnit> letters, List<LanguageUnit> words)
    {
        this.letters = letters;
        this.words = words;
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
        languageUnit.dynamicDifficultyAdjustment = this;
        words.Add(languageUnit);
        if(properties == null)
        {
            properties = new List<Property>();
        }
        foreach(property property in languageUnit.properties)
        {
            Property foundProperty = null;
            foreach(Property p in properties)
            {
                if(p.property == property)
                {
                    foundProperty = p;
                    break;
                }
            }
            if(foundProperty == null)
            {
                foundProperty = new Property();
                foundProperty.property = property;
                foundProperty.levelLock = 0;
                foundProperty.weight = 50;
            }
        }
    }

    /// <summary>
    /// Gets the list of properties
    /// </summary>
    /// <returns>the list of properties</returns>
    public List<Property> GetProperties()
    {
        return properties;
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
        languageUnit.dynamicDifficultyAdjustment = this;
        letters.Add(languageUnit);
        if(properties == null)
        {
            properties = new List<Property>();
        }
        foreach(property property in languageUnit.properties)
        {
            Property foundProperty = null;
            foreach(Property p in properties)
            {
                if(p.property == property)
                {
                    foundProperty = p;
                    break;
                }
            }
            if(foundProperty == null)
            {
                foundProperty = new Property();
                foundProperty.property = property;
                foundProperty.levelLock = 0;
                foundProperty.weight = 50;
            }
        }
    }

    public void AddProperty(Property property)
    {
        if(properties == null)
        {
            properties = new List<Property>();
        }
        properties.Add(property);
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