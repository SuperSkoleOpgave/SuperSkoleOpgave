using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Fishing : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Rigidbody2D rigidbody;

    [SerializeField] int playerAccelleration=50;
    public bool inputFieldSelected=false;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>();   
    }

    private void FixedUpdate()
    {
        if (inputFieldSelected == false)
        {
            if (Input.GetAxis("Horizontal") > 0f)
            {
                rigidbody.AddForce(new Vector2(transform.right.x, transform.right.y) * playerAccelleration);
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                rigidbody.AddForce(-new Vector2(transform.right.x, transform.right.y) * playerAccelleration);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

       
        
    }
}
