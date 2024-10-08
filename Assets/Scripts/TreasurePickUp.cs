using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TreasurePickUp : MonoBehaviour
{
    public RunStats runStats;
        
    public GameObject treasure;
    public int treasureWorth = 50;
    public int treasureScoreValue = 100;

    public float pickUpSpeed;
    public float pickUpDistance;

    [FormerlySerializedAs("numTreausre")] public int numTreasure;

    public GameObject spawnLocation; // Gameobject that will help spawn other instances of the model

    public GameObject holdableBlackBox;

    void Update()
    {
        if (treasure != null)
        {
            treasure.transform.position = Vector3.Lerp(treasure.transform.position, transform.parent.position, pickUpSpeed);

            if (Vector3.Distance(transform.position, treasure.transform.position) < pickUpDistance)
            {
                numTreasure += 1;
                Destroy(treasure);
                runStats.PickupItem(treasureScoreValue, treasureWorth);
                
                Instantiate(holdableBlackBox, spawnLocation.transform.position, Quaternion.Euler(new Vector3(-90, 0f, 0f)));

                treasure = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("blackbox") && treasure == null)
        {
            treasure = other.gameObject;
            treasure.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("blackbox") && treasure != null)
        {
            treasure.GetComponent<Rigidbody>().useGravity = true;
            treasure = null;
        }
    }
}
