using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DDAWordSetter : MonoBehaviour
{
    [SerializeField]
    List<Texture2D> words;
    [SerializeField]
    List<Texture2D> vowelConfusedWords;
    [SerializeField]
    List<Texture2D> doubleConsonantWords;
    [SerializeField]
    List<Texture2D> softDWords;
    [SerializeField]
    List<Texture2D> silentConsonantWords;

    void Start()
    {
        List<LanguageUnit> words = applyLanguageUnits();
    }
    /// <summary>
    /// goes through all words added to the words list and turns them into LanguageUnits
    /// </summary>
    /// <returns></returns>
    private List<LanguageUnit> applyLanguageUnits()
    {
        List<LanguageUnit> returnedUnits = new List<LanguageUnit>();
        foreach(Texture2D word in words)
        {
            LanguageUnit wordUnit = ScriptableObject.CreateInstance<LanguageUnit>();
            wordUnit.identifier = word.name.ToLower();
            List<LanguageUnitProperty> props = new List<LanguageUnitProperty>();
            if (vowelConfusedWords.Contains(word)) props.Add(LanguageUnitProperty.vowelConfuse);
            if (doubleConsonantWords.Contains(word)) props.Add(LanguageUnitProperty.doubleConsonant);
            if (softDWords.Contains(word)) props.Add(LanguageUnitProperty.softD);
            if (silentConsonantWords.Contains(word)) props.Add(LanguageUnitProperty.silentConsonant);
            wordUnit.properties = props;
            returnedUnits.Add(wordUnit);
        }
        return returnedUnits;
    }
}
