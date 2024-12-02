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
        Debug.Log(startPoint.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && validWordInputted==true)
        {
            useFishingRod = true;
        }

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
                
                validWordInputted = false;
            }

        }

    }
}
