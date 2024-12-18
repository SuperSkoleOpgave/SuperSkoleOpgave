using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DynamicDifficultyAdjustment : MonoBehaviour
{
    List<LanguageUnit> words;
    List<LanguageUnit> letters;
    List<LanguageUnitPropertyInfo> properties;

    public bool underTest = false;

    List<LanguageUnitProperty> averagedProperties = new List<LanguageUnitProperty>()
    {
        LanguageUnitProperty.vowel,
        LanguageUnitProperty.consonant,
        LanguageUnitProperty.letter,
        LanguageUnitProperty.word,
        LanguageUnitProperty.vowelConfuse,
        LanguageUnitProperty.softD,
        LanguageUnitProperty.doubleConsonant,
        LanguageUnitProperty.silentConsonant
    };

    Dictionary<LanguageUnitProperty, int> levelLocks;
    Dictionary<char, LanguageUnitProperty> letterProperties;
    Dictionary<char, LanguageUnitProperty> wordLetterProperties;
    int playerLanguageLevel = 0;

    public List<LanguageUnitPropertyInfo> Properties
    {  
        get { return properties; } 
        set { properties = value; } 
    }

    /// <summary>
    /// returns a letter, using properties given
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public LanguageUnit GetLetter(List<LanguageUnitProperty> properties)
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
    public List<LanguageUnit> GetLetters(List<LanguageUnitProperty> properties, int count)
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
    public LanguageUnit GetWord(List<LanguageUnitProperty> properties)
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
    public List<LanguageUnit> GetWords(List<LanguageUnitProperty> properties, int count)
    {
        if(words == null)
        {
            throw new Exception("could not find any words");
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
    private List<LanguageUnit> FilterList(List<LanguageUnit> listToFilter, List<LanguageUnitProperty> filterProperties)
    {
        List<LanguageUnit> filteredList = new List<LanguageUnit>();
        foreach(LanguageUnit languageUnit in listToFilter)
        {
            bool hasFilterProperty = true;
            foreach(LanguageUnitProperty property in filterProperties)
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
            foreach(LanguageUnitProperty property in languageUnit.properties)
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
            throw new Exception("no list contains the languageunit with identifier: " + languageUnit.identifier);
        }
        CalculateAveragedProperties();
        CalculateLanguageLevel();
    }

    /// <summary>
    /// Calculates the weight of various properties which are an average of other properties
    /// </summary>
    private void CalculateAveragedProperties()
    {
        Dictionary<LanguageUnitProperty, float> sums = new Dictionary<LanguageUnitProperty, float>();
        Dictionary<LanguageUnitProperty, int> amounts = new Dictionary<LanguageUnitProperty, int>();
        for(int i = 0; i < averagedProperties.Count; i++)
        {
            sums.Add(averagedProperties[i],0);
            amounts.Add(averagedProperties[i], 0);
        }
        foreach(LanguageUnit letter in letters)
        {
            letter.CalculateWeight();
            foreach(LanguageUnitProperty property in averagedProperties)
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
            foreach(LanguageUnitProperty property in averagedProperties)
            {
                if(word.properties.Contains(property))
                {
                    sums[property] += word.weight;
                    amounts[property]++;
                }
            }
        }
        foreach(LanguageUnitProperty property in averagedProperties)
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
    public bool IsLanguageUnitTypeUnlocked(LanguageUnitProperty property)
    {
        return FindOrCreateProperty(property).levelLock <= playerLanguageLevel;
    }

    /// <summary>
    /// Returns the default player priority
    /// </summary>
    /// <returns>a list of properties</returns>
    public List<LanguageUnitProperty> GetPlayerPriority()
    {
        List<LanguageUnitPropertyInfo> languageUnitProperties = new List<LanguageUnitPropertyInfo>();
        foreach(LanguageUnitProperty languageUnitProperty in averagedProperties)
        {
            if(IsLanguageUnitTypeUnlocked(languageUnitProperty))
            {
                languageUnitProperties.Add(FindOrCreateProperty(languageUnitProperty));
            }
        }
        languageUnitProperties = languageUnitProperties.OrderBy(p=> -p.weight).ToList();
        List<LanguageUnitProperty> sortedProperties = languageUnitProperties.Select(p => p.property).ToList();
        return sortedProperties;
    }

    /// <summary>
    /// Gets the weight of the given property
    /// </summary>
    /// <param name="property">The property to get the weight of</param>
    /// <returns>The weight of the property</returns>
    public float GetPropertyWeight(LanguageUnitProperty property)
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

    /// <summary>
    /// Returns an averaged property's weight
    /// </summary>
    /// <param name="property">The property to get the weight of</param>
    /// <returns>The weight of the given property</returns>
    /// <exception cref="Exception">An Exception is thrown if the property is not on the AveragedProperties list</exception>
    public float GetAveragedPropertyWeight(LanguageUnitProperty property)
    {
        LanguageUnitPropertyInfo foundProperty = FindOrCreateProperty(property);
        if(averagedProperties.Contains(foundProperty.property))
        {
            return foundProperty.weight;
        }
        else
        {
            throw new Exception("given property is not an averaged property. Use GetPropertyWeight Instead");
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
        SetupLetterProperties();
        levelLocks = new Dictionary<LanguageUnitProperty, int>();
        SetupLevelLocks();
        foreach(LanguageUnit languageUnit in letters)
        {
            languageUnit.identifier = languageUnit.identifier.ToLower();
            if(languageUnit.identifier[0] == '(')
            {
                switch(languageUnit.identifier)
                {
                    case "(aa)":
                        languageUnit.identifier = "\u00e5";
                        break;
                    case "(ae)":
                        languageUnit.identifier = "\u00e6";
                        break;
                    case "(oe)":
                        languageUnit.identifier = "\u00f8";
                        break;
                }
            }
            languageUnit.dynamicDifficultyAdjustment = this;
            if(languageUnit.properties.Count > 3)
            {
                throw new Exception("Too many properties");
            }
            
            foreach (LanguageUnitProperty property in languageUnit.properties)
            {
                FindOrCreateProperty(property);
            }
        }
        foreach(LanguageUnit languageUnit in words)
        {
            languageUnit.identifier = languageUnit.identifier.ToLower();
            if(languageUnit.identifier.Contains("(aa)"))
            {
                languageUnit.identifier = languageUnit.identifier.Replace("(aa)", "\u00e5");
            }
            
            if(languageUnit.identifier.Contains("(ae)"))
            {
                languageUnit.identifier = languageUnit.identifier.Replace("(ae)", "\u00e6");
            }
            if(languageUnit.identifier.Contains("(oe)"))
            {
                languageUnit.identifier = languageUnit.identifier.Replace("(oe)", "\u00f8");
            }
            languageUnit.dynamicDifficultyAdjustment = this;
            foreach(char letter in languageUnit.identifier.ToLower())
            {
                
                if(!languageUnit.properties.Contains(wordLetterProperties[letter]))
                {
                    languageUnit.properties.Add(wordLetterProperties[letter]);
                }
            }
            foreach (LanguageUnitProperty property in languageUnit.properties)
            {
                FindOrCreateProperty(property);
            }
        }
        this.letters = letters;

        this.words = words;
    }
    public void SetupLetterProperties()
    {
        letterProperties = new Dictionary<char, LanguageUnitProperty>();
        wordLetterProperties = new Dictionary<char, LanguageUnitProperty>();
        letterProperties.Add('a', LanguageUnitProperty.letterA);
        wordLetterProperties.Add('a', LanguageUnitProperty.wordWithA);
        letterProperties.Add('b', LanguageUnitProperty.letterB);
        wordLetterProperties.Add('b', LanguageUnitProperty.wordWithB);
        letterProperties.Add('c', LanguageUnitProperty.letterC);
        wordLetterProperties.Add('c', LanguageUnitProperty.wordWithC);
        letterProperties.Add('d', LanguageUnitProperty.letterD);
        wordLetterProperties.Add('d', LanguageUnitProperty.wordWithD);
        letterProperties.Add('e', LanguageUnitProperty.letterE);
        wordLetterProperties.Add('e', LanguageUnitProperty.wordWithE);
        letterProperties.Add('f', LanguageUnitProperty.letterF);
        wordLetterProperties.Add('f', LanguageUnitProperty.wordWithF);
        letterProperties.Add('g', LanguageUnitProperty.letterG);
        wordLetterProperties.Add('g', LanguageUnitProperty.wordWithG);
        letterProperties.Add('h', LanguageUnitProperty.letterH);
        wordLetterProperties.Add('h', LanguageUnitProperty.wordWithH);
        letterProperties.Add('i', LanguageUnitProperty.letterI);
        wordLetterProperties.Add('i', LanguageUnitProperty.wordWithI);
        letterProperties.Add('j', LanguageUnitProperty.letterJ);
        wordLetterProperties.Add('j', LanguageUnitProperty.wordWithJ);
        letterProperties.Add('k', LanguageUnitProperty.letterK);
        wordLetterProperties.Add('k', LanguageUnitProperty.wordWithK);
        letterProperties.Add('l', LanguageUnitProperty.letterL);
        wordLetterProperties.Add('l', LanguageUnitProperty.wordWithL);
        letterProperties.Add('m', LanguageUnitProperty.letterM);
        wordLetterProperties.Add('m', LanguageUnitProperty.wordWithM);
        letterProperties.Add('n', LanguageUnitProperty.letterN);
        wordLetterProperties.Add('n', LanguageUnitProperty.wordWithN);
        letterProperties.Add('o', LanguageUnitProperty.letterO);
        wordLetterProperties.Add('o', LanguageUnitProperty.wordWithO);
        letterProperties.Add('p', LanguageUnitProperty.letterP);
        wordLetterProperties.Add('p', LanguageUnitProperty.wordWithP);
        letterProperties.Add('q', LanguageUnitProperty.letterQ);
        wordLetterProperties.Add('q', LanguageUnitProperty.wordWithQ);
        letterProperties.Add('r', LanguageUnitProperty.letterR);
        wordLetterProperties.Add('r', LanguageUnitProperty.wordWithR);
        letterProperties.Add('s', LanguageUnitProperty.letterS);
        wordLetterProperties.Add('s', LanguageUnitProperty.wordWithS);
        letterProperties.Add('t', LanguageUnitProperty.letterT);
        wordLetterProperties.Add('t', LanguageUnitProperty.wordWithT);
        letterProperties.Add('u', LanguageUnitProperty.letterU);
        wordLetterProperties.Add('u', LanguageUnitProperty.wordWithU);
        letterProperties.Add('v', LanguageUnitProperty.letterV);
        wordLetterProperties.Add('v', LanguageUnitProperty.wordWithV);
        letterProperties.Add('w', LanguageUnitProperty.letterW);
        wordLetterProperties.Add('w', LanguageUnitProperty.wordWithW);
        letterProperties.Add('x', LanguageUnitProperty.letterX);
        wordLetterProperties.Add('x', LanguageUnitProperty.wordWithX);
        letterProperties.Add('y', LanguageUnitProperty.letterY);
        wordLetterProperties.Add('y', LanguageUnitProperty.wordWithY);
        letterProperties.Add('z', LanguageUnitProperty.letterZ);
        wordLetterProperties.Add('z', LanguageUnitProperty.wordWithZ);
        letterProperties.Add('\u00c6', LanguageUnitProperty.letterAE);
        wordLetterProperties.Add('\u00c6', LanguageUnitProperty.wordWithAE);
        letterProperties.Add('\u00e6', LanguageUnitProperty.letterAE);
        wordLetterProperties.Add('\u00e6', LanguageUnitProperty.wordWithAE);
        letterProperties.Add('\u00d8', LanguageUnitProperty.letterOE);
        wordLetterProperties.Add('\u00d8', LanguageUnitProperty.wordWithOE);
        letterProperties.Add('\u00f8', LanguageUnitProperty.letterOE);
        wordLetterProperties.Add('\u00f8', LanguageUnitProperty.wordWithOE);
        letterProperties.Add('\u00c5', LanguageUnitProperty.letterAA);
        wordLetterProperties.Add('\u00c5', LanguageUnitProperty.wordWithAA);
        letterProperties.Add('\u00e5', LanguageUnitProperty.letterAA);
        wordLetterProperties.Add('\u00e5', LanguageUnitProperty.wordWithAA);
    }
    public LanguageUnitProperty GetWordLetterProperty(char letter)
    {
        return wordLetterProperties[letter];
    }
    private void CalculateLanguageLevel()
    {
        switch(playerLanguageLevel)
        {
            case 0:
                if(GetAveragedPropertyWeight(LanguageUnitProperty.vowel) <= 45)
                {
                    playerLanguageLevel++;
                }
                break;
            case 1:
                if(GetAveragedPropertyWeight(LanguageUnitProperty.consonant) <= 45)
                {
                    playerLanguageLevel++;
                }
                break;
            case 2:
                if(GetAveragedPropertyWeight(LanguageUnitProperty.letter) <= 40)
                {
                    playerLanguageLevel++;
                }
                break;
            case 3:
                break;
            default:
                throw new Exception("Player level has not been implemented");
        }
    }

    /// <summary>
    /// Tries to find the related object for a given property and if it cant find it creates it.
    /// </summary>
    /// <param name="property">The property to get the object for</param>
    /// <returns>The object for the given property</returns>
    private LanguageUnitPropertyInfo FindOrCreateProperty(LanguageUnitProperty property)
    {
        LanguageUnitPropertyInfo foundProperty = null;
        if(properties == null)
        {
            properties = new List<LanguageUnitPropertyInfo>();
        }
        if(levelLocks == null)
        {
            levelLocks = new Dictionary<LanguageUnitProperty, int>();
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

    public void SetupLevelLocks()
    {
        if(levelLocks == null)
        {
            levelLocks = new Dictionary<LanguageUnitProperty, int>();
        }
        levelLocks.Add(LanguageUnitProperty.consonant, 1);
        levelLocks.Add(LanguageUnitProperty.letter, 2);
        levelLocks.Add(LanguageUnitProperty.word, 3);
        levelLocks.Add(LanguageUnitProperty.vowelConfuse, 3);
        levelLocks.Add(LanguageUnitProperty.softD, 3);
        levelLocks.Add(LanguageUnitProperty.doubleConsonant, 3);
        levelLocks.Add(LanguageUnitProperty.silentConsonant, 3);
    }

    public List<string> GetWordStrings()
    {
        return words.Select(p => p.identifier).ToList();
    }

    /// <summary>
    /// Gets the playerLanguageLevel
    /// </summary>
    /// <returns>the player languagelevel</returns>
    public int GetPlayerLevel()
    {
        return playerLanguageLevel;
    }

    #region unitTesting
    /// <summary>
    /// Adds languageUnits to the words list. The method is intended for testing purpouses and should not be used in completed code
    /// </summary>
    /// <param name="languageUnit">the languageUnit to be added</param>
    public void AddWord(LanguageUnit languageUnit)
    {
        if(!underTest)
        {
            throw new Exception("The method AddWord(LanguageUnit languageUnit) is for testing purposes only");
        }
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
        
        foreach(LanguageUnitProperty property in languageUnit.properties)
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

    public List<LanguageUnitProperty> GetLetterProperty(char letter)
    {
        if(!underTest)
        {
            throw new Exception("The method GetLetterProperty(char letter) is for testing purposes only");
        }
        return new List<LanguageUnitProperty>()
        {
            letterProperties[letter]
        };
    }

    public List<LanguageUnitProperty> GetWordProperties(string word)
    {
        if(!underTest)
        {
            throw new Exception("The method GetWordProperties(string word) is for testing purposes only");
        }
        List<LanguageUnitProperty> properties = new List<LanguageUnitProperty>();
        foreach(char letter in word)
        {
            if(!properties.Contains(wordLetterProperties[letter]))
            {
                properties.Add(wordLetterProperties[letter]);
            }
        }
        return properties;
    }

    /// <summary>
    /// Gets the list of properties
    /// </summary>
    /// <returns>the list of properties</returns>
    public List<LanguageUnitPropertyInfo> GetProperties()
    {
        if(!underTest)
        {
            throw new Exception("The method GetProperties() is for testing purposes only");
        }
        return properties;
    }

    /// <summary>
    /// Adds languageUnits to the words list. The method is intended for testing purpouses and should not be used in completed code
    /// </summary>
    /// <param name="languageUnit">the languageUnit to be added</param>
    public void AddLetter(LanguageUnit languageUnit)
    {
        if(!underTest)
        {
            throw new Exception("The method AddLetter(LanguageUnit languageUnit) is for testing purposes only");
        }
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
        
        foreach(LanguageUnitProperty property in languageUnit.properties)
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
        if(!underTest)
        {
            throw new Exception("The method AddProperty(LanguageUnitPropertyInfo property) is for testing purposes only");
        }
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
    public List<LanguageUnit> GetWordList()
    {
        if(!underTest)
        {
            throw new Exception("The method GetWordList() is for testing purposes only");
        }
        return words;
    }

    /// <summary>
    /// Gets the list of letters(For testing purpouses)
    /// </summary>
    /// <returns>the list of letters</returns>
    public List<LanguageUnit> GetLetterList()
    {
        if(!underTest)
        {
            throw new Exception("The method GetLetterList() is for testing purposes only");
        }
        return letters;
    }

    

    

    /// <summary>
    /// Sets the playerLanguageLevel(For testing purpouses)
    /// </summary>
    /// <param name="level">the new value of playerLanguagageLevel</param>
    public void SetPlayerLevel(int level)
    {
        if(!underTest)
        {
            throw new Exception("The method SetPlayerLevel(int level) is for testing purposes only");
        }
        playerLanguageLevel = level;
    }
    #endregion
}