using System.Collections;
using System.Collections.Generic;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace CORE.Scripts.Game_Rules
{


    /// <summary>
    /// Implementation of IGameRules for games where the player should find the first letter of a word
    /// </summary>
    public class FindFirstLetter : IGameRules
    {
        string currentWord = "";
        string currentLetter;

        List<char> wrongLetters = new List<char>();

        int currentIndex;



        /// <summary>
        /// Returns currentletter
        /// </summary>
        /// <returns>the letter which the player is looking for</returns>
        public string GetCorrectAnswer()
        {
            return currentLetter;
        }

        /// <summary>
        /// Returns currentword
        /// </summary>
        /// <returns>returns the word the player is currently attempting to spell</returns>
        public string GetDisplayAnswer()
        {
            return currentWord;
        }

        public string GetSecondaryAnswer()
        {
            return currentWord;
        }


        /// <summary>
        /// Returns a random letter not in the current word
        /// </summary>
        /// <returns>A random letter not in the current word</returns>
        public string GetWrongAnswer()
        {
            char letter = LetterManager.GetRandomLetter();
            //Ensures the letter is not in the current word
            while(letter.ToString() == currentLetter || wrongLetters.Contains(letter))
            {
                letter = LetterManager.GetRandomLetter();
            }
            wrongLetters.Add(letter);
            return letter.ToString();
        }

        /// <summary>
        /// Checks if the symbol is the same as the one the player is looking for.
        /// </summary>
        /// <param name="symbol">The symbol to be checked</param>
        /// <returns>Whether the symbol is the same as the current letter</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return currentLetter.ToLower() == symbol.ToLower();
        }

        /// <summary>
        /// Tells if the player has reached the end of the current word.
        /// </summary>
        /// <returns>Whether current index is the same as the last index of the word</returns>
        public bool SequenceComplete()
        {
            return currentIndex == currentWord.Length - 1;
        }

        /// <summary>
        /// sets a new random word if it is the first time it is called or if the end of the current is reached. otherwise it just updates the current letter.
        /// </summary>
        public void SetCorrectAnswer()
        {
            currentWord = WordsForImagesManager.GetRandomWordForImage();
            currentLetter = currentWord[0].ToString();
            wrongLetters.Clear();

        }

    }
}