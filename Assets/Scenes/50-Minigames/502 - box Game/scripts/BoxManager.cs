using CORE;
using Scenes;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public BoxCollider spawningBox;
    private Bounds bounds;
    public GameObject spawn;

    [SerializeField]
    private float maxTime;

    [SerializeField]
    private Scrollbar countdownbar;

    [SerializeField]
    private DropOffPointThingy dropOffPointThingy;

    private float timeRemaining;

    private bool positionedPlayer = false;

    private List<string> foundLetters = new List<string>();
    private List<string> words;

    private List<GameObject> activeBoxes = new List<GameObject>();

    private string foundWord = "";

    /// <summary>
    /// Sets up the bounds of the play area and starts the setup of the boxes
    /// </summary>
    void Start()
    {
        bounds = spawningBox.bounds;
        
        StartCoroutine(WaitOnDDA());
    }

    /// <summary>
    /// Waits on the DDA to be loaded and then loads words from it which it then creates boxes for and starts the game timer
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitOnDDA()
    {
        yield return new WaitUntil(()=> GameManager.Instance.dynamicDifficultyAdjustment != null);
        //get a small list of words
        List<LanguageUnit> languageUnits = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 3);
        timeRemaining = maxTime;
        words = new List<string>();
        foreach(LanguageUnit languageUnit in languageUnits)
        {
            words.Add(languageUnit.identifier);
        }
        for (int i = 0; i < words.Count; i++)
        {
            for (int j = 0; j < words[i].Length; j++)
            {
                SpawnBox(words[i][j].ToString());
            }
        }
    }

    /// <summary>
    /// Spawns the player if it hasnt been yet and when counts down on the timer and ends the phase if the timer has run out and the letters of a word has been found 
    /// or if all  boxes has been opened
    /// </summary>
    private void Update()
    {
        //Spawns the player
        if (!positionedPlayer && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(spawn);
            PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerAtack>();
            PlayerManager.Instance.SpawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
            positionedPlayer = true;
        }
        //Counts down on the timer and updates the countdown bar
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownbar.size = timeRemaining / maxTime;
        }
        //Checks if the game can be ended and ends it if possible
        if(((foundWord.Length == 0 && timeRemaining <= 0) || (activeBoxes.Count == 0)) && words != null)
        {
            //Goes through words and checks if the found letters contains the letters of the word
            foreach(string word in words)
            {
                bool lettersFound = true;
                foreach(char letter in word)
                {
                    if(!foundLetters.Contains(letter.ToString()))
                    {
                        lettersFound = false;
                        break;
                    }
                }
                if(lettersFound)
                {
                    foundWord = word;
                    break;
                }
            }
            //ends the game and prepares for the next phase
            if(foundWord.Length > 0)
            {
                string allLetters = dropOffPointThingy.allLettersCollected;
                SceneManager.sceneLoaded += LoadLetters;
                SwitchScenes.SwitchToBoxGamePhase2();
            }  
        }
    }

    /// <summary>
    /// spawns a box with the given letter inside at a random pos within colider
    /// </summary>
    /// <param name="letter">the letter that is inside the spawned box</param>
    void SpawnBox(string letter)
    {
        float offsetX = Random.Range(-bounds.extents.x,bounds.extents.x);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        GameObject temp = Instantiate(boxPrefab);
        activeBoxes.Add(temp);
        temp.transform.position = bounds.center + new Vector3(offsetX,0,offsetZ);
        //give letter
        DestroyBox destroyBox = temp.GetComponent<DestroyBox>();
        destroyBox.symbol = letter;
        destroyBox.boxManager = this;
    }

    /// <summary>
    /// Adds a letter to the found letters and removes the box from the active boxes
    /// </summary>
    /// <param name="letter">The letter which has been found</param>
    /// <param name="box">The box containing the letter</param>
    public void AddLetter(string letter, GameObject box)
    {
        foundLetters.Add(letter);
        activeBoxes.Remove(box);
    }

    /// <summary>
    /// Loads the found letters and the first found word into phase 2 after the scene has been loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    public void LoadLetters(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= LoadLetters;
        GameObject conveyerBelt = GameObject.FindGameObjectWithTag("ConveyerBelt");
        if(conveyerBelt != null)
        {
            conveyerBelt.GetComponent<ConveyerBeltPool>().AddLetters(foundLetters, foundWord);
        }
    }
}
