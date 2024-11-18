using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    List<LanguageUnitPropertyInfo> properties;

    List<languageUnitProperty> averagedProperties = new List<languageUnitProperty>()
    {
        languageUnitProperty.word,
        languageUnitProperty.letter,
        languageUnitProperty.vowel,
        languageUnitProperty.consonant
    };

    Dictionary<languageUnitProperty, int> levelLocks;
    int playerLanguageLevel;

    /// <summary>
    /// returns a letter, using properties given
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public LanguageUnit GetLetter(List<languageUnitProperty> properties)
    {
        if(letters == null)
        {
            throw new Exception("could not find any letters");
        }
        float totalweight = 0;
        List<LanguageUnit> filteredLetters = FilterList(letters, properties);
        foreach (LanguageUnit letter in filteredLetters)
        {
            letter.CalculateWeight();
            totalweight += letter.weight;
        }
        float rand = Random.Range(0f, totalweight);
        float cumulativeWeight = 0;
        foreach (LanguageUnit letter in filteredLetters)
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
    public List<LanguageUnit> GetLetters(List<languageUnitProperty> properties, int count)
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
    public LanguageUnit GetWord(List<languageUnitProperty> properties)
    {
        if(words == null)
        {
            throw new Exception("could not find any words");
        }
        float totalweight = 0;
        List<LanguageUnit> filteredWords = FilterList(words, properties);
        foreach (LanguageUnit word in filteredWords)
        {
            word.CalculateWeight();
            totalweight += word.weight;
        }
        float rand = Random.Range(0f, totalweight);
        float cumulativeWeight = 0;
        foreach (LanguageUnit word in filteredWords)
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
    public List<LanguageUnit> GetWords(List<languageUnitProperty> properties, int count)
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
    /// Creates a list of languageunits which all have properties from a given list
    /// </summary>
    /// <param name="listToFilter">The list which should be filtered</param>
    /// <param name="filterProperties">The properties which languageunits should have to be on the resulting list</param>
    /// <returns>A list of language units which all have some specific properties</returns>
    /// <exception cref="Exception">Throws an exception if no languageunits with the given properties could be found</exception>
    private List<LanguageUnit> FilterList(List<LanguageUnit> listToFilter, List<languageUnitProperty> filterProperties)
    {
        List<LanguageUnit> filteredList = new List<LanguageUnit>();
        foreach(LanguageUnit languageUnit in listToFilter)
        {
            bool hasFilterProperty = true;
            foreach(languageUnitProperty property in filterProperties)
            {
                if(!languageUnit.properties.Contains(property))
                {
                    hasFilterProperty = false;
                    break;
                }
            }
            if(hasFilterProperty)
            {
                filteredList.Add(languageUnit);
            }
        }
        if(filteredList.Count == 0)
        {
            throw new Exception("could not find any languageunits with the given properties");
        }
        return filteredList;
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
            foreach(languageUnitProperty property in languageUnit.properties)
            {
                if(!averagedProperties.Contains(property))
                {
                    LanguageUnitPropertyInfo foundProperty = FindOrCreateProperty(property);
                    
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
        CalculateAveragedProperties();
        CalculateLanguageLevel();
    }

    /// <summary>
    /// Calculates the weight of various properties which are an average of other properties
    /// </summary>
    private void CalculateAveragedProperties()
    {
        Dictionary<languageUnitProperty, float> sums = new Dictionary<languageUnitProperty, float>();
        Dictionary<languageUnitProperty, int> amounts = new Dictionary<languageUnitProperty, int>();
        for(int i = 0; i < averagedProperties.Count; i++)
        {
            sums.Add(averagedProperties[i],0);
            amounts.Add(averagedProperties[i], 0);
        }
        foreach(LanguageUnit letter in letters)
        {
            letter.CalculateWeight();
            foreach(languageUnitProperty property in averagedProperties)
            {
                if(letter.properties.Contains(property))
                {
                    sums[property] += letter.weight;
                    amounts[property]++;
                }
            }
        }
        foreach(LanguageUnit word in words)
        {
            word.CalculateWeight();
            foreach(languageUnitProperty property in averagedProperties)
            {
                if(word.properties.Contains(property))
                {
                    sums[property] += word.weight;
                    amounts[property]++;
                }
            }
        }
        foreach(languageUnitProperty property in averagedProperties)
        {
            if(amounts[property] > 0)
            {
                LanguageUnitPropertyInfo averagedProperty = FindOrCreateProperty(property);
                averagedProperty.weight = sums[property] / amounts[property];
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
    public bool IsLanguageUnitTypeUnlocked(languageUnitProperty property)
    {
        return FindOrCreateProperty(property).levelLock <= playerLanguageLevel;
    }

    /// <summary>
    /// Returns the default player priority
    /// </summary>
    /// <returns>a list of properties</returns>
    public List<languageUnitProperty> GetPlayerPriority()
    {
        List<LanguageUnitPropertyInfo> languageUnitProperties = new List<LanguageUnitPropertyInfo>();
        foreach(languageUnitProperty languageUnitProperty in averagedProperties)
        {
            languageUnitProperties.Add(FindOrCreateProperty(languageUnitProperty));
        }
        languageUnitProperties = languageUnitProperties.OrderBy(p=>p.weight).ToList();
        List<languageUnitProperty> sortedProperties = languageUnitProperties.Select(p => p.property).ToList();
        return sortedProperties;
    }

    /// <summary>
    /// Gets the weight of the given property
    /// </summary>
    /// <param name="property">The property to get the weight of</param>
    /// <returns>The weight of the property</returns>
    public float GetPropertyWeight(languageUnitProperty property)
    {
        LanguageUnitPropertyInfo foundProperty = FindOrCreateProperty(property);
        if(averagedProperties.Contains(foundProperty.property))
        {
            return 0;
        }
        else
        {
            return foundProperty.weight;
        }
        
    }

    private void Load()
    {

    } 

    private void Save()
    {

    }

    /// <summary>
    /// Loads in language units from the bootstrapper. For letters it also sets up their properties and replaces the placeholder text for danish letters with the appropiate letter
    /// </summary>
    /// <param name="letters">The letters to use in the DDA</param>
    /// <param name="words">The words to use in the DDA</param>
    public void SetupLanguageUnits(List<LanguageUnit> letters, List<LanguageUnit> words)
    {
        Debug.Log("setting up languageunits with " + letters.Count + " letters and " + words.Count + " words");
        levelLocks = new Dictionary<languageUnitProperty, int>();
        foreach(LanguageUnit languageUnit in letters)
        {
            if(languageUnit.identifier[0] == '(')
            {
                switch(languageUnit.identifier)
                {
                    case "(AA)":
                        languageUnit.identifier = "\u00c5";
                        break;
                    case "(AE)":
                        languageUnit.identifier = "\u00c6";
                        break;
                    case "(OE)":
                        languageUnit.identifier = "\u00d8";
                        break;
                }
            }
            languageUnit.dynamicDifficultyAdjustment = this;
            foreach (languageUnitProperty property in languageUnit.properties)
            {
                FindOrCreateProperty(property);
            }
        }
        foreach(LanguageUnit languageUnit in words)
        {
            if(languageUnit.identifier[0] == '(')
            {
                switch(languageUnit.identifier)
                {
                    case "(AA)":
                        languageUnit.identifier = "\u00c5";
                        break;
                    case "(AE)":
                        languageUnit.identifier = "\u00c6";
                        break;
                    case "(OE)":
                        languageUnit.identifier = "\u00d8";
                        break;
                }
            }
            languageUnit.dynamicDifficultyAdjustment = this;
            foreach (languageUnitProperty property in languageUnit.properties)
            {
                FindOrCreateProperty(property);
            }
        }
        this.letters = letters;

        this.words = words;
    }


    private void CalculateLanguageLevel()
    {

    }

    /// <summary>
    /// Tries to find the related object for a given property and if it cant find it creates it.
    /// </summary>
    /// <param name="property">The property to get the object for</param>
    /// <returns>The object for the given property</returns>
    private LanguageUnitPropertyInfo FindOrCreateProperty(languageUnitProperty property)
    {
        LanguageUnitPropertyInfo foundProperty = null;
        if(properties == null)
        {
            properties = new List<LanguageUnitPropertyInfo>();
        }
        foreach(LanguageUnitPropertyInfo p in properties)
        {
            if(property == p.property)
            {
                foundProperty = p;
                break;
            }
        }
        if(foundProperty == null)
        {
            foundProperty = new LanguageUnitPropertyInfo();
            foundProperty.property = property;
            foundProperty.weight = 50;
            foundProperty.levelLock = 0;
            if(levelLocks.Keys.Contains(property))
            {
                foundProperty.levelLock = levelLocks[property];
            }
            properties.Add(foundProperty);
        }
        return foundProperty;
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
            properties = new List<LanguageUnitPropertyInfo>();
        }
        foreach(languageUnitProperty property in languageUnit.properties)
        {
            LanguageUnitPropertyInfo foundProperty = null;
            foreach(LanguageUnitPropertyInfo p in properties)
            {
                if(p.property == property)
                {
                    foundProperty = p;
                    break;
                }
            }
            if(foundProperty == null)
            {
                foundProperty = new LanguageUnitPropertyInfo();
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
    public List<LanguageUnitPropertyInfo> GetProperties()
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
            properties = new List<LanguageUnitPropertyInfo>();
        }
        foreach(languageUnitProperty property in languageUnit.properties)
        {
            LanguageUnitPropertyInfo foundProperty = null;
            foreach(LanguageUnitPropertyInfo p in properties)
            {
                if(p.property == property)
                {
                    foundProperty = p;
                    break;
                }
            }
            if(foundProperty == null)
            {
                foundProperty = new LanguageUnitPropertyInfo();
                foundProperty.property = property;
                foundProperty.levelLock = 0;
                foundProperty.weight = 50;
            }
        }
    }

    public void AddProperty(LanguageUnitPropertyInfo property)
    {
        if(properties == null)
        {
            properties = new List<LanguageUnitPropertyInfo>();
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