using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class DDATestScenVerification
{
    [SetUp]
    public void Setup()
    {
       EditorSceneManager.OpenScene("Assets/Scripts/CORE/DDA/DDATests/DDATestScene.unity");
    }
    
    [Test]
    public void VerifyScene()
    {
        var gameObject = GameObject.Find("GameObjectToTestFor");
        Assert.That(gameObject, Is.Not.Null);
    }

    [Test]
    public void VerifyComponent()
    {
        var gameObject = GameObject.Find("GameObjectToTestFor");
        Assert.That(gameObject.GetComponent<DynamicDifficultyAdjustment>(), Is.Not.Null);
    }

    [TearDown]
    public void Teardown()
    {
       EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
