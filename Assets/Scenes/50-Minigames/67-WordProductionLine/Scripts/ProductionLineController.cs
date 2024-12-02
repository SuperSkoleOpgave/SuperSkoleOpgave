using Analytics;
using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class ProductionLineController : MonoBehaviour, IMinigameSetup
    {

        [SerializeField]
        private ProductionLineObjectPool objectPool;

        List<LanguageUnit> langUnit;

        private bool kickedOut = false;

        private string fixedWord;


        private void Start()
        {
            langUnit = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 16);


            
        }


        private void Update()
        {
            if (langUnit.Count == 0 && !kickedOut)
            {
                kickedOut = true;
                SwitchScenes.SwitchToMainWorld();
            }
        }

        /// <summary>
        /// gets one letter and checks if its correct or not
        /// </summary>
        /// <returns> random letter.</returns>
        public string GetLetters()
        {
            string randomLetter = LetterManager.GetRandomLetter().ToString();

            return randomLetter;
        }

        /// <summary>
        /// gets a random word for image.
        /// </summary>
        /// <returns> random word.</returns>
        public string GetImages()
        {
            if (Random.Range(0, 2) == 1)
            {
                string randomWord = langUnit[Random.Range(0, langUnit.Count)].identifier;

                randomWord = WordsForImagesManager.GetRandomWordForImage();
                return randomWord;
            }
            else
            {
                string randomWord = langUnit[Random.Range(0, langUnit.Count)].identifier;

                if (randomWord.Length <= 1)
                {
                    randomWord = WordsForImagesManager.GetRandomWordForImage();
                }
                return randomWord;
            }
        }

        /// <summary>
        /// Fixes the answer.
        /// </summary>
        /// <returns> a letter from a randomword.</returns>
        public string GetFixedCorrect()
        {
            string randomWord = GetImages();

            char randomCharLetter = randomWord.ToString()[0];

            string letter = $"{randomCharLetter}";

            return letter;


        }

        public void SetupGame(IGenericGameMode gameMode, IGameRules gameRules)
        {

        }
    }

}