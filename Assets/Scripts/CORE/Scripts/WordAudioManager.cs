using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CORE.Scripts
{


    public class WordAudioManager
    {
        private static Dictionary<string, List<AudioClip>> wordDictionary = new();
        public static bool IsDataLoaded { get; private set; } = false;

        public static void AddAudioClipToSet(string name, AudioClip input)
        {
            if (wordDictionary.ContainsKey(name.ToLower()))
            {
                wordDictionary[name.ToLower()].Add(input);
            }
            else
            {
                wordDictionary.Add(name.ToLower(), new List<AudioClip>());
                wordDictionary[name.ToLower()].Add(input);
            }
            IsDataLoaded = true;
            
        }

        public static AudioClip GetAudioClipFromWord(string inputWord)
        {
            if (!wordDictionary.TryGetValue(inputWord.ToLower(), out List<AudioClip> data))
                data = null;
            AudioClip audioClip;
            if (data == null)
            {
                Debug.LogError($"Error getting audio for the word: {inputWord}");
            }
            if (data.Count > 1)
                audioClip = data[UnityEngine.Random.Range(0, data.Count)];
            else
                audioClip = data[0];

            return audioClip;
        }
    }
}