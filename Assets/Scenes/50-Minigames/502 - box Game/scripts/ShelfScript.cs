using System.Collections;
using System.Collections.Generic;
using CORE;
using UnityEngine;

public class ShelfScript : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private KeyPress confirmKey;

    private Vector3 placementPoint;

    private float deltaX = 0;

    public List<LetterObject> letters;

    [SerializeField]
    private ConveyerBeltPool conveyerBeltPool;
    /// <summary>
    /// Sets up the initial position to place boxes on and the method for the button to call
    /// </summary>
    void Start()
    {
        placementPoint = startPoint.position;
        confirmKey.onClickMethod = VerifyWord;
    }

    /// <summary>
    /// Places a letter on the shelf
    /// </summary>
    /// <param name="letter">The letter to be placed</param>
    public void PlaceLetter(LetterObject letter)
    {
        letter.transform.position = placementPoint;
        if(deltaX == 0)
        {
            deltaX = letter.GetComponent<MeshRenderer>().bounds.size.x + 1;
        }
        placementPoint = new Vector3(placementPoint.x + deltaX, placementPoint.y, placementPoint.z);
        if(letters == null)
        {
            letters = new List<LetterObject>();
        }
        letters.Add(letter);
    }

    /// <summary>
    /// Removes a letter from the shelf and moves the other letters back on the shelf as needed
    /// </summary>
    /// <param name="letter">The letter to be removed</param>
    public void RemoveLetter(LetterObject letter)
    {
        Vector3 letterPos = letter.transform.position;
        placementPoint = new Vector3(placementPoint.x - deltaX, placementPoint.y, placementPoint.z);
        for (int i = 0; i < letters.Count; i++)
        {
            LetterObject l = letters[i];
            if (l.transform.position.x > letterPos.x)
            {
                l.transform.position = new Vector3(l.transform.position.x - deltaX, l.transform.position.y, l.transform.position.z);
            }
        }
    }

    /// <summary>
    /// Checks that the currently spelled word is correct
    /// </summary>
    public void VerifyWord()
    {
        string word = "";
        for(int i = 0; i < letters.Count; i++)
        {
            word += letters[i].letter;
            
        }
        if(conveyerBeltPool.VerifyWord(word))
        {
            GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(word, true);
            conveyerBeltPool.RemoveFromPossibleWords(word);
            foreach(LetterObject letter in letters)
            {
                letter.fromConveyer = true;
                conveyerBeltPool.ReenterPool(letter.gameObject);
            }
            letters.Clear();
        }
        else 
        {
            GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(conveyerBeltPool.requiredWord, false);
        }
    }
}
