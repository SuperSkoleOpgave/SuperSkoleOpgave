using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private string requiredWord;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private ShelfScript shelfScript;

    private List<GameObject> pooledObjects;

    private Queue<GameObject> queuedObjects;

    [SerializeField]
    private GameObject letterObjectPrefab;

    bool waitingOnLetters = false;

    public bool holdsBox = false;
    // Start is called before the first frame update
    void Start()
    {
        TestSetup();
    }

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
            "s"
        };
        string requiredWord = "kat";
        AddLetters(letters, requiredWord);
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingOnLetters && queuedObjects.Count > 0)
        {
            StartCoroutine(SpawnLetters());
        }
    }

    public void SetLetters(List<string> letters)
    {
        Debug.Log("creating objects for " + letters.Count + " letters");
        pooledObjects = new List<GameObject>();
        queuedObjects = new Queue<GameObject>();
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

    public void ReenterPool(GameObject letterObject)
    {
        letterObject.transform.position = startPoint.position;
        letterObject.SetActive(false);
        queuedObjects.Enqueue(letterObject);
    }

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

    public void AddLetters(List<string> letters, string requiredWord)
    {
        availableLetters = letters;
        this.requiredWord = requiredWord;
        spelledWords = new List<string>();
        possibleWords = new List<string>()
        {
            "kat",
            "hat",
            "b\u00c5d"
        };

        SetLetters(letters);
    }


}
