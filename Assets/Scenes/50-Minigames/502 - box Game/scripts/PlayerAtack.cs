using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    BoxCollider hitBox;

    List<GameObject> targets = new List<GameObject>();
    void Start()
    {
        hitBox = gameObject.AddComponent<BoxCollider>();
        hitBox.isTrigger = true;
        hitBox.center = new(0.15122f, -7.113449f, 1.515244f);
        hitBox.size = new(4.903893f, 15.22691f, 4.030489f);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            for(int i = 0; i < targets.Count; i++)
            {
                Destroy(targets[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GameObject>())
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }

    void Exit()
    {
        Destroy(hitBox);
        Destroy(this);
    }
}
