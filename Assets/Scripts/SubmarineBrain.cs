using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// man this is a dumb name for a script but its descriptive and I couldn't think of anything better
// Here's some brief commentary about what this does :)
//
// This script is a bridge between the submarine controller script and things that seek to interact with it, such as the main control panel
// The point of doing this with a separate script is so that this "brain" can listen for events such as engine destruction and repair,
// and handle the information pathways accordingly.
//
// As a quick example, we may seek to have the throttle on the control panel stay at its value regardless of the engine state.
// This can create some continuity errors if the scripts in charge of destruction are incongruent with this.
// This script acts as a consolidated path to rectify this.
// In this specific example, we may have the brain listen for destruction and repairs, and have it set the throttle to zero and lock
// on a destruction event, and unlock throttle and set to current control panel throttle on repair.
public class SubmarineBrain : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;
    private SubmarineController submarineController;
    private ControlPanelScript controlPanel;
    // TODO will likely want some event listeners here and established in start()

    // todo this is possibly some repeated effort with the submarine controller script. probably want to guard in just one or than the other rather than both. but it DOES make sense for the brain to know the lock state.
    private bool throttleLockState = false;
    private bool rudderLockState = false;
    private bool buoyancyLockState = false;
    
    // Start is called before the first frame update
    void Start()
    {
        submarineController = GetComponent<SubmarineController>();
        controlPanel = panelObject.GetComponent<ControlPanelScript>();
        if (panelObject == null || submarineController == null || controlPanel == null)
        {
            Debug.LogError("WARNING: Submarine Brain can't find a valid controller and/or control panel attached to this Game Object!");
        }
    }

    // TODO this is simplistic right now, will need to add more logic when more is added to the control flow with destruction and repair.
    void Update()
    {
        if (!throttleLockState)
        {
            submarineController.SetThrottle(controlPanel.leverValue);
        }

        if (!rudderLockState)
        {
            submarineController.SetRudderDeflection(controlPanel.wheelValue);
        }

        if (!buoyancyLockState)
        {
            // nothing here yet
        }
    }
}
