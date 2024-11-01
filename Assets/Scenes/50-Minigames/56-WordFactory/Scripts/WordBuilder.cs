using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts
{
    public class WordBuilder : MonoBehaviour
    {
        /// <summary>
        /// Builds a word from the letters on the given teeth.
        /// </summary>
        /// <param name="teeth">List of teeth</param>
        /// <returns>Constructed word</returns>
        public string BuildWord(List<Transform> teeth)
        {
            List<char> letters = new List<char>();

            foreach (Transform tooth in teeth)
            {
                TextMeshProUGUI text = tooth.GetComponentInChildren<TextMeshProUGUI>();
                
                if (text != null)
                {
                    letters.Add(text.text[0]);
                }
            }

            return new string(letters.ToArray());
        }
    }
}