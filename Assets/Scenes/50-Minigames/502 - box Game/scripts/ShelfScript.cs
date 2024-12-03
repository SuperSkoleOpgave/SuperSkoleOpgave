using System.Collections;
using System.Collections.Generic;
using CORE;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

public class ShelfScript : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform turnPoint;

    [SerializeField]
    private Transform despawnPoint;

    [SerializeField]
    private KeyPress confirmKey;

    [SerializeField]
    private GameObject coinPrefab;

    private Vector3 placementPoint;

    private float deltaX = 0;

    public List<LetterObject> letters;

    [SerializeField]
    private ConveyerBeltPool conveyerBeltPool;

    [SerializeField]
    private MeshRenderer beltRenderer;

    private Material beltMaterial;
    
    [SerializeField]
    private MeshRenderer returnBeltRenderer;
    private Material returnBeltMaterial;
    bool moving = false;
    /// <summary>
    /// Sets up the initial position to place boxes on and the method for the button to call. Also retrieves the materials from the belt renderers
    /// </summary>
    void Start()
    {
        placementPoint = startPoint.position;
        confirmKey.onClickMethod = VerifyWord;
        beltMaterial = beltRenderer.material;
        returnBeltMaterial = returnBeltRenderer.material;
    }

    /// <summary>
    /// Moves the belt after the player has spelled a word and until all letters has been returned to the pool
    /// </summary>
    void Update()
    {
        if(moving)
        {
            float speed = 0.25f;
            beltMaterial.mainTextureOffset = new Vector2(beltMaterial.mainTextureOffset.x + speed * Time.deltaTime, beltMaterial.mainTextureOffset.y);
            returnBeltMaterial.mainTextureOffset = new Vector2(returnBeltMaterial.mainTextureOffset.x - speed * Time.deltaTime, returnBeltMaterial.mainTextureOffset.y);
        }
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
    /// Removes a letter from the shelf and moves the other letters back on the shelf as needed while the players 
    /// </summary>
    /// <param name="letter">The letter to be removed</param>
    public void RemoveLetter(LetterObject letter)
    {
        if(!moving)
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
            letters.Remove(letter);
        }
        else
        {
            letters.Remove(letter);
            if(letters.Count == 0)
            {
                moving = false;
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
            PlayerEvents.RaiseGoldChanged(1);
            PlayerEvents.RaiseXPChanged(1);
            Instantiate(coinPrefab);
            conveyerBeltPool.RemoveFromPossibleWords(word);
            foreach(LetterObject letter in letters)
            {
                
                letter.turnPoint = turnPoint.position;
                letter.despawnPoint = despawnPoint.position;
                letter.spelledWord = true;
            }
            //letters.Clear();
            moving = true;

        }
        else 
        {
            GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(conveyerBeltPool.requiredWord, false);
        }

    }
}
