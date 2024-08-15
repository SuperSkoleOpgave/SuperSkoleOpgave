using System.Collections;
using UnityEngine;


/// <summary>
/// Used to destroy a gameobject on contact with the player
/// </summary>
public class DestroyOnContact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Destroys the object when it overlaps with the player
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            StartCoroutine(DestroyWait());
        }
        
    }

    IEnumerator DestroyWait(){
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
