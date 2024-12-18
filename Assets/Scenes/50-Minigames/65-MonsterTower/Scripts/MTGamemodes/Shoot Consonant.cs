using Analytics;
using CORE;
using CORE.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public class ShootConsonant : IMTGameMode
    {
        
        List<ILanguageUnit> consonants;
        private string previousRetrievedAnswer;



        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for textOnIngredientHolder</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager)
        {
          

            foreach (var item in consonants)
            {
                if (item.Identifier[0] == str.ToCharArray()[0])
                {
                    manager.textOnBrick.text = item.ToString();
                }
            }
            
        
        }

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer)
        {
            var rndVowelWithKey = LetterManager.GetRandomVowel();

            while (rndVowelWithKey == correctAnswer.ToCharArray()[0])
            {
                rndVowelWithKey = LetterManager.GetRandomVowel();
            }

            manager.textOnBrick.text = rndVowelWithKey.ToString();

            

            manager.imageKey = rndVowelWithKey.ToString();

          
        }

        /// <summary>
        /// Sets the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
        /// </summary>
        /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for textOnIngredientHolder</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void GetDisplayAnswer(string str, TowerManager manager)
        {
            manager.displayBox.text = str;
        }

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count)
        {
            string[] returnedString = new string[count];


            //consonants = GameManager.Instance.DynamicDifficultyAdjustmentManager.GetNextLanguageUnitsBasedOnLevel(15);
            for (int i = 0; i < count; i++)
            {
                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;

                bool checkIfAvailable = true;

                while (checkIfAvailable)
                {
                    switch (returnedString[i].ToLower())
                    {
                        case "y":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        case "z":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        case "w":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        case "c":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        case "q":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        case "x":
                            returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                            break;

                        default:
                            checkIfAvailable = false;
                            break;
                    }
                }

                while (returnedString[i] == previousRetrievedAnswer)
                {
                    returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;

                    checkIfAvailable = true;

                    while (checkIfAvailable)
                    {
                        switch (returnedString[i].ToLower())
                        {
                            case "y":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            case "z":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            case "w":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            case "c":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            case "q":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            case "x":
                                returnedString[i] = consonants[Random.Range(0, consonants.Count)].Identifier;
                                break;

                            default:
                                checkIfAvailable = false;
                                break;
                        }
                    }
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
            manager.answerHolderPrefab = manager.textHolderPrefab;

            manager.textOnBrick = manager.textHolderPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

}