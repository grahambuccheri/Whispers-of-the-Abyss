using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour
{
    // Turn off Meshrender for Monster Nodes
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}

