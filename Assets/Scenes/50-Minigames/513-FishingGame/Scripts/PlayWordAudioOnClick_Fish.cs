using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayWordAudioOnClick_Fish : MonoBehaviour
{
    // Start is called before the first frame update

    private string wordToPlay;
    private AudioClip wordAudio;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Plays the wordaudio that corresponds to the image pressed. 
    /// </summary>
    private void OnMouseUpAsButton()
    {
        if (wordToPlay == null)
        {
            wordToPlay = transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture.name;
            string wordToUse = wordToPlay.Split(" ")[0];

            wordToUse = wordToUse.Replace("(aa)", "\u00e5");
            wordToUse = wordToUse.Replace("(ae)", "\u00e6");
            wordToUse = wordToUse.Replace("(oe)", "\u00F8");

            wordAudio = WordAudioManager.GetAudioClipFromWord(wordToUse);
        }

        
        AudioManager.Instance.PlaySound(wordAudio, SoundType.Voice, false);
    }
}
