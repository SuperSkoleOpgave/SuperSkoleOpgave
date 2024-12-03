using CORE;
using CORE.Scripts;
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

    [SerializeField] List<GameObject> fishImageHolders;
    [SerializeField] GameObject water;
    public string wordToCheck;

    [SerializeField] int amountOfFish=3;

    [SerializeField] List<GameObject> fishInSea = new List<GameObject>();

    private int amountOfFishCaught;
    

    private List<string> wordsOnFish = new List<string>();
    void Start()
    {
        wordInput.onEndEdit.AddListener(OnEndEditing);
        
        wordInput.onSelect.AddListener((string text) => { playerMovement.inputFieldSelected = true; });

        wordInput.onDeselect.AddListener((string text) => { playerMovement.inputFieldSelected = false; });

      
        SetupNewFish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEndEditing(string text)
    {
        if (wordsOnFish.Contains(text))
        {
            // Debug.Log("Correct answer");
            wordToCheck = text;
            usefishingRod.validWordInputted = true;
        }

    }

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

            //Debug.Log(xPos + "," + yPos);

            int rndFishIndex = Random.Range(0, fishImageHolders.Count);

            Texture textureOnFish = ImageManager.GetImageFromWord(wordsOnFish[i]);


            fishImageHolders[rndFishIndex].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = textureOnFish;

            fishInSea.Add(Instantiate(fishImageHolders[rndFishIndex],water.transform));

            RectTransform rectTransform = fishInSea[i].GetComponent<RectTransform>();
            rectTransform.SetParent(water.transform, false);
            rectTransform.localPosition = new Vector3(xPos, yPos, 0);

            // instObj.transform.SetParent(water.transform);

           // Debug.Log(fishInSea[i].transform.position);
           
        }
    }


    public void FishCaught(GameObject fishCaught)
    {
        GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(wordToCheck, true);
        amountOfFishCaught++;
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
    
}
