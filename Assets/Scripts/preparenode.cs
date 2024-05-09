using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour
{
    public bool debugmode;
    // Turn off Meshrender for Monster Nodes
    void Start()
    {
        transform.localScale = new Vector3(30, 100, 30);
        if (!debugmode) { GetComponent<MeshRenderer>().enabled = false; }
        else {
            Material newMat = Resources.Load("Material.003", typeof(Material)) as Material;
            GetComponent<MeshRenderer>().material = newMat;
        }
        
    }
}

