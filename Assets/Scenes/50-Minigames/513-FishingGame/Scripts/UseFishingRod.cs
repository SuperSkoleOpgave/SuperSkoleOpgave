using CORE;
using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFishingRod : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] LineRenderer fishingLine;

    private bool lineIsRolledOut=false;
    private bool useFishingRod=false;

  

    [SerializeField] float maxLenght = 7.2f;
  
    [SerializeField] float minLength = 0.8f;
    [SerializeField] float currentLength;
    [SerializeField] float lineSpeed = 1f;
    [SerializeField] Transform startPoint;

    private Vector3 lineScale;

    [SerializeField] GameObject hook;

    public bool validWordInputted = false;

    public Vector3 lineEndPos { get; private set; }

    [SerializeField] FishingGameManager gameManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Checks if the f is pressed and a valid word has been inputted into the text input field
        if(Input.GetKeyDown(KeyCode.F) && validWordInputted==true)
        {
            useFishingRod = true;
        }

        // code that makes sure the linerendere goes down to a certain lenght and makes sure the hooke follows along. 
        if (lineIsRolledOut == false && useFishingRod==true)
        {
            currentLength -= lineSpeed * Time.deltaTime;

            lineEndPos = startPoint.localPosition + new Vector3(0, currentLength, 0);

            fishingLine.SetPosition(1, lineEndPos);
            hook.transform.localPosition = lineEndPos;
            if(currentLength<=maxLenght)
            {
                lineIsRolledOut = true;

                useFishingRod = false;
                
            }
        }


        //Makes sure if the line is rolled out to get the hook and line up to the start point. 
        // Also checks if the hook has a child attached and if thats the case a fish has been caugt and a method fishCaught is used from the gamemanager. 
        // if nothing has been caught the words weight gets adjusted accordingly. 
        if(lineIsRolledOut==true && useFishingRod==true)
        {
            currentLength += lineSpeed * Time.deltaTime;

            lineEndPos = startPoint.localPosition + new Vector3(0, currentLength, 0);

            fishingLine.SetPosition(1, lineEndPos);
            hook.transform.localPosition = lineEndPos;

            if (currentLength>=minLength)
            {
                lineIsRolledOut = false;
                useFishingRod = false;
               
                if(hook.transform.childCount>0)
                {
                    for (int i = 0; i < hook.transform.childCount; i++)
                    {
                        gameManager.FishCaught(hook.transform.GetChild(i).gameObject);
                    }
                   
                }
                else
                {
                    GameManager.Instance.dynamicDifficultyAdjustment.AdjustWeightWord(gameManager.wordToCheck, false);
                }
                
                

                validWordInputted = false;
            }

        }

    }
}
