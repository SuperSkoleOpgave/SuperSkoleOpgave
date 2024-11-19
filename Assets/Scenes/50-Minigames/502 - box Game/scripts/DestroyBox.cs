using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyBox : MonoBehaviour
{
    public GameObject letterBox;
    public string symbol;
    private void Start()
    {
        var text = letterBox.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < text.Length; i++)
        {
            text[i].text = symbol;
        }
    }

    /// <summary>
    /// spawns letter and destroyes itself after
    /// </summary>
    public void Destroy()
    {
        Instantiate(letterBox, transform.position+Vector3.up,Quaternion.identity);
        Destroy(gameObject);
    }
}
