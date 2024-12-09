using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFishCaught : MonoBehaviour
{
    [SerializeField] FishingGameManager gameManager;

    /// <summary>
    /// Checks that the fish collided with the hook is the word to check for. 
    /// The word to check for is based on what word was inputted in the text input field. 
    /// If its the right fish the fish will become a child to the hook and follow the hook object. 
    /// </summary>
    /// <param name="collision"></param>
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
