using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

public class DDATests
{
    DynamicDifficultyAdjustment dDAUnderTest;
    #region Setup
    /// <summary>
    /// Loads the scene and sets up the test DDA
    /// </summary>
    [SetUp]
    public void Setup()
    {
       EditorSceneManager.OpenScene("Assets/Scripts/CORE/DDA/DDATests/DDATestScene.unity");
       var gameObject = GameObject.Find("GameObjectToTestFor");
       dDAUnderTest = gameObject.GetComponent<DynamicDifficultyAdjustment>();
    }

    private List<LanguageUnit> CreateLanguageUnits(int amount)
    {
        List<LanguageUnit> languageUnits = new List<LanguageUnit>();
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnit = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            languageUnit.properties = new List<property>();
            languageUnit.identifier = i.ToString();
            languageUnit.dynamicDifficultyAdjustment = dDAUnderTest;
            languageUnits.Add(languageUnit);
        }
        return languageUnits;
    }

    private List<Property> CreateProperties(int amount)
    {
        List<Property> properties = new List<Property>();
        for(int i = 0; i < amount; i++)
        {
            Property property = new Property();
            property.weight = 50;
            property.property = (property)i;
            property.levelLock = 0;
            dDAUnderTest.AddProperty(property);
            properties.Add(property);
        }
        return properties;
    }
    #endregion
    #region TestMethods
    /// <summary>
    /// Ensures the AddWord and GetWords test methods works
    /// </summary>
    [Test]
    public void CanAddWord()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        Assert.Contains(languageUnitUnderTest, dDAUnderTest.GetWords());

    }

    /// <summary>
    /// Ensures the AddLetter and GetLetters test methods works
    /// </summary>
    [Test]
    public void CanAddLetter()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        Assert.Contains(languageUnitUnderTest, dDAUnderTest.GetLetters());
    }

    /// <summary>
    /// Ensures that properties are added to the property list from words
    /// </summary>
    [Test]
    public void CanAddPropertyFromWord()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        Assert.Contains(propertyUnderTest, dDAUnderTest.GetProperties());
    }

    /// <summary>
    /// Ensures that properties are added to the property list from letters
    /// </summary>
    [Test]
    public void CanAddPropertyFromLetter()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        Assert.Contains(propertyUnderTest, dDAUnderTest.GetProperties());
    }

    /// <summary>
    /// Ensures the players level can be manually set
    /// </summary>
    [Test]
    public void CanChangePlayerLevel()
    {
        dDAUnderTest.SetPlayerLevel(4);
        Assert.AreEqual(4, dDAUnderTest.GetPlayerLevel());
    }
    #endregion
    #region LanguageUnit
    /// <summary>
    /// Checks that weight is correctly updated with one property
    /// </summary>
    [Test]
    public void WeightUpdatedOnSingleProperty()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);

        languageUnitUnderTest.CalculateWeight();
        Assert.AreEqual(50, languageUnitUnderTest.weight);
    }

    /// <summary>
    /// Checks weight is correctly updated on multiple properties
    /// </summary>
    [Test]
    public void WeightUpdatedOnMultipleProperties()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        List<Property> properties = CreateProperties(Random.Range(2, 20));
        float totalWeight = 0;
        foreach(Property property in properties)
        {
            languageUnitUnderTest.properties.Add(property.property);
            totalWeight += property.weight;
        }
        languageUnitUnderTest.CalculateWeight();
        Assert.AreEqual(totalWeight, languageUnitUnderTest.weight);
    }
    #endregion
    #region AdjustWeight
    /// <summary>
    /// Ensures the weight is adjusted upwards when a answer is wrong
    /// </summary>
    [Test]
    public void CanAdjustWeightofWordUpwards()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, false);

        Assert.AreEqual(50 + 1, propertyUnderTest.weight);
    }

    /// <summary>
    /// Ensures the weight is adjusted downwards if the answer is correct
    /// </summary>
    [Test]
    public void CanAdjustWeightofWordDownWards()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);

        Assert.AreEqual(50 - 1, propertyUnderTest.weight);
    }

    /// <summary>
    /// Ensures the weight cant be adjusted below 1
    /// </summary>
    [Test]
    public void WeightCantbeAdjustedBelowOne()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        propertyUnderTest.weight = 1;
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);

        Assert.AreEqual(1, propertyUnderTest.weight);
    }

    /// <summary>
    /// Ensures the weight cant be adjusted above 100
    /// </summary>
    [Test]
    public void WeightCantbeAdjustedAbove100()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        propertyUnderTest.weight = 100;
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, false);

        Assert.AreEqual(100, propertyUnderTest.weight);
    }

    /// <summary>
    /// Ensures weight can be adjusted for letters
    /// </summary>
    [Test]
    public void WeightCanBeAdjustedForLetters()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, false);

        Assert.AreEqual(50 + 1, propertyUnderTest.weight);
    }

    /// <summary>
    /// Ensures that langugageUnits are on one of the lists
    /// </summary>
    [Test]
    public void LanguageUnitMustBeOnAList()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        Assert.Throws<Exception>(() => dDAUnderTest.AdjustWeight(languageUnitUnderTest, false));
    }
    #endregion
    #region AdjustWeightLetter
    /// <summary>
    /// Ensures you can adjust a letters weight based on its identifier
    /// </summary>
    [Test]
    public void CanAdjustWeightofletter()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        dDAUnderTest.AdjustWeightLetter(languageUnitUnderTest.identifier, false);

        Assert.AreEqual(50 + 1, propertyUnderTest.weight);
    }
    /// <summary>
    /// Ensures you cant adjust weight of letters based on nonexistant letters
    /// </summary>
    [Test]
    public void CantAdjustWeightofnonExistantLetter()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        dDAUnderTest.AdjustWeightLetter("e", false);

        Assert.AreEqual(50, propertyUnderTest.weight);
    }
    #endregion
    #region AdjustWeightWord
    /// <summary>
    /// Ensures you can adjust a words weight based on its identifier
    /// </summary>
    [Test]
    public void CanAdjustWeightofWord()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeightWord(languageUnitUnderTest.identifier, false);

        Assert.AreEqual(50 + 1, propertyUnderTest.weight);
    }
    /// <summary>
    /// Ensures you cant adjust weight of words based on nonexistant words
    /// </summary>
    [Test]
    public void CantAdjustWeightofnonExistantWord()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        Property propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeightWord("ee", false);

        Assert.AreEqual(50, propertyUnderTest.weight);
    }
    #endregion
    #region IsLanguageUnitUnlocked
    /// <summary>
    /// Ensures the player can use properties with a required level lower than their own
    /// </summary>
    [Test]
    public void UnlockedIfLevelLockIsBelowPlayerLevel()
    {
        CreateProperties(1);
        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(0));
    }

    /// <summary>
    /// Ensures the player can use properties with a required level equal to their own
    /// </summary>
    [Test]
    public void UnlockedIfLevelLockIsEqualToPlayerLevel()
    {
        Property propertyUnderTest = CreateProperties(1)[0];
        propertyUnderTest.levelLock = 1;
        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(0));
    }

    /// <summary>
    /// Ensures the player cant use properties with a required level higher than their own
    /// </summary>
    [Test]
    public void LockedIfLevelLockIsAbovePlayerLevel()
    {
        Property propertyUnderTest = CreateProperties(1)[0];
        propertyUnderTest.levelLock = 2;
        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(false, dDAUnderTest.IsLanguageUnitTypeUnlocked(0));
    }
    #endregion
    #region GetLetter
    /// <summary>
    /// Checks letters can be retrieved
    /// </summary>
    [Test]
    public void CanGetLetter()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetLetter(new List<property>()));
    }
    /// <summary>
    /// Checks that the highest weighted letter is the most likely to be returned
    /// </summary>
    [Test]
    public void CanGetHighestWeightedLetterInGetLetter()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 99;
        properties[1].weight = 1;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[0].identifier == dDAUnderTest.GetLetter(new List<property>()).identifier)
            {
                highestWeightedRetrieved++;
            }
            else
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(highestWeightedRetrieved, lowestWeigtedRetrieved);
    }
    /// <summary>
    /// Checks that a lower weighted letter can be returned
    /// </summary>
    [Test]
    public void CanGetLowerWeightedLetterInGetLetter()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetLetter(new List<property>()).identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    /// <summary>
    /// Ensures an error is thrown then the letters list is empty
    /// </summary>
    [Test]
    public void EmptyLettersListThrowsExceptionInGetLetter()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetter(new List<property>()));
    }

    /// <summary>
    /// Ensures you only get letters with a property when using it to filter letters
    /// </summary>
    [Test]
    public void CanFilterLettersBasedOnSingleProperty()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<property>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures you only get letters with all filter properties0
    /// </summary>
    [Test]
    public void CanFilterLettersBasedOnMultipleProperties()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<property>(){properties[0].property, properties[1].property}));
    }
    #endregion
    #region GetLetters
    /// <summary>
    /// Ensures you can retrieve all letters on the list 
    /// </summary>
    [Test]
    public void CanGetAllLetters()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetLetters(new List<property>(), amount).Count);
    }
    /// <summary>
    /// Ensures you can get some of the letters in the list
    /// </summary>
    [Test]
    public void CanGetSomeLetters()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetLetters(new List<property>(), requestAmount).Count);
    }
    /// <summary>
    /// Ensures the  highest weighted letter is the most likely when using getLetters
    /// </summary>
    [Test]
    public void CanGetHighestWeightedLetterInGetLetters()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 99;
        properties[1].weight = 1;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[0].identifier == dDAUnderTest.GetLetters(new List<property>(), 1)[0].identifier)
            {
                highestWeightedRetrieved++;
            }
            else
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(highestWeightedRetrieved, lowestWeigtedRetrieved);
    }
    /// <summary>
    /// Ensures lower weighted letters can be retrieved when using GetLetters
    /// </summary>
    [Test]
    public void CanGetLowerWeightedLetterInGetLetters()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetLetters(new List<property>(), 1)[0].identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    /// <summary>
    /// Ensures you dont get duplicate letters when requesting multiple letters
    /// </summary>
    [Test]
    public void MultipleLettersDontGiveDuplicates()
    {
        int amount = Random.Range(2, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        List<LanguageUnit> retrievedList = dDAUnderTest.GetLetters(new List<property>(), amount);
        Assert.AreEqual(retrievedList.Count, retrievedList.Distinct().Count());
    }

    /// <summary>
    /// Ensures you cant request more letters than there are on the list
    /// </summary>
    [Test]
    public void RequestAmountCantBeAboveListSize()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        Assert.Throws<Exception>(() => dDAUnderTest.GetLetters(new List<property>(), 2));
    }

    /// <summary>
    /// Ensures an empty list throws an exception when using getLetters
    /// </summary>
    [Test]
    public void EmptyLettersListThrowsExceptionInGetLetters()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetters(new List<property>(), 1));
    }
    #endregion
    #region GetWord
    /// <summary>
    /// Ensures you can get a word
    /// </summary>
    [Test]
    public void CanGetWord()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddWord(languageUnitUnderTest);


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetWord(new List<property>()));
    }
    /// <summary>
    /// Ensures you are most likely to get the highest weighted word
    /// </summary>
    [Test]
    public void CanGetHighestWeightedWordInGetWord()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 99;
        properties[1].weight = 1;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[0].identifier == dDAUnderTest.GetWord(new List<property>()).identifier)
            {
                highestWeightedRetrieved++;
            }
            else
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(highestWeightedRetrieved, lowestWeigtedRetrieved);
    }

    /// <summary>
    /// Ensures you can get lower weighted words
    /// </summary>
    [Test]
    public void CanGetLowerWeightedWordInGetWord()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetWord(new List<property>()).identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    /// <summary>
    /// Ensures an empty words list throws an exception
    /// </summary>
    [Test]
    public void EmptyWordsListThrowsExceptionInGetWord()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWord(new List<property>()));
    }

    /// <summary>
    /// Ensures you only get words with a property when using it to filter words
    /// </summary>
    [Test]
    public void CanFilterWordsBasedOnSingleProperty()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<property>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures you only get words with all filter properties0
    /// </summary>
    [Test]
    public void CanFilterWordsBasedOnMultipleProperties()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<property>(){properties[0].property, properties[1].property}));
    }
    #endregion
    #region GetWords
    /// <summary>
    /// Ensures you can get all words on the list
    /// </summary>
    [Test]
    public void CanGetAllWords()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetWords(new List<property>(), amount).Count);
    }
    /// <summary>
    /// Ensures you can get some of the words on the list
    /// </summary>
    [Test]
    public void CanGetSomeWords()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetWords(new List<property>(), requestAmount).Count);
    }
    /// <summary>
    /// Ensures the highest weighted word is the most likely to get
    /// </summary>
    [Test]
    public void CanGetHighestWeightedWordInGetWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 99;
        properties[1].weight = 1;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[0].identifier == dDAUnderTest.GetWords(new List<property>(), 1)[0].identifier)
            {
                highestWeightedRetrieved++;
            }
            else
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(highestWeightedRetrieved, lowestWeigtedRetrieved);
    }
    /// <summary>
    /// Ensures you can get lower weighted words
    /// </summary>
    [Test]
    public void CanGetLowerWeightedWordInGetWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<Property> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetWords(new List<property>(), 1)[0].identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    /// <summary>
    /// Ensures you dont get duplicates when requesting multiple words
    /// </summary>
    [Test]
    public void MultipleWordsDontGiveDuplicates()
    {
        int amount = Random.Range(2, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<Property> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        
        List<LanguageUnit> retrievedList = dDAUnderTest.GetWords(new List<property>(), amount);
        Assert.AreEqual(retrievedList.Count, retrievedList.Distinct().Count());
    }

    /// <summary>
    /// Ensures you get an exception when requesting more words than there are on the list
    /// </summary>
    [Test]
    public void RequestAmountCantBeAboveWordListSize()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        languageUnitUnderTest.properties.Add(CreateProperties(1)[0].property);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        Assert.Throws<Exception>(() => dDAUnderTest.GetWords(new List<property>(), 2));
    }

    /// <summary>
    /// Ensures an empty word list throws an exception
    /// </summary>
    [Test]
    public void EmptyWordsListThrowsExceptionInGetWords()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWords(new List<property>(), 1));
    }
    #endregion
    /// <summary>
    /// Cleans up after tests are done
    /// </summary>
    [TearDown]
    public void Teardown()
    {
       EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
