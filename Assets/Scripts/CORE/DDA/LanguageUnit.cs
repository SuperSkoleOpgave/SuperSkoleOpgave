using System.Collections;
using System.Collections.Generic;
using UnityEditor.Toolbars;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageUnit", menuName = "LanguageUnit/LanguageUnit")]
public class LanguageUnit : ScriptableObject
{
    public string identifier;
    public List<Property> properties;
    public float weight;

    public void GetWeight()
    {
        weight = 0;
        foreach(Property prop in properties)
        {
            weight += prop.weight;
        }
    }
}
