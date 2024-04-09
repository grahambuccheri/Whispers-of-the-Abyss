using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    private CharacterController submarineController;
    private float ShipSpeed { get; } = 0f;
    public float maxShipSpeed = 100f;
    public float shipAcceleration = 12.5f;
    private float EngineRpm { get; } = 0f;
    public float maxEngineRpm = 220f;
    public float engineAcceleration = 50f;
    public float Throttle { get; set; } = 0f;
    public bool ReverseThrottle { get; set; } = false;
    
    // to send signals to other scripts that watch this for changes in values. 
    // TODO implement. Unused rn.
    //public UnityEvent onSubmarineStateChange; // this variable signals a change in any gettable submarine parameters.
    // watch this event in a secondary script if you need to respond to submarine values changing, such as for a tachometer.
    
    // for debug. Activate this mode to enable manual control.
    public bool DebugMode { get; set; } = false;
    public float DebugThrottleIncrement { get; set; } = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO should this be per frame or on a fixed update interval? experiment.
        ProcessThrottle(); // throttle up the engines
        ProcessMovement(); // determine change in motion based on state
        
    }
    
    // internal
    
    // spins engine to correct RPM given ship state.
    // this method is what throttle directly impacts.
    void ProcessThrottle()
    {
        
    }

    // takes into account drag, linear acceleration, and rotation.
    // TODO all unimplemented. do linear accel first.
    void ProcessMovement()
    {
        
    }
}
