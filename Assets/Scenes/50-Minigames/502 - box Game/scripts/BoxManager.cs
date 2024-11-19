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
        SpawnBox();
        
    }

    private void Update()
    {
        if (!positionedPlayer && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.PositionPlayerAt(spawn);
            PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerAtack>();
            positionedPlayer = true;
        }
    }

    void SpawnBox()
    {
        float offsetX = Random.Range(-bounds.extents.x,bounds.extents.x);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        GameObject temp = Instantiate(boxPrefab);

        temp.transform.position = bounds.center + new Vector3(offsetX,0,offsetZ);
        //give letter
        temp.GetComponent<DestroyBox>().symbol = "S";
    }
}
