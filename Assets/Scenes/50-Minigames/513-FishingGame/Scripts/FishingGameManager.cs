using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_InputField wordInput;

    [SerializeField] PlayerMovement_Fishing playerMovement;

    [SerializeField] UseFishingRod usefishingRod;

    private string tempWordToCheck = "abe";

    private List<string> wordsOnFish = new List<string>();
    void Start()
    {
        wordInput.onEndEdit.AddListener(OnEndEditing);
        
        wordInput.onSelect.AddListener((string text) => { playerMovement.inputFieldSelected = true; });

        wordInput.onDeselect.AddListener((string text) => { playerMovement.inputFieldSelected = false; });

        wordsOnFish.Add(tempWordToCheck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEndEditing(string text)
    {
        if (wordsOnFish.Contains(text))
        {
            Debug.Log("Correct answer");
            usefishingRod.validWordInputted = true;
        }

    }

   
    
}
