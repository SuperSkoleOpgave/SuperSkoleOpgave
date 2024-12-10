using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages a box object that displays a symbol and handles its lifecycle in the game.
/// </summary>
public class DestroyBox : MonoBehaviour
{
    [SerializeField]
    private GameObject letterBox;
    [SerializeField]
    private GameObject SFX;

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
    /// Spawns a letter box and destroys itself afterward
    /// </summary>
    public void Destroy()
    {
        boxManager.RemoveThis(gameObject);
        GameObject temp = Instantiate(letterBox, transform.position + Vector3.up *0.5f,Quaternion.identity);
        var text = temp.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < text.Length; i++)
        {
            text[i].text = symbol;
        }
        Instantiate(SFX, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Destroy(gameObject);
    }
}
