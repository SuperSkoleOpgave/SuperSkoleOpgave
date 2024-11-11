using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DynamicDifficultyAdjustment : MonoBehaviour
{
    List<string> words;
    List<string> letters;
    List<string> properties;
    int playerLanguageLevel;

    public void GetLetter(List<string> properties) {

    }

    public void GetLetters(List<string> properties) {
        
    }

    public void GetWord(List<string> properties) {

    }

    public void GetWords(List<string> properties) {
        
    }

    public void AdjustWeight(string languageUnit, bool correct)
    {
        if(words.Contains(languageUnit))
        {
            AdjustWeightWord(languageUnit, correct);
        }
        else if(letters.Contains(languageUnit))
        {
            AdjustWeightLetter(languageUnit, correct);
        }
        else
        {
            Debug.LogError("no list contains the languageunit with identifier: " + languageUnit);
        }
        CalculateLanguageLevel();
    }

    private void AdjustWeightWord(string languageUnit, bool correct)
    {

    }

    private  void AdjustWeightLetter(string languageUnit, bool correct)
    {

    }

    public bool IsLanguageUnitTypeUnlocked(string property)
    {
        return true;
    }

    public List<string> GetPlayerPriorityUnlocked()
    {
        return new List<string>();
    }

    private void Load()
    {

    } 

    private void Save()
    {

    }

    private void SetupLanguageUnits()
    {

    }

    private void CalculateLanguageLevel()
    {

    }
}
