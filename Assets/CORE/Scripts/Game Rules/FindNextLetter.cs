using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts.GameRules
{
    /// <summary>
    /// Implementation of IGameRules for games where the player should look for a specific letter
    /// </summary>
    public class FindNextLetter : IGameRules
    {
        string correctLetter;
        string previousLetter = "";

        List<char> previousLetters = new List<char>();

        /// <summary>
        /// returns the variable correctLetter
        /// </summary>
        /// <returns>the correct letter</returns>
        public string GetCorrectAnswer()
        {
            return correctLetter;
        }

        /// <summary>
        /// Returns the correct letter in uppercase
        /// </summary>
        /// <returns>the correct letter in uppercase</returns>
        public string GetDisplayAnswer()
        {
            return "Find bogstavet efter " + previousLetter;
        }
        
        /// <summary>
        /// Returns a random letter which is not the correct one
        /// </summary>
        /// <returns>A random letter which is not the correct one</returns>
        public string GetWrongAnswer()
        {
            string letter = LetterManager.GetRandomLetter().ToString().ToLower();
            while(letter == GetCorrectAnswer())
            {
                letter = LetterManager.GetRandomLetter().ToString().ToLower();
            }
            return letter;
        }

        /// <summary>
        /// Checks if the lowercase version of the given letter is the same as the correct one
        /// </summary>
        /// <param name="symbol">The symbol to be checked</param>
        /// <returns>Whether it is the correct one</returns>
        public bool IsCorrectSymbol(string symbol)
        {
            return correctLetter == symbol.ToLower();
        }


        /// <summary>
        /// not used
        /// </summary>
        /// <returns>always true</returns>
        public bool SequenceComplete()
        {
            return true;
        }

        /// <summary>
        /// changes correctLetter to a new one
        /// </summary>
        public void SetCorrectAnswer()
        {
            char temp = LetterManager.GetRandomLetter();
            
            List<char> letters = LetterManager.GetAllLetters();
            while(letters.IndexOf(temp) == letters.Count - 1 || previousLetters.Contains(temp)){
                temp = LetterManager.GetRandomLetter();
            }
            previousLetter = temp.ToString();
            previousLetters.Add(temp);
            correctLetter = letters[letters.IndexOf(temp) + 1].ToString().ToLower();
        }
    }
}
