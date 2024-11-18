
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analytics;
using Letters;
using Scenes._10_PlayerScene.Scripts;
using Unity;
using UnityEngine;
using Words;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace CORE.Scripts.Game_Rules 
{
    public class DynamicGameRules : IGameRules
    {
        public static bool usesImages = true;
        string correctAnswer;
        string word = "";
        int index;
        int remainingLetterIndex = 1;
        private List<char> wrongAnswerList;
        public languageUnitProperty usedProperty;
        private List<LanguageUnit> languageUnits = new List<LanguageUnit>();
        private List<LanguageUnit> languageUnitsList = new List<LanguageUnit>();

        private List<languageUnitProperty> priorities;

        private bool usesSequence = false;

        
        /// <summary>
        /// Retrieves a correct answer
        /// </summary>
        /// <returns>a correct answer</returns>
        public string GetCorrectAnswer()
        {
            
            //gets a random letter from the languageunits list if it contains more than one element
            if(languageUnits.Count > 1 && (usedProperty == languageUnitProperty.letter || usedProperty == languageUnitProperty.vowel || usedProperty == languageUnitProperty.consonant))
            {
                return languageUnits[Random.Range(0, languageUnits.Count)].identifier;
            }
            return  correctAnswer;
        }

        /// <summary>
        /// Returns a string used in information display in minigames
        /// </summary>
        /// <returns>an information string</returns>
        public string GetDisplayAnswer()
        {
            string displayString = "";
            
            //Sets the display string to the correctanswer if it is a letter and there are no extra letters is in languageUnits. In that case it returns the type of letter 
            if(usedProperty == languageUnitProperty.letter || usedProperty == languageUnitProperty.vowel || usedProperty == languageUnitProperty.consonant)
            {

                displayString = "Error";
                
                if(usedProperty == languageUnitProperty.vowel)
                {
                    displayString = "vokaler";
                }
                else if(usedProperty == languageUnitProperty.consonant)
                {
                    displayString = "konsonanter";
                }
            }
            //for now returns the word to ensure compatability with existing gamemodes but should be removed once the GetSecondaryAnswer() is properly implemented
            else if(usedProperty == languageUnitProperty.word)
            {
                return word;
            }
            
            return displayString;
        }

        /// <summary>
        /// Returns a wrong answer
        /// </summary>
        /// <returns>a wrong answer of the specified type</returns>
        public string GetWrongAnswer()
        {
            //Returns a letter from the wronganswerlist if a word is currently not used
            if(!usesSequence)
            {
                string answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)].ToString();
                while(GetSecondaryAnswer().Length > 0 && GetSecondaryAnswer().Contains(answer[0]))
                {
                    answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)].ToString();
                }
                return answer;
            }
            //Returns a letter from the current word tat is not the one the player is looking for
            else if(remainingLetterIndex < word.Length)
            {
                char letter = word[remainingLetterIndex];
                remainingLetterIndex++;
                return letter.ToString();
            }
            //If all letters from the list has been added a random letter is added which is not the correctanswer
            else 
            {
                char answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)];
                while(answer.ToString() == correctAnswer)
                {
                    answer = wrongAnswerList[Random.Range(0, wrongAnswerList.Count)];
                }
                return answer.ToString();
            }
        }

        /// <summary>
        /// Checks if a symbol is the correct one
        /// </summary>
        /// <param name="symbol">The symbol to checked</param>
        /// <returns>Whether it is the correct symbol</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            //if a sequence is not used it returns whether the lowered versions of the symbol and the correctanswer is equal to each other
            if(!usesSequence && (symbol.ToLower() == correctAnswer.ToLower() || LanguageUnitsContainsIdentifier(symbol)))
            {
                return true;
            }
            //if a sequence is used it returns true after moving on to the next letter in the word
            else if(usesSequence && symbol.ToLower() == correctAnswer.ToLower())
            {
                index++;
                if(index < word.Length)
                {
                    correctAnswer = word[index].ToString();
                }
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Returns whether the sequence is complete
        /// </summary>
        /// <returns>whether the sequence is complete</returns>
        public bool SequenceComplete()
        {
            return index >= word.Length;
        }

        /// <summary>
        /// Sets the correct answer based on input from the DDA
        /// </summary>
        public void SetCorrectAnswer()
        {
            index = 0;
            remainingLetterIndex = 1;
            
            if(languageUnitsList.Count == 0)
            {
                List<languageUnitProperty> properties = new List<languageUnitProperty>();
                languageUnitProperty filterProperty = usedProperty;
                properties.Add(filterProperty);
                languageUnitProperty errorProperty;
                switch(usedProperty)
                {
                    case languageUnitProperty.vowel:
                        errorProperty = languageUnitProperty.consonant;
                        languageUnitsList = GameManager.Instance.dynamicDifficultyAdjustment.GetLetters(properties, 5);
                        correctAnswer = languageUnitsList[0].identifier;
                        break;
                    case languageUnitProperty.consonant:
                        errorProperty = languageUnitProperty.vowel;
                        languageUnitsList = GameManager.Instance.dynamicDifficultyAdjustment.GetLetters(properties, 5);
                        correctAnswer = languageUnitsList[0].identifier;
                        break;
                    case languageUnitProperty.letter:
                        errorProperty = languageUnitProperty.letter;
                        languageUnitsList = GameManager.Instance.dynamicDifficultyAdjustment.GetLetters(new List<languageUnitProperty>(), 5);
                        correctAnswer = languageUnitsList[0].identifier;
                        break;
                    case languageUnitProperty.word:
                        errorProperty = languageUnitProperty.letter;
                        languageUnitsList = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(properties, 5);
                        word = languageUnitsList[0].identifier;
                        break;
                    default:
                        throw new Exception("the property " + usedProperty + " is not implemented");
                }
                
                DetermineWrongLetterCategory(errorProperty);
            }
            if(languageUnits == null)
            {
                languageUnits = new List<LanguageUnit>();
            }
            else {
                languageUnits.Clear();

            }
            languageUnits.Add(languageUnitsList[Random.Range(0, languageUnitsList.Count)]);
            correctAnswer = languageUnits[0].identifier;
        }

        /// <summary>
        /// Determines which lettercategory to use for wronganswers
        /// </summary>
        /// <param name="letterCategory">the lettercategory to use for wrong letters</param>
        private void DetermineWrongLetterCategory(languageUnitProperty letterCategory)
        {
            switch(letterCategory)
            {
                //for consonants and vowels if the player is low enough level it also sets up so correct answer looks for a random correct letter
                case languageUnitProperty.consonant:
                    wrongAnswerList = LetterRepository.GetVowels().ToList();
                    break;
                case languageUnitProperty.vowel:
                    wrongAnswerList = LetterRepository.GetConsonants().ToList();
                    break;
                case languageUnitProperty.letter:
                    wrongAnswerList = LetterRepository.GetAllLetters().ToList();
                    wrongAnswerList.Remove(correctAnswer[0]);
                    break;
                default:
                    Debug.LogError("unknown letter category");
                    break;
            }
        }

        public string GetSecondaryAnswer()
        {

            if(languageUnits.Count > 1)
            {
                string res = "";
                foreach(LanguageUnit languageUnit in languageUnits)
                {
                    res += languageUnit.identifier;
                }
                return res;
            }
            else if(word.Length == 0)
            {
                string res = "";
                foreach(LanguageUnit languageUnit in languageUnitsList)
                {
                    res += languageUnit.identifier;
                }
                return res;
            }
            return word;
        }


        private bool LanguageUnitsContainsIdentifier(string identifier)
        {
            foreach(LanguageUnit languageUnit in languageUnits)
            {
                if(languageUnit.identifier.ToLower() == identifier.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public void AddFilteredList(List<LanguageUnit> languageUnits)
        {
            languageUnitsList = languageUnits;
        }

        public void UseFirstLetter()
        {
            correctAnswer = word[0].ToString();
        }
        public void UseFirstVowel()
        {
            List<Char> vowels = LetterRepository.GetVowels().ToList();
            for(int i = 0; i < word.Length; i++)
            {
                if(vowels.Contains(word.ToUpper()[i]))
                {
                    correctAnswer = word[i].ToString();
                    break;
                }
            }
        }

        public void SetUsedProperty(languageUnitProperty property)
        {
            usedProperty = property;
        }

        public List<LanguageUnit> GetLanguageUnits()
        {
            return languageUnits;
        }
    }
}