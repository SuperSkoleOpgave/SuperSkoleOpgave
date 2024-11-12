using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class IngredientHolderPickup : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 startPosition;

    Vector3 mousePosition;
    public bool isDragable = true;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(gameObject.transform.position==startPosition && isDragable==false)
        //{

        //    StartCoroutine(MakeDragableAgainAfterCoolDown());
        //}
      

    }

    IEnumerator MakeDragableAgainAfterCoolDown()
    {
        yield return new WaitForSeconds(4);

        isDragable = true;

    }

    

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    

    private void OnMouseDown()
    {
        if (isDragable == true)
        {
            mousePosition = Input.mousePosition - GetMousePos();
        }
    }

    private void OnMouseDrag()
    {
        if (isDragable == true)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
    }

    private void OnMouseExit()
    {
        isDragable = true;
    }


}
