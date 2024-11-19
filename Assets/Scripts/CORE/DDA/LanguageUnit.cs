using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.Toolbars;
using UnityEngine;


[CreateAssetMenu(fileName = "LanguageUnit", menuName = "LanguageUnit/LanguageUnit")]
public class LanguageUnit : ScriptableObject
{
    public string identifier;
    public List<LanguageUnitProperty> properties;
    public float weight;

    public DynamicDifficultyAdjustment dynamicDifficultyAdjustment;

    /// <summary>
    /// sets the weight of the languageunit to the total weight of all properties
    /// </summary>
    public void CalculateWeight()
    {
        weight = 0;
        foreach(LanguageUnitProperty prop in properties)
        {
            if(dynamicDifficultyAdjustment == null)
            {
                throw new Exception("DDA has not been assigned");
            }
            weight += dynamicDifficultyAdjustment.GetPropertyWeight(prop);
        }
    }
}
