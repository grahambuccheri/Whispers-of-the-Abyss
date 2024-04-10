using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class SubmarineController : MonoBehaviour
{
    
    private CharacterController submarineController;
    [Header("Drag Coefficients")]
    [SerializeField] private float forwardDragCoefficient = 1f;
    [SerializeField] private float verticalDragCoefficient = 1.5f;
    
    [Header("Speed parameters")]
    [SerializeField] private float shipSpeed = 0f; // careful setting this directly.
    [SerializeField] private float maxShipSpeed = 100f;
    [SerializeField] private float maxShipAcceleration = 12.5f;
    [SerializeField] private float verticalSpeed = 0f; // careful setting this directly.
    [SerializeField] private float maxVerticalSpeed = 10f;
    [SerializeField] private float maxVerticalAcceleration = 5f;
    
    [Header("Engine Parameters")] // TODO change me to follow the paradigm of all others! have engine rpm go from 0 to 1. have secondary scripts map this and others like it to actual values.
    [SerializeField] private float engineRpm = 0f; // careful setting this directly. Note this is a proportion of max RPM, so if you read this for a secondary script, map it to your desired RPM range.
    [SerializeField] private float maxEngineAcceleration = 0.25f; // engine revs a quarter of max speed per section by default.

    [Header("Rudder Parameters")] 
    [SerializeField] private bool externalDeflection = false; // check this if you want to set deflection directly.
    [SerializeField] private float rudderDeflection = 0f; // careful setting this directly.
    [SerializeField] private float maxShipRotationRate = 30f; // degrees per second.
    // comment regarding above, if you implement a wheel for the rudder, whatever handles that can likely just directly set deflection based on its own speed stuff.

    [Header("Ballast Parameters")] 
    [SerializeField] private float buoyancy = 0f;
    [SerializeField] private float fillAndDrainRate = 0.1f;
    
    [Header("Control variables")]
    [SerializeField] private float throttle = 0f;
    [SerializeField] private bool reverseThrottle = false;
    [SerializeField] private float targetRudderDeflection = 0f;
    [SerializeField] private float targetBuoyancy = 0f;
    
    // for debug. Activate this mode to enable manual control.
    [Header("Debug Settings")]
    [SerializeField] public bool debugMode = false;
    [SerializeField] public float debugThrottleIncrement = 0.1f;
    [SerializeField] public float debugRudderIncrement = 0.1f;
    [SerializeField] public float debugBallastIncrement = 0.1f;
    
    // interface. Use these from other scripts when interacting with this script.
    
    // Sets throttle to given target. Clamps from 0 to 1.
    void SetThrottle(float target)
    {
        throttle = Mathf.Clamp(target, 0f, 1f);
    }

    // Sets target rudder deflection. Clamps from -1 to 1.
    // -1 represents full deflection right.
    // 1 represents full deflection left.
    // 0 represents no deflection.
    // this will auto un-flag externalDeflection.
    void SetTargetRudderDeflection(float target)
    {
        externalDeflection = false;
        targetRudderDeflection = Mathf.Clamp(target, -1, 1);
    }

    // DIRECTLY sets rudder deflection. Make sure this is your intended effect rather than setting target deflection.
    // Clamps from -1 to +1.
    // -1 represents full deflection right.
    // 1 represents full deflection left.
    // 0 represents no deflection.
    // this will auto flag externalDeflection.
    void SetRudderDeflection(float target)
    {
        externalDeflection = true; 
        rudderDeflection = Mathf.Clamp(target, -1, 1);
    }

    // Sets target buoyancy. Clamps from -1 to 1.
    // -1 represents full negative buoyancy (Sinking).
    // 1 represents full positive buoyancy (rising).
    // 0 represents neutral buoyancy.
    void SetTargetBuoyancy(float target)
    {
        targetBuoyancy = Mathf.Clamp(target, -1, 1);
    }
    
    // Unity functions
    
    // Start is called before the first frame update
    void Start()
    {
        submarineController = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO should this be per frame or on a fixed update interval? experiment.
        UpdateState(); // throttle up the engines
        ProcessMovement(); // determine change in motion based on state
    }
    
    // internal processing. don't touch unless you know what you're doing lol.
    
    // updates state of all properties.
    // Specifically, updates rudder, ballast, and throttle states. DOES NOT IMPART MOVEMENT OR ROTATION.
    void UpdateState()
    {
        HandleThrottle();
        HandleRudder();
        HandleBallast();
    }
    
    // takes into account drag, linear acceleration, and rotation.
    // TODO all unimplemented. do linear accel first.
    void ProcessMovement()
    {
        HandleAngularAccel();
        HandleLinearAccel();
    }
    
    // spins engine to correct RPM given ship state.
    // this method is what throttle directly impacts.
    void HandleThrottle()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SetThrottle(throttle + debugThrottleIncrement);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                SetThrottle(throttle - debugThrottleIncrement);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                reverseThrottle = !reverseThrottle;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                SetThrottle(0);
            }
        }
        
        var direction = reverseThrottle ? -1 : 1;
        
        // if direction is opposite current RPM (aka will decrease magnitude of RPM), then we use 1 as the bound as to not clip aggressively on deceleration.
        var bound = direction * engineRpm < 0 ? 1 : throttle;
        engineRpm = Mathf.Clamp(engineRpm + (maxEngineAcceleration * direction * Time.deltaTime),
            -bound,
            bound);
    }

    void HandleRudder()
    {
        // TODO UNIMPLEMENTED
    }

    // handles fill level of ballast. effectively the same as handle throttle with minor tweaks
    void HandleBallast()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                SetTargetBuoyancy(targetBuoyancy + debugBallastIncrement);
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                SetTargetBuoyancy(targetBuoyancy - debugBallastIncrement);
            }
            else if (Input.GetKeyDown(KeyCode.Backslash))
            {
                SetTargetBuoyancy(0);
            }
        }
        
        var direction = targetBuoyancy > buoyancy ? 1 : -1;
        var bound = targetBuoyancy >= 0 ? targetBuoyancy : -targetBuoyancy;
        // if direction is opposite current RPM (aka will decrease magnitude of RPM), then we use 1 as the bound as to not clip aggressively on deceleration.
        bound = direction * buoyancy < 0 ? 1 : bound;
        buoyancy = Mathf.Clamp(buoyancy + (fillAndDrainRate * direction * Time.deltaTime),
            -bound,
            bound);
    }

    // handles rudder consequences.
    // has two modes of operation. if the hyperparameter externalDeflection is checked, will simply act on whatever the current rudder deflection is.
    // if that parameter is unchecked, will first move the rudder to the target deflection based on set deflection rate.
    // In either case, after rudder deflection is set, will set the rotation speed of the craft.
    void HandleAngularAccel()
    {
        // TODO UNIMPLEMENTED
    }

    void HandleLinearAccel()
    {
        var forwardDeltaV = GetForwardsAcceleration() * Time.deltaTime;
        var verticalDeltaV = GetVerticalAcceleration() * Time.deltaTime;
        
        // todo consider adding another "force" that slows the ship based on severity of current turn.
        // if the ship is in a sharp turn, it should face more resistance. rotational induced acceleration.

        // TODO do we want the ship to behave the same forwards and backwards? probably not.
        shipSpeed = Mathf.Clamp(shipSpeed + forwardDeltaV, -maxShipSpeed, maxShipSpeed);
        verticalSpeed = Mathf.Clamp(verticalSpeed + verticalDeltaV, -maxVerticalSpeed, maxVerticalSpeed);

        var netMotion = shipSpeed * Time.deltaTime * Vector3.forward +
                         verticalSpeed * Time.deltaTime * Vector3.up;

        submarineController.Move(netMotion);
    }

    float GetForwardsAcceleration()
    {
        // engine accelerates ship as a proportion of the ship acceleration parameter based on its speed rel to max rpm.
        // aka maximum ship accel at maximum engine RPM. pretty straight forwards.
        var thrustAcceleration = engineRpm * maxShipAcceleration;
        
        // drag acts opposite of ship direction. based on velocity squared.
        var direction = shipSpeed >= 0 ? 1 : -1;
        var dragAcceleration = -1 * direction * forwardDragCoefficient * (shipSpeed * shipSpeed);

        var netAcceleration = (thrustAcceleration + dragAcceleration);
        return netAcceleration;
    }
    
    // returns resultant acceleration vector from buoyancy related forces (buoyancy + vertical drag)
    // performs similar processing to above method.
    float GetVerticalAcceleration()
    {
        var buoyantAcceleration = buoyancy * maxVerticalAcceleration;
        var direction = verticalSpeed >= 0 ? 1 : -1;
        var dragAcceleration = -1 * direction * verticalDragCoefficient * (verticalSpeed * verticalSpeed);
        var netAcceleration = (buoyantAcceleration + dragAcceleration);
        return netAcceleration;
    }
    
    // private float Normalize(float val, float min, float max)
    // {
    //     if (min > max)
    //     {
    //         (min, max) = (max, min);
    //     } 
    //     else if (Mathf.Approximately(min, max))
    //     {
    //         return 1;
    //     }
    //
    //     val = Mathf.Clamp(val, min, max);
    //     var norm = (val - min) / (max - min);
    //
    //     return norm;
    // }
}
