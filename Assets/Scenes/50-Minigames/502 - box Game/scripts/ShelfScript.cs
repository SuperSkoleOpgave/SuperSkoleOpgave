using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfScript : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;

    private Vector3 placementPoint;

    private float deltaX = 0;

    public List<LetterObject> letters;

    [SerializeField]
    private ConveyerBeltPool conveyerBeltPool;
    // Start is called before the first frame update
    void Start()
    {
        placementPoint = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceLetter(LetterObject letter)
    {
        letter.transform.position = placementPoint;
        if(deltaX == 0)
        {
            deltaX = letter.GetComponent<MeshRenderer>().bounds.size.x + 1;
        }
        placementPoint = new Vector3(placementPoint.x + deltaX, placementPoint.y, placementPoint.z);
        if(letters == null)
        {
            letters = new List<LetterObject>();
        }
        letters.Add(letter);
    }

    public void RemoveLetter(LetterObject letter)
    {
        Vector3 letterPos = letter.transform.position;
        letters.Remove(letter);
        placementPoint = new Vector3(placementPoint.x - deltaX, placementPoint.y, placementPoint.z);
        for (int i = 0; i < letters.Count; i++)
        {
            LetterObject l = letters[i];
            Debug.Log(l.letter);
            if (l.transform.position.x > letterPos.x)
            {
                l.transform.position = new Vector3(l.transform.position.x - deltaX, l.transform.position.y, l.transform.position.z);
            }
        }
    }

    public void VerifyWord()
    {
        string word = "";
        for(int i = 0; i < letters.Count; i++)
        {
            word += letters[i].letter;
        }
        if(conveyerBeltPool.VerifyWord(word))
        {
            conveyerBeltPool.RemoveFromPossibleWords(word);
        }
    }
}
