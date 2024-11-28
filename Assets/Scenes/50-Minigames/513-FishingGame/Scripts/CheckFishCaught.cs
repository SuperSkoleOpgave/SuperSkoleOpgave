using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFishCaught : MonoBehaviour
{
    [SerializeField] FishingGameManager gameManager;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string imageNameOnFish = collision.gameObject.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture.name;

        if(imageNameOnFish==gameManager.wordToCheck)
        {
            collision.gameObject.transform.SetParent(gameObject.transform);


        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
