using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalLoop : MonoBehaviour
{

    [SerializeField]
    private GameObject letterBoxText;

    public TextMeshProUGUI letterText;

    public bool isCorrect = true;

    void Start()
    {
        letterText = letterBoxText.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Gets letter and displays on box
    /// </summary>
    /// <param name="letter"></param>
    public void GetLetter(string letter)
    {
        letterText.text = letter;
    }

    public void ResetCube()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
