using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DDAWordSetter : MonoBehaviour
{
    [SerializeField]
    List<LanguageUnit> letters;

    [SerializeField]
    List<Texture2D> words,
    vowelConfusedWords,
    eOrErEndWords,
    rEndWords,
    softDWords,
    consonantConfuseWords,
    silentConsonantWords,
    doubleConsonantWords,
    gEndWords,
    ngEndWords;

    /// <summary>
    /// Loads the letters list and the generated words list into the given DDA
    /// </summary>
    /// <param name="dynamicDifficultyAdjustment">The DDA to load the languageUnits into</param>
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
        List<string> addedWords = new List<string>();
        foreach(Texture2D word in words)
        {
            LanguageUnit wordUnit = ScriptableObject.CreateInstance<LanguageUnit>();
            wordUnit.identifier = word.name.ToLower();
            if(wordUnit.identifier.Contains(' '))
            {
                wordUnit.identifier = wordUnit.identifier.Split(' ')[0];
            }
            List<LanguageUnitProperty> props = new List<LanguageUnitProperty>();
            if (vowelConfusedWords.Contains(word)) props.Add(LanguageUnitProperty.vowelConfuse);
            if (eOrErEndWords.Contains(word)) props.Add(LanguageUnitProperty.eOrErEnd);
            if (rEndWords.Contains(word)) props.Add(LanguageUnitProperty.rEnd);
            if (softDWords.Contains(word)) props.Add(LanguageUnitProperty.softD);
            if (consonantConfuseWords.Contains(word)) props.Add(LanguageUnitProperty.consonantConfuse);
            if (silentConsonantWords.Contains(word)) props.Add(LanguageUnitProperty.silentConsonant);
            if (doubleConsonantWords.Contains(word)) props.Add(LanguageUnitProperty.doubleConsonant);
            if (gEndWords.Contains(word)) props.Add(LanguageUnitProperty.gEnd);
            if (ngEndWords.Contains(word)) props.Add(LanguageUnitProperty.ngEnd);
            
            wordUnit.properties = props;
            if(!addedWords.Contains(wordUnit.identifier))
            {
                returnedUnits.Add(wordUnit);
                addedWords.Add(wordUnit.identifier);
            }
        }
        return returnedUnits;
    }


    #region TestMethods
    /// <summary>
    /// Adds a word to the various lists
    /// </summary>
    /// <param name="word">the texture2D of the word with the word itself as its name</param>
    /// <param name="vowelConfusedWord">if it is a vowel confused word</param>
    /// <param name="doubleConsonant">if it is a double consonant word</param>
    /// <param name="softDWord">if it is a soft d word</param>
    /// <param name="silentConsonantWord">if it is a silent consonant word</param>
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

    /// <summary>
    /// Adds a letter
    /// </summary>
    /// <param name="letter">The letter to be added</param>
    public void AddLetter(LanguageUnit letter)
    {
        if(letters == null)
        {
            letters = new List<LanguageUnit>();
        }
        letters.Add(letter);
    }
    
    /// <summary>
    /// Whether the words list contains the texture2D
    /// </summary>
    /// <param name="word">The Texture2D to be checked</param>
    /// <returns>Whether the Texture2D is on the list</returns>
    public bool WordsContainsWord(Texture2D word)
    {
        return words.Contains(word);
    }

    /// <summary>
    /// Whether the vowelConfusedWords list contains the texture2D
    /// </summary>
    /// <param name="word">The Texture2D to be checked</param>
    /// <returns>Whether the Texture2D is on the list</returns>
    public bool VowelConfusedWordsContainsWord(Texture2D word)
    {
        return vowelConfusedWords.Contains(word);
    }
    
    /// <summary>
    /// Whether the doubleConsonantWords list contains the texture2D
    /// </summary>
    /// <param name="word">The Texture2D to be checked</param>
    /// <returns>Whether the Texture2D is on the list</returns>
    public bool DoubleConsonantWordsContainsWord(Texture2D word)
    {
        return doubleConsonantWords.Contains(word);
    }

    /// <summary>
    /// Whether the softDWords list contains the texture2D
    /// </summary>
    /// <param name="word">The Texture2D to be checked</param>
    /// <returns>Whether the Texture2D is on the list</returns>
    public bool SoftDWordsContainsWord(Texture2D word)
    {
        return softDWords.Contains(word);
    }

    /// <summary>
    /// Whether the silentConsonantWords list contains the texture2D
    /// </summary>
    /// <param name="word">The Texture2D to be checked</param>
    /// <returns>Whether the Texture2D is on the list</returns>
    public bool SilentConsonantWordsContainsWord(Texture2D word)
    {
        return silentConsonantWords.Contains(word);
    }
    #endregion
}
