using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._54_SymbolEater.Scripts.Gamemodes;
using UnityEngine;
using System.Collections.Generic;
using CORE;
using Analytics;
using Letters;

namespace Scenes._50_Minigames.Gamemode
{
    public class SymbolEaterSetter: IGameModeSetter
    {
        public List<IGenericGameMode> gamemodes = new List<IGenericGameMode>()
        {
            new FindSymbols(),
            null,
            new SymbolEaterLevel3(),
            new Level4_SymbolEater(),
            new Level5_SymbolEater()
        };


        private List<IGameRules> gamerules = new List<IGameRules>()
        {
            new DynamicGameRules(),
            null,
            new DynamicGameRules(),
            new DynamicGameRules(),
            new DynamicGameRules()
        };

        private List<IGenericGameMode> letterGamemodes = new List<IGenericGameMode>
        {
            new Level4_SymbolEater(),
            new RecognizeNameOfLetter(),
            new RecognizeSoundOfLetter()
        };

        private List<IGenericGameMode> letterCategoryGamemodes = new List<IGenericGameMode>
        {
            new FindSymbols()
        };


        private List<IGenericGameMode> wordGamemodes = new List<IGenericGameMode>
        {
            new SpellWordFromImage(),
            new FindFirstLetterFromImage(),
            new SymbolEaterLevel3()
        };

        /// <summary>
        /// Determines which gamemodes to use
        /// </summary>
        /// <param name="level">the level of the player</param>
        /// <returns>a set of a gamemode and gamerules</returns>
        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            //GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ø", 60);
            //GameManager.Instance.PerformanceWeightManager.SetEntityWeight("X", 60);
            //GameManager.Instance.PerformanceWeightManager.SetEntityWeight("ko", 60);
            DynamicGameRules dynamicGameRules = new DynamicGameRules();
            List<ILanguageUnit> languageUnit = GameManager.Instance.DynamicDifficultyAdjustmentManager
                    .GetNextLanguageUnitsBasedOnLevel(80);
            IGenericGameMode mode;
            switch(languageUnit[0].LanguageUnitType)
            {
                case LanguageUnit.Letter:
                    mode = letterGamemodes[Random.Range(0, letterGamemodes.Count)];
                    LetterData letterData = (LetterData)languageUnit[0];
                    if(letterData.Category == LetterCategory.Vowel || letterData.Category == LetterCategory.Consonant)
                    {
                        mode = letterCategoryGamemodes[Random.Range(0, letterCategoryGamemodes.Count)];
                    }
                    List<ILanguageUnit> filteredLetters = new List<ILanguageUnit>();
                    foreach(ILanguageUnit unit in languageUnit)
                    {
                        if(unit.LanguageUnitType == LanguageUnit.Letter)
                        {
                            if(unit.Identifier.Length > 1)
                            {
                                Debug.LogError("word got sorted into wrong category");
                            }
                            filteredLetters.Add(unit);
                        }
                    }
                    dynamicGameRules.AddFilteredList(filteredLetters);
                    break;
                case LanguageUnit.Word:
                    mode = wordGamemodes[Random.Range(0, wordGamemodes.Count)];
                    List<ILanguageUnit>filteredWords = new List<ILanguageUnit>();
                    for(int i = 0; i < languageUnit.Count; i++)
                    {
                        if(languageUnit[i].LanguageUnitType == LanguageUnit.Word && WordsForImagesManager.imageWords.Contains(languageUnit[i].Identifier))
                        {
                            filteredWords.Add(languageUnit[i]);
                        }
                    }
                    if(filteredWords.Count > 0)
                    {
                        dynamicGameRules.AddFilteredList(filteredWords);
                    }
                    else
                    {
                        (IGenericGameMode, DynamicGameRules) modeSet = NoValidWordsMode(languageUnit);
                        dynamicGameRules = modeSet.Item2;
                        mode = modeSet.Item1;
                    }
                    break;
                case LanguageUnit.Sentence:
                default:
                    Debug.LogError("the type of language unit has not been implemented");
                    mode = new FindSymbols();
                    break;
            }
            return (dynamicGameRules, mode);
        }

        private (IGenericGameMode, DynamicGameRules) NoValidWordsMode(List<ILanguageUnit> languageUnits)
        {
            IGenericGameMode mode = null;
            DynamicGameRules dynamicGameRules = new DynamicGameRules();
            List<ILanguageUnit> filteredLetters = new List<ILanguageUnit>();
            bool pickedGamemode = false;
            
            LetterData letter = new LetterData("ds", LetterCategory.Consonant, 1);
            for(int i = 1; i < languageUnits.Count; i++)
            {
                if(languageUnits[i].LanguageUnitType == LanguageUnit.Letter)
                {
                    LetterData letterData = (LetterData)languageUnits[i];
                    if(!pickedGamemode)
                    {
                        mode = letterGamemodes[Random.Range(0, letterGamemodes.Count)];
                        
                        if(letterData.Category == LetterCategory.Vowel || letterData.Category == LetterCategory.Consonant)
                        {
                            mode = letterCategoryGamemodes[Random.Range(0, letterCategoryGamemodes.Count)];
                        }
                        pickedGamemode = true;
                        letter = letterData;
                    }
                    if(letter.Identifier.Length == 1 && letterData.Category == letter.Category)
                    {
                        filteredLetters.Add(letter);
                    }
                }
            }
            if(filteredLetters.Count > 0)
            {
                dynamicGameRules.AddFilteredList(filteredLetters);
                return (mode, dynamicGameRules);
            }
            else
            {
                return (null, null);
            }
        }

        /// <summary>
        /// returns a gamemode of the Symbol Eater type
        /// </summary>
        /// <param name="level">The level to be used as index</param>
        /// <returns>the gamemode of the level given or null if it is outside the indexes of the list</returns>
        public IGenericGameMode SetMode(int level)
        {
            if(gamemodes.Count > level && level >= 0)
            {
                return gamemodes[level];
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a gamemode based on a given string
        /// </summary>
        /// <param name="gamemode">the string representation of the given gamemode</param>
        /// <returns>the desired gamemode or the default one if the desired gamemode could not be found</returns>
        public IGenericGameMode SetMode(string gamemode)
        {
            ISEGameMode modeReturned;
            switch (gamemode)
            {
                case "spellword":
                    modeReturned = new SpellWordFromImage();
                    break;
                case "imagetosound":
                    modeReturned = new FindImageFromSound();
                    break;
                case "recognizesoundofletter":
                    modeReturned = new RecognizeSoundOfLetter();
                    break;
                case "recognizenameofletter":
                    modeReturned = new RecognizeNameOfLetter();
                    break;
                case "findnumber":
                    modeReturned = new FindNumber();
                    break;
                case "findsymbol":
                    modeReturned = new FindSymbol();
                    break;
                case "findsymbols":
                    modeReturned = new FindSymbols();
                    break;
                case "findfirstletterfromimage":
                    modeReturned = new FindFirstLetterFromImage();
                    break;
                case "spellincorrectword":
                    modeReturned = new SpellIncorrectWord();
                    break;
                case "SymbolEaterLevel3":
                    modeReturned = new SymbolEaterLevel3();
                    break;
                case "Level4_SymbolEater":
                    modeReturned = new Level4_SymbolEater();
                    break;
                case "Level5_SymbolEater":
                    modeReturned = new Level5_SymbolEater();
                    break;
                default:
                    Debug.Log("given mode was not among expected options, returning null");
                    modeReturned = null;
                    break;
            }
            return modeReturned;
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="level">The level to use as index for the desired gamerules</param>
        /// <returns></returns>
        public IGameRules SetRules(int level)
        {
            if(gamerules.Count > level && level >= 0)
            {
                return gamerules[level];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// returns a gamerule set
        /// </summary>
        /// <param name="rules">The rules we are looking for</param>
        /// <returns>the desired gamerules. Otherwise returns the default set</returns>
        public IGameRules SetRules(string gamerules)
        {
            IGameRules rulesReturned;
            switch (gamerules)
            {
                case "spellword":
                    rulesReturned = new SpellWord();
                    break;
                case "findnumberseries":
                    rulesReturned = new FindNumberSeries();
                    break;
                case "findcorrectletter":
                    rulesReturned = new FindCorrectLetter();
                    break;
                case "findlettertype":
                    rulesReturned = new FindLetterType();
                    break;
                case "findnextletter":
                    rulesReturned = new FindNextLetter();
                    break;
                case "findfirstletter":
                    rulesReturned = new FindFirstLetter();
                    break;
                case "findincorrectwords":
                    rulesReturned = new FindIncorrectWords();
                    break;
                case "findvowels":
                    rulesReturned = new FindVowel();
                    break;
                case "findconsonants":
                    rulesReturned = new FindConsonant();
                    break;
                case "GetVowelFromPic":
                    rulesReturned = new FindLetterInPicture();
                    break;
                case "Level4_SymbolEater":
                    rulesReturned = new FindFMNSConsonantBySound();
                    break;
                case "Level5_SymbolEater":
                    rulesReturned = new FindFMNSConsonantBySound();
                    break;
                default:
                    Debug.Log("given ruleset was not among expected options, returning null");
                    rulesReturned = null;
                    break;
            }
           return rulesReturned;
        }
    }
}
