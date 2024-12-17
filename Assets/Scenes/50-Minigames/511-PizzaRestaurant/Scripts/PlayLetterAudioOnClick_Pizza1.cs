using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayLetterAudioOnClick_Pizza : MonoBehaviour
{
    // Start is called before the first frame update

    private string letterToPlay;
    private AudioClip letterAudio;

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
        if (letterToPlay == null)
        {
            letterToPlay = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
            string letterToUse = letterToPlay;

            letterToUse = letterToUse.Replace("(aa)", "\u00e5");
            letterToUse = letterToUse.Replace("(ae)", "\u00e6");
            letterToUse = letterToUse.Replace("(oe)", "\u00F8");

            letterAudio = LetterAudioManager.GetAudioClipFromLetter(letterToUse.ToLower()+"1");

        }

        
        AudioManager.Instance.PlaySound(letterAudio, SoundType.Voice, false);
    }
}
