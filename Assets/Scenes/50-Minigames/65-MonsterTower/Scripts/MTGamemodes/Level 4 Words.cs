using Analytics;
using CORE;
using CORE.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class Level4_Words : IMTGameMode
    {
        private string previousRetrievedAnswer;




        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for textOnIngredientHolder</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {


            manager.textOnBrick.text = str.ToUpper();


        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {

            List<LanguageUnit> words = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 20);

            var rndWordWithKey = words[Random.Range(0, words.Count)].identifier;


            while (rndWordWithKey == correctAnswer)
            {
                rndWordWithKey = words[Random.Range(0, words.Count)].identifier;
            }

            manager.textOnBrick.text = rndWordWithKey.ToString();

            manager.imageKey = rndWordWithKey.ToString();


        }

        /// <summary>
        /// Sets the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
        /// </summary>
        /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for textOnIngredientHolder</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void GetDisplayAnswer(string str, TowerManager manager)
        {

            AudioClip clip= LetterAudioManager.GetAudioClipFromLetter(str[0] +"1");

            manager.VoiceClip = clip;

        }

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count)
        {
            

            string[] returnedString = new string[count];


            List<LanguageUnit> words = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 20);
            for (int i = 0; i < count; i++)
            {

                //Code to make sure that the previous answer is not getting repeated imediatly after. 

                returnedString[i] = words[Random.Range(0, words.Count)].identifier;

                while (returnedString[i] == previousRetrievedAnswer)
                {

                    returnedString[i] = words[Random.Range(0,words.Count)].identifier;
                }

                previousRetrievedAnswer = returnedString[i];
            }

            return returnedString;
        }
        /// <summary>
        /// changes the prefab of the TowerManager so we only apply 1 image to the bricks
        /// </summary>
        /// <param name="manager">a reference back to the towermanager</param>
        public void SetAnswerPrefab(TowerManager manager)
        {
            manager.hearLetterButton.SetActive(true);
            manager.answerHolderPrefab = manager.textHolderPrefab;

            manager.textOnBrick = manager.textHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            manager.descriptionText.text = "Tryk p\u00e5 ammunition for at lade. \nTryk p� den gr�nne knap for at h�re et forbogstav og skyd det ord som har forbogstavet";
        }
    }

}