using CORE;
using CORE.Scripts;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishingGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_InputField wordInput;

    [SerializeField] PlayerMovement_Fishing FishingPole;

    [SerializeField] UseFishingRod usefishingRod;

    [SerializeField] List<GameObject> fishImageHolders;
    [SerializeField] GameObject water;
    public string wordToCheck;

    [SerializeField] int amountOfFish=3;

    [SerializeField] List<GameObject> fishInSea = new List<GameObject>();

    private int amountOfFishCaught;

    [SerializeField] TextMeshProUGUI fishCaughtScoreUI;
    [SerializeField] GameObject wrongAnswerUIText;

    [SerializeField] GameObject playerSpawnPoint;
    

    private List<string> wordsOnFish = new List<string>();
    private GameObject spawnedPlayer;
    [SerializeField] AudioClip backGroundMusic;

    [SerializeField] GameObject fishingEquipment;
    private PlayerMovement_Fishing playerMovement;

    public bool fishCaught=false;

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            spawnedPlayer = PlayerManager.Instance.SpawnedPlayer;
            spawnedPlayer.GetComponent<Rigidbody>().useGravity = false;
            spawnedPlayer.GetComponent<PlayerFloating>().enabled = false;
            PlayerManager.Instance.PositionPlayerAt(playerSpawnPoint);

            playerMovement = spawnedPlayer.AddComponent<PlayerMovement_Fishing>();
            spawnedPlayer.GetComponent<SpinePlayerMovement>().enabled = false;
            spawnedPlayer.GetComponent<CapsuleCollider>().enabled = true;

            spawnedPlayer.GetComponent<PlayerAnimatior>().SetCharacterState("Idle");



        }
        else
        {
            Debug.Log("WordFactory GM.Start(): Player Manager is null");
        }


        AudioManager.Instance.PlaySound(backGroundMusic, SoundType.Music, true);

        //Setting up the textinputfield with different methods. 
        wordInput.onEndEdit.AddListener(OnEndEditing);
        
        // makes sure that when the text input field is selected the character can't move. 
        wordInput.onSelect.AddListener((string text) => { FishingPole.inputFieldSelected = true; playerMovement.inputFieldSelected = true; wordInput.text = ""; });

      
        SetupNewFish();



     


    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    /// <summary>
    /// Method used when the player is done inputting a word
    /// So when enter is pressed or going out of the text input field. 
    /// </summary>
    /// <param name="text"></param>
    void OnEndEditing(string text)
    {
        FishingPole.inputFieldSelected = false; 
        playerMovement.inputFieldSelected = false;

        Debug.Log("Move:" + playerMovement.inputFieldSelected);

        if (wordsOnFish.Contains(text))
        {
            // Debug.Log("Correct answer");
            wordToCheck = text;
            usefishingRod.validWordInputted = true;
        }
        else
        {
           StartCoroutine(ShowWrongAnswerUIText());
        }

        EventSystem theSystem = EventSystem.current;
        if (!theSystem.alreadySelecting)
        {
            theSystem.SetSelectedGameObject(null);
        }

    }

    /// <summary>
    /// Instatiates a certain amount of new fish on random places in the water. 
    /// </summary>
   void SetupNewFish()
    {
        wordsOnFish.Clear();

        for (int i = 0; i < amountOfFish; i++)
        {
            string wordToAdd = GameManager.Instance.dynamicDifficultyAdjustment.GetWord(new List<LanguageUnitProperty>()).identifier;
            wordsOnFish.Add(wordToAdd);
        }
        

        for (int i = 0; i < amountOfFish; i++)
        {
            float xPos = Random.Range(-360, 360);
            float yPos= Random.Range(-225, 10);


            int rndFishIndex = Random.Range(0, fishImageHolders.Count);

            Texture textureOnFish = ImageManager.GetImageFromWord(wordsOnFish[i]);


            fishImageHolders[rndFishIndex].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = textureOnFish;

            fishInSea.Add(Instantiate(fishImageHolders[rndFishIndex],water.transform));

            RectTransform rectTransform = fishInSea[i].GetComponent<RectTransform>();
            rectTransform.SetParent(water.transform, false);
            rectTransform.localPosition = new Vector3(xPos, yPos, 0);

           
           
        }
    }

    /// <summary>
    /// Method used when a fish is caught and amount of fish caught gets updated and the weight of the word is adjusted in the dda system. 
    /// It of cóurse also removes the fish from the game and checks if all fish are gone new ones get spawned. 
    /// </summary>
    /// <param name="fishCaught"></param>
    public void FishCaught(GameObject fishCaught)
    {
        GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(wordToCheck, true);
        amountOfFishCaught++;
        fishCaughtScoreUI.text = "Fisk Fanget:" + amountOfFishCaught;

        this.fishCaught = false;
        if (fishInSea.Count > 0)
        {
            Destroy(fishCaught);
            fishInSea.Remove(fishCaught);

            if(fishInSea.Count<=0)
            {
                
                SetupNewFish();
            }
        }

        //Debug.Log("fish Caught:" + amountOfFishCaught);



    }

    /// <summary>
    /// Used to show a wrong answer text for a certain amount of time and then hide it after. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowWrongAnswerUIText()
    {
        wrongAnswerUIText.SetActive(true);
        yield return new WaitForSeconds(4);
        wrongAnswerUIText.SetActive(false);
    }
    
}
