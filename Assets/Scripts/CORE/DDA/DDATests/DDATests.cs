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
    DDAWordSetter dDAWordSetterUnderTest;
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
       dDAWordSetterUnderTest = gameObject.GetComponent<DDAWordSetter>();
    }

    /// <summary>
    /// Creates a given number of empty languageUnits
    /// </summary>
    /// <param name="amount">how many LanguageUnits to be created</param>
    /// <returns>a list of languageunits</returns>
    private List<LanguageUnit> CreateLanguageUnits(int amount)
    {
        List<LanguageUnit> languageUnits = new List<LanguageUnit>();
        for(int i = 0; i < amount; i++)
        {
            LanguageUnit languageUnit = (LanguageUnit)ScriptableObject.CreateInstance("LanguageUnit");
            languageUnit.properties = new List<LanguageUnitProperty>();
            languageUnit.identifier = i.ToString();
            languageUnit.dynamicDifficultyAdjustment = dDAUnderTest;
            languageUnits.Add(languageUnit);
        }
        return languageUnits;
    }

    /// <summary>
    /// Create a LanguageUnitPropertyInfo with default values
    /// </summary>
    /// <param name="amount">how many LanguageUnitPropertyInfo should be created</param>
    /// <returns>a list of LanguageUnitPropertyInfo</returns>
    private List<LanguageUnitPropertyInfo> CreateProperties(int amount)
    {
        List<LanguageUnitPropertyInfo> properties = new List<LanguageUnitPropertyInfo>();
        for(int i = 0; i < amount; i++)
        {
            LanguageUnitPropertyInfo property = new LanguageUnitPropertyInfo();
            property.weight = 50;
            property.property = (LanguageUnitProperty)i;
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(Random.Range(2, 20));
        float totalWeight = 0;
        foreach(LanguageUnitPropertyInfo property in properties)
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        Assert.Throws<Exception>(() => dDAUnderTest.AdjustWeight(languageUnitUnderTest, false));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with a single languageunit
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnSingleLanguageUnitPropertyAndAveragedProperty()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(dDAUnderTest.GetPropertyWeight(languageUnitUnderTest.properties[0]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[1]));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with a single languageunit
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnSingleLanguageUnitMultiplePropertiesAndSingleAveragedProperty()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitUnderTest.properties.Add(propertiesUnderTest[1].property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(dDAUnderTest.GetPropertyWeight(languageUnitUnderTest.properties[0]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[1]));
    }
    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with a single languageunit
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnSingleLanguageUnitPropertyAndMultipleAveragedProperties()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.letter);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.vowel);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[2]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[1]));
    }
    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsSinglePropertyAndSingleAveragedProperty()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitsUnderTest[0].properties.Add(propertyUnderTest.property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(propertyUnderTest.property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[0], true);
        Assert.AreEqual(dDAUnderTest.GetPropertyWeight(languageUnitsUnderTest[0].properties[0]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[1]));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits and multiple properties which all languageUnits have
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsPropertiesAndSingleAveragedPropertyBothContainsAll()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[1].property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[1].properties.Add(propertiesUnderTest[1].property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[0], true);
        float weight = (languageUnitsUnderTest[0].weight + languageUnitsUnderTest[1].weight) / 2;
        Assert.AreEqual(weight, dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[2]));
    }
    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits and multiple properties where some have all 
    /// and adjustweight is run on the one with all properties 
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsPropertiesAndSingleAveragedPropertyOneContainsAllOneAdjustWeightBothProperties()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[1].property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[0], true);
        float weight = (languageUnitsUnderTest[0].weight + languageUnitsUnderTest[1].weight) / 2;
        Assert.AreEqual(weight, dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[2]));
    }
    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits and multiple properties where some have all 
    /// and adjustweight is run one the one with some properties
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsPropertiesAndSingleAveragedPropertyOneContainsAllOneAdjustWeightOneProperty()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[1].property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.letter);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[1], true);
        float weight = (languageUnitsUnderTest[0].weight + languageUnitsUnderTest[1].weight) / 2;
        Assert.AreEqual(weight, dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[2]));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits a single property and multiple averaged properties which all languageunits have
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsSinglePropertyAndMultipleAveragedProperties()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitsUnderTest[0].properties.Add(propertyUnderTest.property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.vowel);
        languageUnitsUnderTest[1].properties.Add(propertyUnderTest.property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.vowel);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[0], true);
        Assert.AreEqual(dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[1]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[2]));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with a single languageunit
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnSingleLanguageUnitMultiplePropertiesAndAveragedProperty()
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitUnderTest.properties.Add(propertiesUnderTest[1].property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.letter);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.vowel);
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[2]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitUnderTest.properties[1]));
    }

    /// <summary>
    /// Ensures that an averaged propertys weight is calculated correctly with multiple languageunits and multiple properties where some have all 
    /// and adjustweight is run one the one with some properties
    /// </summary>
    [Test]
    public void AveragedPropertiesCalculatedCorrectlyOnMultipleLanguageUnitsPropertiesAndAveragedPropertySplitBetweenLAnguageUnits()
    {
        List<LanguageUnit> languageUnitsUnderTest = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> propertiesUnderTest = CreateProperties(2);
        languageUnitsUnderTest[0].properties.Add(propertiesUnderTest[0].property);
        languageUnitsUnderTest[0].properties.Add(LanguageUnitProperty.letter);
        languageUnitsUnderTest[1].properties.Add(propertiesUnderTest[1].property);
        languageUnitsUnderTest[1].properties.Add(LanguageUnitProperty.vowel);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[0]);
        dDAUnderTest.AddLetter(languageUnitsUnderTest[1]);
        dDAUnderTest.AdjustWeight(languageUnitsUnderTest[1], true);
        Assert.AreNotEqual(dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[1].properties[1]), dDAUnderTest.GetAveragedPropertyWeight(languageUnitsUnderTest[0].properties[1]));
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
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


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetLetter(new List<LanguageUnitProperty>()));
    }
    /// <summary>
    /// Checks that the highest weighted letter is the most likely to be returned
    /// </summary>
    [Test]
    public void CanGetHighestWeightedLetterInGetLetter()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
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
            if(languageUnits[0].identifier == dDAUnderTest.GetLetter(new List<LanguageUnitProperty>()).identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetLetter(new List<LanguageUnitProperty>()).identifier)
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
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetter(new List<LanguageUnitProperty>()));
    }

    /// <summary>
    /// Ensures you only get letters with a property when using it to filter letters
    /// </summary>
    [Test]
    public void CanFilterLettersBasedOnSingleProperty()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures you only get letters with all filter properties
    /// </summary>
    [Test]
    public void CanFilterLettersBasedOnMultipleProperties()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[0].property, properties[1].property}));
    }

    /// <summary>
    /// Ensures you get an exception if using a property for which no letters exists
    /// </summary>
    [Test]
    public void GettingAnEmptyLetterListThrowsAnException()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(1);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(1);
        languageUnits[0].properties.Add(properties[0].property);
        dDAUnderTest.AddLetter(languageUnits[0]);

        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){LanguageUnitProperty.wordWithE}));
    }

    /// <summary>
    /// Ensures when you filter words that you only get letters 
    /// </summary>
    [Test]
    public void CanOnlyGetLetters()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures the order of filter properties doesn't matter
    /// </summary>
    [Test]
    public void FilterOrderDoesntMatterForLetters()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(1);
        languageUnits[0].properties.Add(properties[0].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[0].property, properties[0].property}));
    }

    /// <summary>
    /// Ensures duplicate filter properties has no effect on the result
    /// </summary>
    [Test]
    public void DuplicateFilterPropertiesHasNoEffectOnResult()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(3);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        languageUnits[2].properties.Add(properties[0].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        dDAUnderTest.AddLetter(languageUnits[2]);
        Assert.AreEqual(dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[0].property, properties[1].property}), dDAUnderTest.GetLetter(new List<LanguageUnitProperty>(){properties[1].property, properties[0].property}));
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), amount).Count);
    }
    /// <summary>
    /// Ensures you can get some of the letters in the list
    /// </summary>
    [Test]
    public void CanGetSomeLetters()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), requestAmount).Count);
    }
    /// <summary>
    /// Ensures the  highest weighted letter is the most likely when using getLetters
    /// </summary>
    [Test]
    public void CanGetHighestWeightedLetterInGetLetters()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
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
            if(languageUnits[0].identifier == dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), 1)[0].identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), 1)[0].identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddLetter(languageUnits[i]);
        }
        List<LanguageUnit> retrievedList = dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), amount);
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
        Assert.Throws<Exception>(() => dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), 2));
    }

    /// <summary>
    /// Ensures an empty list throws an exception when using getLetters
    /// </summary>
    [Test]
    public void EmptyLettersListThrowsExceptionInGetLetters()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetLetters(new List<LanguageUnitProperty>(), 1));
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


        Assert.AreEqual(languageUnitUnderTest, dDAUnderTest.GetWord(new List<LanguageUnitProperty>()));
    }
    /// <summary>
    /// Ensures you are most likely to get the highest weighted word
    /// </summary>
    [Test]
    public void CanGetHighestWeightedWordInGetWord()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
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
            if(languageUnits[0].identifier == dDAUnderTest.GetWord(new List<LanguageUnitProperty>()).identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetWord(new List<LanguageUnitProperty>()).identifier)
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
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWord(new List<LanguageUnitProperty>()));
    }

    /// <summary>
    /// Ensures you only get words with a property when using it to filter words
    /// </summary>
    [Test]
    public void CanFilterWordsBasedOnSingleProperty()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures you get an exception if using a property for which no words exists
    /// </summary>
    [Test]
    public void GettingAnEmptyWordListThrowsAnException()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(1);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(1);
        languageUnits[0].properties.Add(properties[0].property);
        dDAUnderTest.AddWord(languageUnits[0]);

        Assert.Throws<Exception>(()=> dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){LanguageUnitProperty.wordWithE}));
    }

    /// <summary>
    /// Ensures when you filter words that you only get words 
    /// </summary>
    [Test]
    public void CanOnlyGetWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddLetter(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[0].property}));
    }

    /// <summary>
    /// Ensures you only get words with all filter properties
    /// </summary>
    [Test]
    public void CanFilterWordsBasedOnMultipleProperties()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 1;
        properties[1].weight = 99;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[0].property, properties[1].property}));
    }

    /// <summary>
    /// Ensures duplicate filter properties has no effect on the result
    /// </summary>
    [Test]
    public void DuplicateFilterPropertiesHasNoEffectOnResultForWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(1);
        languageUnits[0].properties.Add(properties[0].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(languageUnits[0], dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[0].property, properties[0].property}));
    }

    /// <summary>
    /// Ensures the order of filter properties doesn't matter
    /// </summary>
    [Test]
    public void FilterOrderDoesntMatterForWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(3);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[0].properties.Add(properties[1].property);
        languageUnits[1].properties.Add(properties[1].property);
        languageUnits[2].properties.Add(properties[0].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        dDAUnderTest.AddWord(languageUnits[2]);
        Assert.AreEqual(dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[0].property, properties[1].property}), dDAUnderTest.GetWord(new List<LanguageUnitProperty>(){properties[1].property, properties[0].property}));
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        


        Assert.AreEqual(amount, dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), amount).Count);
    }
    /// <summary>
    /// Ensures you can get some of the words on the list
    /// </summary>
    [Test]
    public void CanGetSomeWords()
    {
        int amount = Random.Range(1, 20);
        List<LanguageUnit> languageUnits = CreateLanguageUnits(amount);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        
        int requestAmount = Random.Range(1, amount);

        Assert.AreEqual(requestAmount, dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), requestAmount).Count);
    }
    /// <summary>
    /// Ensures the highest weighted word is the most likely to get
    /// </summary>
    [Test]
    public void CanGetHighestWeightedWordInGetWords()
    {
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
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
            if(languageUnits[0].identifier == dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), 1)[0].identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(2);
        properties[0].weight = 50;
        properties[1].weight = 49;
        languageUnits[0].properties.Add(properties[0].property);
        languageUnits[1].properties.Add(properties[1].property);
        dDAUnderTest.AddWord(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        
        int lowestWeigtedRetrieved = 0;
        for(int i = 0; i < 100; i++)
        {
            if(languageUnits[1].identifier == dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), 1)[0].identifier)
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
        List<LanguageUnitPropertyInfo> properties = CreateProperties(amount);
        for(int i = 0; i < amount; i++)
        {
            languageUnits[i].properties.Add(properties[i].property);
            dDAUnderTest.AddWord(languageUnits[i]);
        }
        
        List<LanguageUnit> retrievedList = dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), amount);
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
        Assert.Throws<Exception>(() => dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), 2));
    }

    /// <summary>
    /// Ensures an empty word list throws an exception
    /// </summary>
    [Test]
    public void EmptyWordsListThrowsExceptionInGetWords()
    {
        Assert.Throws<Exception>(()=> dDAUnderTest.GetWords(new List<LanguageUnitProperty>(), 1));
    }
    #endregion
    #region GetPlayerPriority
    /// <summary>
    /// Ensures you can get the default order if all have the same weight
    /// </summary>
    [Test]
    public void CanGetDefaultCompleteOrder()
    {
        List<LanguageUnitProperty> languageUnitProperties = new List<LanguageUnitProperty>()
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
        dDAUnderTest.SetupLevelLocks();
        dDAUnderTest.SetPlayerLevel(3);
        Assert.AreEqual(languageUnitProperties, dDAUnderTest.GetPlayerPriority());
    }

    /// <summary>
    /// Ensures errors ensures the relevant property moves up in priority
    /// </summary>
    [Test]
    public void ErrorsMovePropertiesUpInPriority()
    {
        List<LanguageUnitProperty> languageUnitProperties = new List<LanguageUnitProperty>()
        {
            LanguageUnitProperty.word,
            LanguageUnitProperty.vowel,
            LanguageUnitProperty.consonant,
            LanguageUnitProperty.letter,
            LanguageUnitProperty.vowelConfuse,
            LanguageUnitProperty.softD,
            LanguageUnitProperty.doubleConsonant,
            LanguageUnitProperty.silentConsonant
        };
        dDAUnderTest.SetupLevelLocks();
        dDAUnderTest.SetPlayerLevel(3);
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.word);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, false);
        Assert.AreEqual(languageUnitProperties, dDAUnderTest.GetPlayerPriority());
    }

    /// <summary>
    /// Ensures doing something correct moves the relevant property down in priority 
    /// </summary>
    [Test]
    public void CorrectMovePropertiesDownInPriority()
    {
        List<LanguageUnitProperty> languageUnitProperties = new List<LanguageUnitProperty>()
        {
            LanguageUnitProperty.consonant,
            LanguageUnitProperty.letter,
            LanguageUnitProperty.word,
            LanguageUnitProperty.vowelConfuse,
            LanguageUnitProperty.softD,
            LanguageUnitProperty.doubleConsonant,
            LanguageUnitProperty.silentConsonant,
            LanguageUnitProperty.vowel
        };
        dDAUnderTest.SetupLevelLocks();
        dDAUnderTest.SetPlayerLevel(3);
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        LanguageUnitPropertyInfo propertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(propertyUnderTest.property);
        languageUnitUnderTest.properties.Add(LanguageUnitProperty.vowel);
        dDAUnderTest.AddWord(languageUnitUnderTest);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(languageUnitProperties, dDAUnderTest.GetPlayerPriority());
    }

    [Test]
    public void OnlyReturnsPropertiesThePlayerHasAccessTo()
    {
        List<LanguageUnitProperty> languageUnitProperties = new List<LanguageUnitProperty>()
        {
            LanguageUnitProperty.vowel
        };
        dDAUnderTest.SetupLevelLocks();
        Assert.AreEqual(languageUnitProperties, dDAUnderTest.GetPlayerPriority());
    }
    #endregion
    #region CalculatePlayerLevel
    /// <summary>
    /// Ensures the player can level up to level 1
    /// </summary>
    [Test]
    public void PlayerCanLevelUpToLevel1()
    {
        TestLevelUp(LanguageUnitProperty.vowel, 1, 46);
    }
    /// <summary>
    /// Ensures the player can level up to level 2
    /// </summary>
    [Test]
    public void PlayerCanLevelUpToLevel2()
    {
        TestLevelUp(LanguageUnitProperty.consonant, 2, 46);
    }
    
    /// <summary>
    /// Ensures the player can level up to level 3
    /// </summary>
    [Test]
    public void PlayerCanLevelUpToLevel3()
    {
        TestLevelUp(LanguageUnitProperty.letter, 3, 41);
    }

    /// <summary>
    /// Common structure for level up tests
    /// </summary>
    /// <param name="property">the property whos weight must be reduced for level up</param>
    /// <param name="targetLevel">the level the player should hit</param>
    /// <param name="startWeight">The weight the property should start at</param>
    private void TestLevelUp(LanguageUnitProperty property, int targetLevel, float startWeight)
    {
        LanguageUnit languageUnitUnderTest = CreateLanguageUnits(1)[0];
        LanguageUnitPropertyInfo languageUnitPropertyUnderTest = CreateProperties(1)[0];
        languageUnitUnderTest.properties.Add(languageUnitPropertyUnderTest.property);
        languageUnitUnderTest.properties.Add(property);
        languageUnitPropertyUnderTest.weight = startWeight;
        dDAUnderTest.AddLetter(languageUnitUnderTest);
        dDAUnderTest.SetPlayerLevel(targetLevel - 1);
        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);
        Assert.AreEqual(targetLevel, dDAUnderTest.GetPlayerLevel());
    }
    #endregion
    #region SetupLanguageUnits
    /// <summary>
    /// Test that letter properties are correctly set up for a given letter
    /// </summary>
    /// <param name="letter">The letter to be tested</param>
    private void TestLetter(char letter)
    {
        dDAUnderTest.SetupLetterProperties();
        List<LanguageUnit> languageUnits = CreateLanguageUnits(2);
        languageUnits[0].identifier = letter.ToString();
        languageUnits[0].properties = dDAUnderTest.GetLetterProperty(letter);
        languageUnits[1].identifier = letter.ToString();
        languageUnits[1].properties = dDAUnderTest.GetWordProperties(letter.ToString());
        dDAUnderTest.AddLetter(languageUnits[0]);
        dDAUnderTest.AddWord(languageUnits[1]);
        Assert.AreEqual(true, VerifyLetter(letter, dDAUnderTest.GetLetters()[0].properties[0]));
        Assert.AreEqual(true, VerifyLetter(letter, dDAUnderTest.GetWords()[0].properties[0])); 
    }
    
    /// <summary>
    /// Verify that the given property is correct for the letter
    /// </summary>
    /// <param name="letter">The letter the property is connected to</param>
    /// <param name="letterProperty">The letter property to be checked</param>
    /// <returns></returns>
    private bool VerifyLetter(char letter, LanguageUnitProperty letterProperty)
    {
        switch(letter)
        {
            case 'a':
                return letterProperty == LanguageUnitProperty.wordWithA || letterProperty == LanguageUnitProperty.letterA;
            case 'b':
                return letterProperty == LanguageUnitProperty.wordWithB || letterProperty == LanguageUnitProperty.letterB;
            case 'c':
                return letterProperty == LanguageUnitProperty.wordWithC || letterProperty == LanguageUnitProperty.letterC;
            case 'd':
                return letterProperty == LanguageUnitProperty.wordWithD || letterProperty == LanguageUnitProperty.letterD;
            case 'e':
                return letterProperty == LanguageUnitProperty.wordWithE || letterProperty == LanguageUnitProperty.letterE;
            case 'f':
                return letterProperty == LanguageUnitProperty.wordWithF || letterProperty == LanguageUnitProperty.letterF;
            case 'g':
                return letterProperty == LanguageUnitProperty.wordWithG || letterProperty == LanguageUnitProperty.letterG;
            case 'h':
                return letterProperty == LanguageUnitProperty.wordWithH || letterProperty == LanguageUnitProperty.letterH;
            case 'i':
                return letterProperty == LanguageUnitProperty.wordWithI || letterProperty == LanguageUnitProperty.letterI;
            case 'j':
                return letterProperty == LanguageUnitProperty.wordWithJ || letterProperty == LanguageUnitProperty.letterJ;
            case 'k':
                return letterProperty == LanguageUnitProperty.wordWithK || letterProperty == LanguageUnitProperty.letterK;
            case 'l':
                return letterProperty == LanguageUnitProperty.wordWithL || letterProperty == LanguageUnitProperty.letterL;
            case 'm':
                return letterProperty == LanguageUnitProperty.wordWithM || letterProperty == LanguageUnitProperty.letterM;
            case 'n':
                return letterProperty == LanguageUnitProperty.wordWithN || letterProperty == LanguageUnitProperty.letterN;
            case 'o':
                return letterProperty == LanguageUnitProperty.wordWithO || letterProperty == LanguageUnitProperty.letterO;
            case 'p':
                return letterProperty == LanguageUnitProperty.wordWithP || letterProperty == LanguageUnitProperty.letterP;
            case 'q':
                return letterProperty == LanguageUnitProperty.wordWithQ || letterProperty == LanguageUnitProperty.letterQ;
            case 'r':
                return letterProperty == LanguageUnitProperty.wordWithR || letterProperty == LanguageUnitProperty.letterR;
            case 's':
                return letterProperty == LanguageUnitProperty.wordWithS || letterProperty == LanguageUnitProperty.letterS;
            case 't':
                return letterProperty == LanguageUnitProperty.wordWithT || letterProperty == LanguageUnitProperty.letterT;
            case 'u':
                return letterProperty == LanguageUnitProperty.wordWithU || letterProperty == LanguageUnitProperty.letterU;
            case 'v':
                return letterProperty == LanguageUnitProperty.wordWithV || letterProperty == LanguageUnitProperty.letterV;
            case 'w':
                return letterProperty == LanguageUnitProperty.wordWithW || letterProperty == LanguageUnitProperty.letterW;
            case 'x':
                return letterProperty == LanguageUnitProperty.wordWithX || letterProperty == LanguageUnitProperty.letterX;
            case 'y':
                return letterProperty == LanguageUnitProperty.wordWithY || letterProperty == LanguageUnitProperty.letterY;
            case 'z':
                return letterProperty == LanguageUnitProperty.wordWithZ || letterProperty == LanguageUnitProperty.letterZ;
            case '\u00c6':
                return letterProperty == LanguageUnitProperty.wordWithAE || letterProperty == LanguageUnitProperty.letterAE;
            case '\u00d8':
                return letterProperty == LanguageUnitProperty.wordWithOE || letterProperty == LanguageUnitProperty.letterOE;
            case '\u00c5':
                return letterProperty == LanguageUnitProperty.wordWithAA || letterProperty == LanguageUnitProperty.letterAA;
        }
        return false;
    }
    [Test]
    public void TestAddingA()
    {
        TestLetter('a');
    }
    [Test]
    public void TestAddingB()
    {
        TestLetter('b');
    }
    [Test]
    public void TestAddingC()
    {
        TestLetter('c');
    }
    [Test]
    public void TestAddingD()
    {
        TestLetter('d');
    }
    [Test]
    public void TestAddingE()
    {
        TestLetter('e');
    }
    [Test]
    public void TestAddingF()
    {
        TestLetter('f');
    }
    [Test]
    public void TestAddingG()
    {
        TestLetter('g');
    }
    [Test]
    public void TestAddingH()
    {
        TestLetter('h');
    }
    [Test]
    public void TestAddingI()
    {
        TestLetter('i');
    }
    [Test]
    public void TestAddingJ()
    {
        TestLetter('j');
    }
    [Test]
    public void TestAddingK()
    {
        TestLetter('k');
    }
    [Test]
    public void TestAddingL()
    {
        TestLetter('l');
    }
    [Test]
    public void TestAddingM()
    {
        TestLetter('m');
    }
    [Test]
    public void TestAddingN()
    {
        TestLetter('n');
    }
    [Test]
    public void TestAddingO()
    {
        TestLetter('o');
    }
    [Test]
    public void TestAddingP()
    {
        TestLetter('p');
    }
    [Test]
    public void TestAddingQ()
    {
        TestLetter('q');
    }
    [Test]
    public void TestAddingR()
    {
        TestLetter('r');
    }
    [Test]
    public void TestAddingS()
    {
        TestLetter('s');
    }
    [Test]
    public void TestAddingT()
    {
        TestLetter('t');
    }
    [Test]
    public void TestAddingU()
    {
        TestLetter('u');
    }
    [Test]
    public void TestAddingV()
    {
        TestLetter('v');
    }
    [Test]
    public void TestAddingW()
    {
        TestLetter('w');
    }
    [Test]
    public void TestAddingX()
    {
        TestLetter('x');
    }
    [Test]
    public void TestAddingY()
    {
        TestLetter('y');
    }
    [Test]
    public void TestAddingZ()
    {
        TestLetter('z');
    }
    [Test]
    public void TestAddingAE()
    {
        TestLetter('\u00c6');
    }
    [Test]
    public void TestAddingOE()
    {
        TestLetter('\u00d8');
    }
    [Test]
    public void TestAddingAA()
    {
        TestLetter('\u00c5');
    }

    /// <summary>
    /// Ensures that letters are added
    /// </summary>
    [Test]
    public void LettersGetsAdded()
    {
        List<LanguageUnit> languageUnitUnderTest = CreateLanguageUnits(1);
        languageUnitUnderTest[0].identifier = "a";
        dDAUnderTest.SetupLanguageUnits(languageUnitUnderTest, new List<LanguageUnit>());
        Assert.AreEqual(1, dDAUnderTest.GetLetters().Count);
    }

    /// <summary>
    /// Ensures words are added
    /// </summary>
    [Test]
    public void WordsGetsAdded()
    {
        List<LanguageUnit> languageUnitUnderTest = CreateLanguageUnits(1);
        languageUnitUnderTest[0].identifier = "a";
        dDAUnderTest.SetupLanguageUnits(new List<LanguageUnit>(), languageUnitUnderTest);
        Assert.AreEqual(1, dDAUnderTest.GetWords().Count);
    }

    /// <summary>
    /// Ensures the letter property is added for a letter
    /// </summary>
    [Test]
    public void LetterPropertyGetsAddedForLetters()
    {
        List<LanguageUnit> languageUnitUnderTest = CreateLanguageUnits(1);
        languageUnitUnderTest[0].identifier = "a";
        dDAUnderTest.SetupLanguageUnits(languageUnitUnderTest, new List<LanguageUnit>());
        Assert.AreEqual(LanguageUnitProperty.letterA, dDAUnderTest.GetLetters()[0].properties[0]);
    }

    /// <summary>
    /// Ensures the letter properties are added for words
    /// </summary>
    [Test]
    public void LetterPropertyGetsAddedForWords()
    {
        List<LanguageUnit> languageUnitUnderTest = CreateLanguageUnits(1);
        languageUnitUnderTest[0].identifier = "hav";
        dDAUnderTest.SetupLanguageUnits(new List<LanguageUnit>(), languageUnitUnderTest);
        foreach(char letter in languageUnitUnderTest[0].identifier)
        {
            Assert.AreEqual(true, dDAUnderTest.GetWords()[0].properties.Contains(dDAUnderTest.GetWordLetterProperty(letter)));
        }
        
    }

    /// <summary>
    /// Ensures words get added to the DDAWordSetter
    /// </summary>
    [Test]
    public void WordGetsAddedToDDAWordSetter()
    {
        Texture2D word = AddWordToDDaWordSetter("test", false, false, false, false);
        Assert.AreEqual(true, dDAWordSetterUnderTest.WordsContainsWord(word));
    }

    /// <summary>
    /// Ensures Vowel confused words gets added to the vowelConfusedWords list
    /// </summary>
    [Test]
    public void VowelConfusedWordGetsAddedToDDAWordSetter()
    {
        Texture2D word = AddWordToDDaWordSetter("test", true, false, false, false);
        Assert.AreEqual(true, dDAWordSetterUnderTest.VowelConfusedWordsContainsWord(word));
    }

    /// <summary>
    /// Ensures double consonant words gets added to the DoubleConsonantWords list
    /// </summary>
    [Test]
    public void DoubleConsonantWordGetsAddedToDDAWordSetter()
    {
        Texture2D word = AddWordToDDaWordSetter("test", false, true, false, false);
        Assert.AreEqual(true, dDAWordSetterUnderTest.DoubleConsonantWordsContainsWord(word));
    }

    /// <summary>
    /// Ensures Soft D Words words gets added to the SoftDWords list
    /// </summary>
    [Test]
    public void SoftDWordGetsAddedToDDAWordSetter()
    {
        Texture2D word = AddWordToDDaWordSetter("test", false, false, true, false);
        Assert.AreEqual(true, dDAWordSetterUnderTest.SoftDWordsContainsWord(word));
    }

    /// <summary>
    /// Ensures silent consonant Words words gets added to the SilentConsonantWords list
    /// </summary>
    [Test]
    public void SilentConsonantWordGetsAddedToDDAWordSetter()
    {
        Texture2D word = AddWordToDDaWordSetter("test", false, false, false, true);
        Assert.AreEqual(true, dDAWordSetterUnderTest.SilentConsonantWordsContainsWord(word));
    }

    /// <summary>
    /// Ensures letters from the DDAwordsetter gets added to the DDA
    /// </summary>
    [Test]
    public void LettersFromDDAWordSetterGetsAddedToDDA()
    {
        LanguageUnit letter = CreateLanguageUnits(1)[0];
        letter.identifier = "a";
        dDAWordSetterUnderTest.AddLetter(letter);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(1, dDAUnderTest.GetLetters().Count);
    }

    /// <summary>
    /// Ensures words from the DDAWordSetter gets added to the DDA
    /// </summary>
    [Test]
    public void WordsFromDDAWordSetterGetsAddedToDDA()
    {
        string word = "test";
        AddWordToDDaWordSetter(word, false, false, false, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(1, dDAUnderTest.GetWords().Count);
    }

    /// <summary>
    /// Ensures a vowel confused word has the correct property in the DDA after getting added
    /// </summary>
    [Test]
    public void vowelConfusedWordHasCorrectProperty()
    {
        AddWordToDDaWordSetter("test", true, false, false, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(true, dDAUnderTest.GetWords()[0].properties.Contains(LanguageUnitProperty.vowelConfuse));
    }

    /// <summary>
    /// Ensures a soft d word has the correct property in the DDA after getting added
    /// </summary>
    [Test]
    public void softDWordHasCorrectProperty()
    {
        AddWordToDDaWordSetter("test", false, false, true, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(true, dDAUnderTest.GetWords()[0].properties.Contains(LanguageUnitProperty.softD));
    }

    /// <summary>
    /// Ensures a double consonant word has the correct property in the DDA after getting added
    /// </summary>
    [Test]
    public void doubleConsonantWordHasCorrectProperty()
    {
        AddWordToDDaWordSetter("test", false, true, false, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(true, dDAUnderTest.GetWords()[0].properties.Contains(LanguageUnitProperty.doubleConsonant));
    }

    /// <summary>
    /// Ensures a silent consonant word has the correct property in the DDA after getting added
    /// </summary>
    [Test]
    public void silentConsonantWordHasCorrectProperty()
    {
        AddWordToDDaWordSetter("test", false, false, false, true);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(true, dDAUnderTest.GetWords()[0].properties.Contains(LanguageUnitProperty.silentConsonant));
    }

    /// <summary>
    /// Ensures spaces and chars after them are removed
    /// </summary>
    [Test]
    public void SpacesGetRemoved()
    {
        AddWordToDDaWordSetter("test 1", false, false, false, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual("test", dDAUnderTest.GetWords()[0].identifier);
    }

    /// <summary>
    /// Ensures that duplicate words are only added once to the DDA
    /// </summary>
    [Test]
    public void DuplicatesGetsRemoved()
    {
        AddWordToDDaWordSetter("test 1", false, false, false, false);
        AddWordToDDaWordSetter("test 2", false, false, false, false);
        dDAWordSetterUnderTest.LoadWords(dDAUnderTest);
        Assert.AreEqual(1, dDAUnderTest.GetWords().Count);
    }

    /// <summary>
    /// Adds a word to the DDAWordSetter
    /// </summary>
    /// <param name="word">the word to be added</param>
    /// <param name="vowelConfusedWord">if it is a vowel confused word</param>
    /// <param name="doubleConsonant">if it is a double consonant word</param>
    /// <param name="softDWord">if it is a soft d word</param>
    /// <param name="silentConsonantWord">if it is a silent consonant word</param>
    /// <returns>The texture2D used by the DDAWordSetter</returns>
    private Texture2D AddWordToDDaWordSetter(string word, bool vowelConfusedWord, bool doubleConsonant, bool softDWord, bool silentConsonantWord)
    {
        Texture2D image = new Texture2D(0, 0);
        image.name = word;
        dDAWordSetterUnderTest.AddWord(image, vowelConfusedWord, doubleConsonant, softDWord, silentConsonantWord);
        return image;
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