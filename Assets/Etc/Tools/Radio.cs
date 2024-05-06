using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour, IInteractableTool
{
    public ToolData SO;
    public ToolData data
    {
        get => SO;
        set => data = SO;
    }

    public void interact()
    {
        return;
    }

    public void stopInteract()
    {
        return;
    }
}
