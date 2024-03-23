using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    public bool interacting;
    public bool toolFlag; // Indicated whether the object we are interacting with is a tool (pickable object)
    public Sprite itemInteracting;

    // Name of item we are interacting with
    public string itemInteractingName;

    // Item we are interacting with, this can be an object you pick up or something on the ship you control.
    public GameObject item;

    public void Reset()
    {
        if (item != null)
        {
            item.transform.parent = null; // Unparent child
        }
        interacting = false;
        toolFlag = false;
        itemInteracting = null;
        itemInteractingName = null;
        item = null;
    }
}
