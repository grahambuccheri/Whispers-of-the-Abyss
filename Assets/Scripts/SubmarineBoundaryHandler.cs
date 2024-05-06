using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineBoundaryHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LevelExit"))
        {
            // TODO leave level stuff here
        }

        if (other.CompareTag("LevelExitWarn"))
        {
            // TODO warn close to boundary!
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO
    }
}
