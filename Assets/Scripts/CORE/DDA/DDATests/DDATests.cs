using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

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

    /// <summary>
    /// Cleans up after tests are done
    /// </summary>
    [TearDown]
    public void Teardown()
    {
       EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
