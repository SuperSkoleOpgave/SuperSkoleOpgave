using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CORE;
using CORE.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConveyerBeltPool : MonoBehaviour
{
    [SerializeField]
    private List<string> availableLetters;
    [SerializeField]
    private List<string> spelledWords;
    [SerializeField]
    private List<string> possibleWords;
    public string requiredWord;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private ShelfScript shelfScript;

    [SerializeField]
    private MeshRenderer meshRenderer;
    private Material beltMaterial;
    [SerializeField]
    private MeshRenderer returnBeltRenderer;
    private Material returnBeltMaterial;

    [SerializeField]
    private List<GameObject> pooledObjects;

    private Queue<GameObject> queuedObjects;

    [SerializeField]
    private GameObject letterObjectPrefab;


    bool waitingOnLetters = false;

    public bool holdsBox = false;
    
    private void Start()
    {
        beltMaterial = meshRenderer.material;
        returnBeltMaterial = returnBeltRenderer.material;
        //TestSetup();
    }

    /// <summary>
    /// Used if the second phase needs to be tested in isolation
    /// </summary>
    private void TestSetup()
    {
        List<string> letters = new List<string>()
        {
            "k",
            "a",
            "t",
            "h",
            "e",
            "p",
            "q",
            "w",
            "r",
            "y",
            "u",
            "i",
            "o",
            "å",
            "s",
            "a",
            "l",
            "f"
        };
        string requiredWord = "kat";
        AddLetters(letters, requiredWord);
    }

    /// <summary>
    /// Restarts letter spawning when letters are added to the queue
    /// </summary>
    void Update()
    {
        if(waitingOnLetters && queuedObjects.Count > 0)
        {
            StartCoroutine(SpawnLetters());
        }
        MoveBelt();
    }

    /// <summary>
    /// Sets up gameobjects for the given letters
    /// </summary>
    /// <param name="letters">The letters to use</param>
    public void SetLetters(List<string> letters)
    {
        pooledObjects = new List<GameObject>();
        queuedObjects = new Queue<GameObject>();
        //Goes through each letter and sets up its gameobject and adds the LetterObject script to pooledObjects and queuedObjects
        foreach(string letter in letters)
        {
            GameObject letterObject = Instantiate(letterObjectPrefab);
            var text = letterObject.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < text.Length; i++)
            {
                text[i].text = letter;
            }

            letterObject.transform.position = startPoint.position;
            letterObject.SetActive(false);
            pooledObjects.Add(letterObject);
            queuedObjects.Enqueue(letterObject);
            LetterObject letterObjectScript = letterObject.GetComponent<LetterObject>();
            letterObjectScript.endPoint = endPoint.position;
            letterObjectScript.conveyerBeltPool = this;
            letterObjectScript.shelfScript = shelfScript;
            letterObjectScript.letter = letter;
        }
        StartCoroutine(SpawnLetters());
    }

    /// <summary>
    /// Resets a letterobjects position and adds it to the back of queuedObjects
    /// </summary>
    /// <param name="letterObject">The gameobject to use</param>
    public void ReenterPool(GameObject letterObject)
    {
        letterObject.transform.position = startPoint.position;
        letterObject.SetActive(false);
        
        queuedObjects.Enqueue(letterObject);
    }

    /// <summary>
    /// Continually spawns letters until the list is empty
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnLetters()
    {

        if(queuedObjects.Count > 0)
        {
            queuedObjects.Dequeue().SetActive(true);
            yield return new WaitForSeconds(2);
            StartCoroutine(SpawnLetters());
        }
        else
        {
            waitingOnLetters = true;
        }
    }

    /// <summary>
    /// Adds the letters and the required word found in phase 1
    /// </summary>
    /// <param name="letters">The letters found</param>
    /// <param name="requiredWord">The required word which letters from was guaranteed</param>
    public void AddLetters(List<string> letters, string requiredWord)
    {
        availableLetters = letters;
        this.requiredWord = requiredWord;
        spelledWords = new List<string>();
        StartCoroutine(WaitOnWordsLoaded());
        
        SetLetters(letters);
    }

    /// <summary>
    /// Waits on words getting added to the words manager and then saves the ones which can be spelled with the found letters
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitOnWordsLoaded()
    {
        yield return new WaitUntil(() => GameManager.Instance.dynamicDifficultyAdjustment != null);
        List<string> allWords = GameManager.Instance.dynamicDifficultyAdjustment.GetWordStrings();
        //Goes through the words in allWords and checks which can be spelled with the letters found in phase 1
        foreach(string word in allWords)
        {
            bool wordContained = true;
            Dictionary<char, int> usedLetters = new Dictionary<char, int>();
            foreach(char letter in word)
            {
                if(availableLetters.Contains(letter.ToString().ToLower()) && !usedLetters.Keys.Contains(letter))
                {
                    usedLetters.Add(letter, 1);
                }
                else if(usedLetters.Keys.Contains(letter))
                {
                    int count = 0;
                    foreach(string letter1 in availableLetters)
                    {
                        if(letter.ToString().ToLower() == letter1)
                        {
                            count++;
                        }
                        if(count > usedLetters[letter])
                        {
                            usedLetters[letter] = count;
                            break;
                        }
                    }
                    if(count < usedLetters[letter])
                    {
                        wordContained = false;
                        break;
                    }
                }
                else
                {
                    wordContained = false;
                    break;
                }
            }
            if(wordContained)
            {
                possibleWords.Add(word);
            }
        }
    }

    /// <summary>
    /// Checks that the given word is correct and hasnt been spelled yet
    /// </summary>
    /// <param name="word">The word to be checked</param>
    /// <returns>Whether it is correct</returns>
    public bool VerifyWord(string word)
    {
        return possibleWords.Contains(word);
    }

    /// <summary>
    /// Removes a spelled word from possiblewords and move it to the spelledwords list
    /// </summary>
    /// <param name="word">The word to be moved</param>
    public void RemoveFromPossibleWords(string word)
    {
        possibleWords.Remove(word);
        spelledWords.Add(word);
    }

    /// <summary>
    /// Gives the illusion of moving the belt by offsetting the belt
    /// </summary>
    private void MoveBelt()
    {
        float speed = 0.25f;
        beltMaterial.mainTextureOffset = new Vector2(beltMaterial.mainTextureOffset.x + speed * Time.deltaTime, beltMaterial.mainTextureOffset.y);
        returnBeltMaterial.mainTextureOffset = new Vector2(returnBeltMaterial.mainTextureOffset.x - speed * Time.deltaTime, returnBeltMaterial.mainTextureOffset.y);
    }

}
