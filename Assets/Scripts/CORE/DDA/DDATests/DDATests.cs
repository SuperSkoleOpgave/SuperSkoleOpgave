using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DDATests
{
    
    #region TestMethods
    [Test]
    public void CanAddWord()
    {
        DynamicDifficultyAdjustment dDAUnderTest = new DynamicDifficultyAdjustment();
        LanguageUnit languageUnitUnderTest = new LanguageUnit();
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.word;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties.Add(propertyUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);


    }
    #endregion

    #region AdjustWeight
    [Test]
    public void CanAdjustWeightofWordUpwards()
    {
        DynamicDifficultyAdjustment dDAUnderTest = new DynamicDifficultyAdjustment();
        LanguageUnit languageUnitUnderTest = new LanguageUnit();
        Property propertyUnderTest = new Property();
        propertyUnderTest.property = property.testProperty;
        propertyUnderTest.weight = 50;
        propertyUnderTest.levelLock = 0;
        languageUnitUnderTest.identifier = "dd";
        languageUnitUnderTest.properties.Add(propertyUnderTest);
        dDAUnderTest.AddWord(languageUnitUnderTest);

        dDAUnderTest.AdjustWeight(languageUnitUnderTest, true);

        Assert.AreEqual(propertyUnderTest, 50 + 1);
    }
    #endregion
    // A Test behaves as an ordinary method
    [Test]
    public void DDATestsSimplePasses()
    {
        Assert.AreEqual(true, true);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator DDATestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
