using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DDAWordSetter : MonoBehaviour
{
    [SerializeField]
    List<LanguageUnit> letters;

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


    public void LoadWords(DynamicDifficultyAdjustment dynamicDifficultyAdjustment)
    {
        dynamicDifficultyAdjustment.SetupLanguageUnits(letters, ApplyLanguageUnits());
    }

    /// <summary>
    /// goes through all words added to the words list and turns them into LanguageUnits
    /// </summary>
    /// <returns></returns>
    private List<LanguageUnit> ApplyLanguageUnits()
    {
        List<LanguageUnit> returnedUnits = new List<LanguageUnit>();
        foreach(Texture2D word in words)
        {
            LanguageUnit wordUnit = new LanguageUnit();
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

    #region TestMethods
    public void AddWord(Texture2D word, bool vowelConfusedWord, bool doubleConsonantWord, bool softDWord, bool silentConsonantWord)
    {
        if(words == null)
        {
            words = new List<Texture2D>();
        }
        words.Add(word);
        if(vowelConfusedWords == null)
        {
            vowelConfusedWords = new List<Texture2D>();
        }
        if(vowelConfusedWord)
        {
            vowelConfusedWords.Add(word);
        }
        if(doubleConsonantWords == null)
        {
            doubleConsonantWords = new List<Texture2D>();
        }
        if(doubleConsonantWord)
        {
            doubleConsonantWords.Add(word);
        }
        if(softDWords == null)
        {
            softDWords = new List<Texture2D>();
        }
        if(softDWord)
        {
            softDWords.Add(word);
        }
        if(silentConsonantWords == null)
        {
            silentConsonantWords = new List<Texture2D>();
        }
        if(silentConsonantWord)
        {
            silentConsonantWords.Add(word);
        }

    }

    public bool WordsContainsWord(Texture2D word)
    {
        return words.Contains(word);
    }
    public bool VowelConfusedWordsContainsWord(Texture2D word)
    {
        return vowelConfusedWords.Contains(word);
    }
    
    public bool DoubleConsonantWordsContainsWord(Texture2D word)
    {
        return doubleConsonantWords.Contains(word);
    }

    public bool SoftDWordsContainsWord(Texture2D word)
    {
        return softDWords.Contains(word);
    }

    public bool SilentConsonantWordsContainsWord(Texture2D word)
    {
        return silentConsonantWords.Contains(word);
    }
    #endregion
}
