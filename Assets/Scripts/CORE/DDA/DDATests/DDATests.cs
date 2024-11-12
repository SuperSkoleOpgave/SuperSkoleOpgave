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
    #endregion
    #region TestMethods
    /// <summary>
    /// Ensures the AddWord and GetWords test methods works
    /// </summary>
    [Test]
    public void CanAddWord()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.word;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddWord(languageUnitUnderTest);

        Assert.Contains(languageUnitUnderTest, dDAUnderTest.GetWords());

    }

    /// <summary>
    /// Ensures the AddLetter and GetLetters test methods works
    /// </summary>
    [Test]
    public void CanAddLetter()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.letter;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddLetter(languageUnitUnderTest);

        Assert.Contains(languageUnitUnderTest, dDAUnderTest.GetLetters());
    }

    /// <summary>
    /// Ensures that properties are added to the property list from words
    /// </summary>
    [Test]
    public void CanAddPropertyFromWord()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.letter;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddWord(languageUnitUnderTest);

        Assert.Contains(propertyUnderTest, dDAUnderTest.GetProperties());
    }

    /// <summary>
    /// Ensures that properties are added to the property list from letters
    /// </summary>
    [Test]
    public void CanAddPropertyFromLetter()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.letter;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
    [Test]
    public void WeightUpdatedOnSingleProperty()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };

        languageUnitUnderTest.CalculateWeight();
        Assert.AreEqual(50, languageUnitUnderTest.weight);
    }

    [Test]
    public void WeightUpdatedOnMultipleProperties()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        languageUnitUnderTest.properties = new List<Property>();
        languageUnitUnderTest.identifier = "d";
        float totalWeight = 0;
        for(int i = 0; i < Random.Range(2, 20); i++)
        {
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = Random.Range(0, 100);
            totalWeight += propertyUnderTest.weight;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.properties.Add(propertyUnderTest);
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 1;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 100;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
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
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        Assert.Throws<Exception>(() => dDAUnderTest.AdjustWeight(languageUnitUnderTest, false));
    }
    #endregion
    #region IsLanguageUnitUnlocked
    /// <summary>
    /// Ensures the player can use properties with a required level lower than their own
    /// </summary>
    [Test]
    public void UnlockedIfLevelLockIsBelowPlayerLevel()
    {
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }

    /// <summary>
    /// Ensures the player can use properties with a required level equal to their own
    /// </summary>
    [Test]
    public void UnlockedIfLevelLockIsEqualToPlayerLevel()
    {
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 1;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }

    /// <summary>
    /// Ensures the player cant use properties with a required level higher than their own
    /// </summary>
    [Test]
    public void LockedIfLevelLockIsAbovePlayerLevel()
    {
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 2;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(false, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }
    #endregion
    #region GetLetter
    [Test]
    public void CanGetLetter()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddLetter(languageUnitUnderTest);


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetLetter(new List<Property>()));
    }
    [Test]
    public void CanGetHighestWeightedLetterInGetLetter()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 99;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 1;
        propertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "e";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddLetter(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnitUnderTest.identifier == dDAUnderTest.GetLetter(new List<Property>()).identifier)
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
    [Test]
    public void CanGetLowerWeightedLetterInGetLetter()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        lowerweightedPropertyUnderTest.property = property.testProperty;
        lowerweightedPropertyUnderTest.weight = 49;
        lowerweightedPropertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "e";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddLetter(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(LowerWeightedLanguageUnitUnderTest.identifier == dDAUnderTest.GetLetter(new List<Property>()).identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    [Test]
    public void EmptyLettersListThrowsExceptionInGetLetter()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetter(new List<Property>()));
    }
    #endregion
    #region GetLetters
    [Test]
    public void CanGetAllLetters()
    {
        int amount = Random.Range(1, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = "d";
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddLetter(languageUnitUnderTest);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetLetters(new List<Property>(), amount).Count);
    }
    [Test]
    public void CanGetSomeLetters()
    {
        int amount = Random.Range(2, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = "d";
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddLetter(languageUnitUnderTest);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetLetters(new List<Property>(), requestAmount).Count);
    }
    [Test]
    public void CanGetHighestWeightedLetterInGetLetters()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 99;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 1;
        propertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "e";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddLetter(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnitUnderTest.identifier == dDAUnderTest.GetLetters(new List<Property>(), 1)[0].identifier)
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
    [Test]
    public void CanGetLowerWeightedLetterInGetLetters()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        lowerweightedPropertyUnderTest.property = property.testProperty;
        lowerweightedPropertyUnderTest.weight = 49;
        lowerweightedPropertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "e";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddLetter(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(LowerWeightedLanguageUnitUnderTest.identifier == dDAUnderTest.GetLetters(new List<Property>(), 1)[0].identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    [Test]
    public void MultipleLettersDontGiveDuplicates()
    {
        int amount = Random.Range(2, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = i.ToString();
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddLetter(languageUnitUnderTest);
        }
        List<LanguageUnit> retrievedList = dDAUnderTest.GetLetters(new List<Property>(), amount);
        Assert.AreEqual(retrievedList.Count, retrievedList.Distinct().Count());
    }

    [Test]
    public void RequestAmountCantBeAboveListSize()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "d";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        Assert.Throws<Exception>(() => dDAUnderTest.GetLetters(new List<Property>(), 2));
    }

    [Test]
    public void EmptyLettersListThrowsExceptionInGetLetters()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetters(new List<Property>(), 1));
    }
    #endregion
    #region GetWord
    [Test]
    public void CanGetWord()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddWord(languageUnitUnderTest);


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetWord(new List<Property>()));
    }
    [Test]
    public void CanGetHighestWeightedWordInGetWord()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 99;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 1;
        propertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "ee";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddWord(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnitUnderTest.identifier == dDAUnderTest.GetWord(new List<Property>()).identifier)
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
    [Test]
    public void CanGetLowerWeightedWordInGetWord()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        lowerweightedPropertyUnderTest.property = property.testProperty;
        lowerweightedPropertyUnderTest.weight = 49;
        lowerweightedPropertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "ee";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddWord(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(LowerWeightedLanguageUnitUnderTest.identifier == dDAUnderTest.GetWord(new List<Property>()).identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    [Test]
    public void EmptyLettersListThrowsExceptionInGetWord()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWord(new List<Property>()));
    }
    #endregion
    #region GetWords
    [Test]
    public void CanGetAllWordss()
    {
        int amount = Random.Range(1, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = "dd";
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddWord(languageUnitUnderTest);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetWords(new List<Property>(), amount).Count);
    }
    [Test]
    public void CanGetSomeWords()
    {
        int amount = Random.Range(2, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = "d";
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddWord(languageUnitUnderTest);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetWords(new List<Property>(), requestAmount).Count);
    }
    [Test]
    public void CanGetHighestWeightedWordInGetWords()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 99;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 1;
        propertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "ee";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddWord(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        
        int highestWeightedRetrieved = 0;
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnitUnderTest.identifier == dDAUnderTest.GetWords(new List<Property>(), 1)[0].identifier)
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
    [Test]
    public void CanGetLowerWeightedWordInGetWords()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        LanguageUnit LowerWeightedLanguageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property lowerweightedPropertyUnderTest = new Property();
        lowerweightedPropertyUnderTest.property = property.testProperty;
        lowerweightedPropertyUnderTest.weight = 49;
        lowerweightedPropertyUnderTest.levelLock = 0;
        LowerWeightedLanguageUnitUnderTest.identifier = "ee";
        LowerWeightedLanguageUnitUnderTest.properties = new List<Property>
        {
            lowerweightedPropertyUnderTest
        };
        dDAUnderTest.AddWord(LowerWeightedLanguageUnitUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(LowerWeightedLanguageUnitUnderTest.identifier == dDAUnderTest.GetWords(new List<Property>(), 1)[0].identifier)
            {
                lowestWeigtedRetrieved++;
            }
        }
        Assert.Greater(lowestWeigtedRetrieved, 0);
    }

    [Test]
    public void MultipleWordsDontGiveDuplicates()
    {
        int amount = Random.Range(2, 20);
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            Property propertyUnderTest = new Property();
            propertyUnderTest.property = property.testProperty;
            propertyUnderTest.weight = 50;
            propertyUnderTest.levelLock = 0;
            languageUnitUnderTest.identifier = i.ToString();
            languageUnitUnderTest.properties = new List<Property>
            {
                propertyUnderTest
            };
            dDAUnderTest.AddWord(languageUnitUnderTest);
        }
        
        List<LanguageUnit> retrievedList = dDAUnderTest.GetWords(new List<Property>(), amount);
        Assert.AreEqual(retrievedList.Count, retrievedList.Distinct().Count());
    }

    [Test]
    public void RequestAmountCantBeAboveWordListSize()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties = new List<Property>
        {
            propertyUnderTest
        };
        dDAUnderTest.AddWord(languageUnitUnderTest);
        Assert.Throws<Exception>(() => dDAUnderTest.GetWords(new List<Property>(), 2));
    }

    [Test]
    public void EmptyWordsListThrowsExceptionInGetLetters()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWords(new List<Property>(), 1));
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
