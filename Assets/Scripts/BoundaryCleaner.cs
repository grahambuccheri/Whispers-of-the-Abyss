using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// annoying maintenance script. Deletes any black boxes that leave the game area.
public class BoundaryCleaner : MonoBehaviour
{
    private Collider playArea;

    private void Start()
    {
        playArea = GetComponent<Collider>();
        playArea.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blackbox"))
        {
            Destroy(other.gameObject);
        }
    }
}
