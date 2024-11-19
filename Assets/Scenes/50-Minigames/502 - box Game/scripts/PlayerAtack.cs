using Scenes._10_PlayerScene.Scripts;
using Scenes._20_MainWorld.Scripts;
using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    BoxCollider hitBox;

    List<DestroyBox> targets = new List<DestroyBox>();
    string inventory = "";
    private PlayerAnimatior animatior;
    void Start()
    {
        animatior = GetComponent<PlayerAnimatior>();
        hitBox = gameObject.AddComponent<BoxCollider>();
        hitBox.isTrigger = true;
        hitBox.center = new(0.6743157f, -5.974327f, -1.754525f);
        hitBox.size = new(11.18315f, 12.94866f, 7.647582f);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            for(int i = 0; i < targets.Count; i++)
            {
                targets[i].Destroy();
            }
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * 7;
        movement = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * movement;
        // Move the player
        GetComponent<Rigidbody>().velocity = new(movement.x, GetComponent<Rigidbody>().velocity.y, movement.z);

        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            animatior.SetCharacterState("Walk");
        }
        else
        {
            animatior.SetCharacterState("Idle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<DestroyBox>())
        {
            targets.Add(other.GetComponent<DestroyBox>());
        }
        if (other.GetComponentInChildren<TextMeshProUGUI>())
        {
            inventory += other.GetComponentInChildren<TextMeshProUGUI>().text;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(targets.Contains(other.GetComponent<DestroyBox>()))
        {
            targets.Remove(other.GetComponent<DestroyBox>());
        }

    }

    void Exit()
    {
        Destroy(hitBox);
        Destroy(this);
    }
}
