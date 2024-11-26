using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFishingRod : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject fishingLine;

    private bool lineIsRolledOut;
    private bool useFishingRod;

    private float yScale;

    [SerializeField] float maxYScale = 7.2f;
    [SerializeField] float minYScale = 0.8f;

   private Vector3 lineScale;

    public bool validWordInputted = false;

    void Start()
    {
        
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
            yScale += 0.01f;
            fishingLine.transform.localScale=new Vector3(1f,minYScale+yScale,1f);

            if(yScale>=maxYScale)
            {
                lineIsRolledOut = true;

                useFishingRod = false;
                yScale = 0;
            }
        }

        if(lineIsRolledOut==true && useFishingRod==true)
        {
            yScale -=0.01f;

            lineScale = new Vector3(1f, maxYScale + yScale, 1f);

            fishingLine.transform.localScale = lineScale;

            if(lineScale.y<=minYScale)
            {
                lineIsRolledOut = false;
                useFishingRod = false;
                yScale = 0;

                validWordInputted = false;
            }

        }

    }
}
