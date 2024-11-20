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
    
    bool fromConveyer = true;

    public string letter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
