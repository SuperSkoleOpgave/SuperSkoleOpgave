using System.Collections.Generic;
using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Letters;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes._50_Minigames.Gamemode
{
    public class PathOfDangerSetter: IGameModeSetter
    {
        public List<IGenericGameMode> gamemodes = new List<IGenericGameMode>()
        {
            new Level4_POD(),
            new Level5_POD()
        };

        public List<IGenericGameMode> letterGameModes = new List<IGenericGameMode>()
        {
            new Level4_POD(),
            new Level5_POD()
        };

        public List<IGenericGameMode> wordGameModes = new List<IGenericGameMode>()
        {
            new Level4_POD_Words(),
            new Level5_POD_Words()
        };


        private List<IGameRules> gamerules = new List<IGameRules>()
        {
            null,
            null,
            null,
            null,
            null,
        };

        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {
            
            IGenericGameMode mode = null;
            List<LanguageUnitProperty> priorities = GameManager.Instance.dynamicDifficultyAdjustment.GetPlayerPriority();
            for(int i = 0; i < priorities.Count; i++)
            {
                if(priorities[i] == LanguageUnitProperty.letter)
                {
                    mode = letterGameModes[Random.Range(0, letterGameModes.Count)];
                    break;
                }
                if(priorities[i] == LanguageUnitProperty.word)
                {
                    mode = wordGameModes[Random.Range(0, wordGameModes.Count)];
                    break;
                }
            }
            return (null, mode);
        }

        /// <summary>
        /// returns a gamemode of the Path of Danger type
        /// </summary>
        /// <param name="level">The playerlevel used as index on the gamemode list</param>
        /// <returns></returns>
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
            IPODGameMode modeReturned;
            switch (gamemode)
            {
               
                case "level 4":
                    modeReturned = new Level4_POD();
                    break;
                case "level 5":
                    modeReturned = new Level5_POD();
                    break;

                case "level 4 words":
                    modeReturned = new Level4_POD_Words();
                    break;

                case "level 5 words":
                    modeReturned = new Level5_POD_Words();
                    break;
                default:
                    Debug.Log("given mode was not among expected options, returning default gamemode");
                    modeReturned = new Level4_POD();
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
            switch(gamerules)
            {
                default:
                    Debug.Log("given ruleset was not among expected options, returning default gamerules");
                    rulesReturned = new SpellWord();
                    break;
            }
            return rulesReturned;
        }
    }
}