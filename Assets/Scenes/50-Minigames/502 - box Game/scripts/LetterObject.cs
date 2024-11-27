using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public ConveyerBeltPool conveyerBeltPool;
    public Vector3 endPoint;

    public ShelfScript shelfScript;

    float speed = 5;

    bool selected = false;
    
    public bool fromConveyer = true;

    public string letter;



    /// <summary>
    /// Moves the gameobject along the conveyerbelt unless it is the currently selected object. 
    /// In that case it follows the mouse and if the player clicks on the place they want to move it to it gets placed there´. Or if rightclicking it gets moved back to where it came from 
    /// </summary>
    void Update()
    {
        if(gameObject.activeSelf && transform.position != endPoint && !selected && fromConveyer)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint,  speed * Time.deltaTime);
        }
        if(transform.position == endPoint && !selected && fromConveyer)
        {
            conveyerBeltPool.ReenterPool(gameObject);
        }
        if(selected)
        {
            LayerMask layerMask = LayerMask.GetMask("Terrain");
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out var hit, 100, layerMask)) {
                transform.position = hit.point;
                if(Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.gameObject.tag == "ConveyerBelt" && !fromConveyer)
                {
                    selected = false;
                    conveyerBeltPool.holdsBox = false;
                    fromConveyer = true;
                    transform.position = new Vector3(transform.position.x, endPoint.y, endPoint.z);
                }
                if(Input.GetKeyDown(KeyCode.Mouse1) && hit.collider.gameObject.tag == "ConveyerBelt" && fromConveyer)
                {
                    selected = false;
                    conveyerBeltPool.holdsBox = false;
                    transform.position = new Vector3(transform.position.x, endPoint.y, endPoint.z);
                }
                if(Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.gameObject.tag == "WordShelf" && fromConveyer)
                {
                    selected = false;
                    conveyerBeltPool.holdsBox = false;
                    fromConveyer = false;
                    shelfScript.PlaceLetter(this);
                }
                if(Input.GetKeyDown(KeyCode.Mouse1) && hit.collider.gameObject.tag == "WordShelf" && !fromConveyer)
                {
                    selected = false;
                    conveyerBeltPool.holdsBox = false;
                    shelfScript.PlaceLetter(this);
                }
            }
        }
    }

    /// <summary>
    /// Starts moving the box if the player clicks on it
    /// </summary>
    void OnMouseDown()
    {
        if(!conveyerBeltPool.holdsBox && !selected)
        {
            selected = true;
            conveyerBeltPool.holdsBox = true;
            if(!fromConveyer)
            {
                shelfScript.RemoveLetter(this);
            }
        }
    }
}
