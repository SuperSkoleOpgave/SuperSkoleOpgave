using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class DDATestScenVerification
{
    /// <summary>
    /// Loads the test scene
    /// </summary>
    [SetUp]
    public void Setup()
    {
       EditorSceneManager.OpenScene("Assets/Scripts/CORE/DDA/DDATests/DDATestScene.unity");
    }
    
    /// <summary>
    /// Verifies the scene is loaded
    /// </summary>
    [Test]
    public void VerifyScene()
    {
        var gameObject = GameObject.Find("GameObjectToTestFor");
        Assert.That(gameObject, Is.Not.Null);
    }

    /// <summary>
    /// Verify the dda can be found
    /// </summary>
    [Test]
    public void VerifyComponent()
    {
        var gameObject = GameObject.Find("GameObjectToTestFor");
        Assert.That(gameObject.GetComponent<DynamicDifficultyAdjustment>(), Is.Not.Null);
    }

    /// <summary>
    /// Cleans up after the tests are done
    /// </summary>
    [TearDown]
    public void Teardown()
    {
       EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
