using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public BoxCollider spawningBox;
    private Bounds bounds;
    public GameObject spawn;

    private bool positionedPlayer = false;

    void Start()
    {
        bounds = spawningBox.bounds;
        //get a small list of words
        List<string> words = new List<string>()
        { 
            "b\u00c5d",
            "kat",
            "hat"
        };
        for (int i = 0; i < words.Count; i++)
        {
            for (int j = 0; j < words[i].Length; j++)
            {
                SpawnBox(words[i][j].ToString());
            }
        }

    }

    private void Update()
    {
        if (!positionedPlayer && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(spawn);
            PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerAtack>();
            PlayerManager.Instance.SpawnedPlayer.GetComponent<Rigidbody>().useGravity = true;
            positionedPlayer = true;
        }
    }

    /// <summary>
    /// spawns a box with the given letter inside at a random pos within colider
    /// </summary>
    /// <param name="letter">the letter that is inside the spawned box</param>
    void SpawnBox(string letter)
    {
        float offsetX = Random.Range(-bounds.extents.x,bounds.extents.x);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        GameObject temp = Instantiate(boxPrefab);

        temp.transform.position = bounds.center + new Vector3(offsetX,0,offsetZ);
        //give letter
        temp.GetComponent<DestroyBox>().symbol = letter;
    }
}
