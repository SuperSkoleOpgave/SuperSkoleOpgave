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
    [SetUp]
    public void Setup()
    {
       EditorSceneManager.OpenScene("Assets/Scripts/CORE/DDA/DDATests/DDATestScene.unity");
       var gameObject = GameObject.Find("GameObjectToTestFor");
       dDAUnderTest = gameObject.GetComponent<DynamicDifficultyAdjustment>();
    }
    #endregion
    #region TestMethods
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

    [Test]
    public void CanAddProperty()
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

    [Test]
    public void CanChangePlayerLevel()
    {
        dDAUnderTest.SetPlayerLevel(4);
        Assert.AreEqual(4, dDAUnderTest.GetPlayerLevel());
    }
    #endregion

    #region AdjustWeight
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
    #endregion
    #region IsLanguageUnitUnlocked
    [Test]
    public void UnlockedIfLevelLockIsBelowPlayerLevel()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }

    [Test]
    public void UnlockedIfLevelLockIsEqualToPlayerLevel()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 1;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(true, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }

    [Test]
    public void LockedIfLevelLockIsAbovePlayerLevel()
    {
        LanguageUnit languageUnitUnderTest = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 2;
        

        dDAUnderTest.SetPlayerLevel(1);

        Assert.AreEqual(false, dDAUnderTest.IsLanguageUnitTypeUnlocked(propertyUnderTest));
    }
    #endregion

    [TearDown]
    public void Teardown()
    {
       EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
