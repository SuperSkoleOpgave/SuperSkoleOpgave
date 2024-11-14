
using System.Collections.Generic;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Linq;
using UnityEngine;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using CORE;
using Letters;

namespace Scenes.Minigames.LetterGarden.Scripts.Gamemodes {
    /// <summary>
    /// A LettergardenGamemode implementation where the player should draw random numbers
    /// </summary>
    public class DrawWithoutBee : LettergardenGameMode
    {

        List<LanguageUnit> languageUnits = new List<LanguageUnit>();
        public List<LanguageUnit> GetCurrentLanguageUnits()
        {
            return languageUnits;
        }

        /// <summary>
        /// creates a list of SplineSymbolDataHolders of a given length
        /// </summary>
        /// <param name="amount">How many elements should be in the list</param>
        /// <returns>A list of SplineSymbolDataHolders</returns>
        public List<SplineSymbolDataHolder> GetSymbols(int amount, IGameRules gameRules)
        {
            List<SplineSymbolDataHolder> result = new List<SplineSymbolDataHolder>();
            List<string> usedLetters = new List<string>();
            bool shouldRegenerateAnswer = true;
            if(gameRules.GetType() == typeof(DynamicGameRules))
            {
                shouldRegenerateAnswer = true;
            }
            gameRules.SetCorrectAnswer();
            //Adds a given amount of random letters to the result list based on the given game rules.
            for (int i = 0; i < amount; i++)
            {
                if(shouldRegenerateAnswer)
                {
                    gameRules.SetCorrectAnswer();
                }
                string letter = gameRules.GetCorrectAnswer();
                
                while(usedLetters.Contains(letter))
                {
                    if(shouldRegenerateAnswer)
                    {
                        gameRules.SetCorrectAnswer();
                    }
                    letter = gameRules.GetCorrectAnswer();
                }
                usedLetters.Add(letter);
                if(gameRules.GetType() == typeof(DynamicGameRules))
                {
                    DynamicGameRules dynamicGameRules = (DynamicGameRules)gameRules;
                    foreach(LanguageUnit languageUnit in dynamicGameRules.GetLanguageUnits())
                    {
                        if(languageUnit.identifier == letter)
                        {
                            languageUnits.Add(languageUnit);
                            break;
                        }
                    }
                }
                if(Random.Range(0, 2) == 0)
                {
                    result.Add(new SplineSymbolDataHolder(SymbolManager.capitalLettersObjects[letter.ToUpper()[0]], SymbolManager.capitalLetters[letter.ToUpper()[0]], letter.ToUpper()[0]));
                }
                else
                {
                    result.Add(new SplineSymbolDataHolder(SymbolManager.lowercaseLettersObjects[letter.ToLower()[0]], SymbolManager.lowercaseLetters[letter.ToLower()[0]], letter.ToLower()[0]));
                }
            }
            return result;
        }

        public void SetUpGameModeDescription(ActiveLetterHandler activeLetterHandler)
        {
            activeLetterHandler.descriptionText.text = "Tegn bogstaver. Bien viser start/slut \n Tryk [Mellemrum] for at høre bogstavet";
        }

        public bool UseBee()
        {
            return false;
        }

    }
}