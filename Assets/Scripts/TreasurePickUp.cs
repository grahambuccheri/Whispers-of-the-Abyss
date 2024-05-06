using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePickUp : MonoBehaviour
{
    public GameObject treasure;

    public float pickUpSpeed;
    public float pickUpDistance;

    public int numTreausre;

    public GameObject spawnLocation; // Gameobject that will help spawn other instances of the model

    public GameObject holdableBlackBox;

    void Update()
    {
        if (treasure != null)
        {
            treasure.transform.position = Vector3.Lerp(treasure.transform.position, transform.parent.position, pickUpSpeed);

            if (Vector3.Distance(transform.position, treasure.transform.position) < pickUpDistance)
            {
                numTreausre += 1;
                Destroy(treasure);

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
