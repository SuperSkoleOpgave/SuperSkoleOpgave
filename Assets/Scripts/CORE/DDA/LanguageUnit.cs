using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.Toolbars;
using UnityEngine;


[CreateAssetMenu(fileName = "LanguageUnit", menuName = "LanguageUnit/LanguageUnit")]
public class LanguageUnit : ScriptableObject
{
    public string identifier;
    public List<property> properties;
    public float weight;

    public DynamicDifficultyAdjustment dynamicDifficultyAdjustment;

    /// <summary>
    /// sets the weight of the languageunit to the total weight of all properties
    /// </summary>
    public void CalculateWeight()
    {
        weight = 0;
        foreach(property prop in properties)
        {

            weight += dynamicDifficultyAdjustment.GetPropertyWeight(prop);
        }
    }
}
