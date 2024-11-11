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

    bool clicked = false;

    Button button;

    Vector3 mousePosition;
    void Start()
    {
        button=gameObject.transform.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
      

    }

    


    void MoveIngredient()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {

            gameObject.transform.position = hit.transform.position;
        }

    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }


}
