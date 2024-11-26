using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Manages a box object that displays a symbol and handles its lifecycle in the game.
/// </summary>
public class DestroyBox : MonoBehaviour
{
    [SerializeField]
    private GameObject letterBox;
    
    public string symbol;
    public BoxManager boxManager;

    /// <summary>
    /// Ensures the letterbox prefab has been assigned
    /// </summary>
    private void Awake()
    {
        if (letterBox == null)
            Debug.LogError($"Required reference 'letterBox' is missing on {gameObject.name}");
    }

    /// <summary>
    /// Sets the text on the letterbox to the given symbol
    /// </summary>
    private void Start()
    {
        var text = letterBox.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < text.Length; i++)
        {
            text[i].text = symbol;
        }
    }

    /// <summary>
    /// Spawns a letter box and destroys itself afterward
    /// </summary>
    public void Destroy()
    {
        Instantiate(letterBox, transform.position+Vector3.up,Quaternion.identity);
        boxManager.AddLetter(symbol, gameObject);
        Destroy(gameObject);
    }
}
