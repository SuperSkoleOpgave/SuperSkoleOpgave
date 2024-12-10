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

    private List<string> words;
    List<GameObject> activeBoxes = new();
    List<string> list = new List<string>();
    /// <summary>
    /// Sets up the bounds of the play area and starts the setup of the boxes
    /// </summary>
    void Start()
    {
        bounds = spawningBox.bounds;
        timeRemaining = Mathf.Infinity;
        StartCoroutine(WaitOnDDA());
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += LoadLetters;
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

    public void RemoveThis(GameObject theObjectToRemove)
    {
        activeBoxes.Remove(theObjectToRemove);
        if(activeBoxes.Count <= 0 )
        {
            List<LanguageUnit> languageUnits = GameManager.Instance.dynamicDifficultyAdjustment.GetWords(new List<LanguageUnitProperty>(), 3);
            words = new List<string>();
            foreach (LanguageUnit languageUnit in languageUnits)
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
        else
        {

            for (global::System.Int32 i = 0; i < dropOffPointThingy.allLettersCollected.Length; i++)
            {
                list.Add(dropOffPointThingy.allLettersCollected[i].ToString());
            }
            SwitchScenes.SwitchToBoxGamePhase2();
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
            conveyerBelt.GetComponent<ConveyerBeltPool>().AddLetters(list, "kat");
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += LoadLetters;
    }
}
