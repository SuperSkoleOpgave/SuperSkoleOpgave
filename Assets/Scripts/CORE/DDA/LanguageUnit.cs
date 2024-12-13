using System;
using System.Collections;
using System.Collections.Generic;
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
        if(properties.Count > 0)
        {
            int amount = 0;
            foreach(LanguageUnitProperty prop in properties)
            {
                if(dynamicDifficultyAdjustment == null)
                {
                    throw new Exception("DDA has not been assigned");
                }
                float deltaWeight = dynamicDifficultyAdjustment.GetPropertyWeight(prop);
                if(deltaWeight > 0)
                {
                    weight += deltaWeight;
                    amount++;
                }
            }
            if(amount > 0)
            {
                weight /= amount;
            }
        }
        
    }
}
