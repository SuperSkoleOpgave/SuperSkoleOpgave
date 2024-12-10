using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Fishing : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public Rigidbody rigidbody;

    [SerializeField] int playerAccelleration=10;
    public bool inputFieldSelected=false;

    void Start()
    {
       rigidbody= gameObject.GetComponent<Rigidbody>();
        rigidbody.drag = 5;
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
