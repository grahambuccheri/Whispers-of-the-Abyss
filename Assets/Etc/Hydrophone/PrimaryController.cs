using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryController : MonoBehaviour
{
    public bool engineReversed = false; //handles whether the engine is in reverse. Must stop engine to reverse
    public bool generatorOn = false; //if true, charge battery, otherwise do not.
    public float throttle = 0f; //handles throttle of electric motor. 0 if off, 1 is max speed
    public float batteryCharge = 0f; // handles charge percentage
    public float hydrophoneRotation = 0f; //calculated in degrees with 0 being straight forward
    public float rudderDeflection = 0f; //min deflection is -35 degrees, max is +35 degrees
    public float ballastLevel = 0.5f; // Level of 0 is max downward sink rate, 0.5 is neutrally buoyant, 1 is max float upwards

    // Start is called before the first frame update
    void Start()
    {
        // Initialize any necessary variables or components
    }

    // Update is called once per frame
    void Update()
    {
        // Handle engine reverse
        if (Input.GetKeyDown(KeyCode.R) && throttle == 0f)
        {
            engineReversed = !engineReversed;
        }

        // Handle generator
        if (Input.GetKeyDown(KeyCode.G))
        {
            generatorOn = !generatorOn;
        }

        // Handle throttle
        throttle = Input.GetAxis("Vertical"); // Assuming vertical axis is used for throttle

        // Handle battery charge
        if (generatorOn)
        {
            batteryCharge += Time.deltaTime * 0.1f; // Charge battery at a rate of 0.1 per second
        }
        else
        {
            batteryCharge -= Time.deltaTime * throttle * 0.2f; // Discharge battery based on throttle
        }
        batteryCharge = Mathf.Clamp01(batteryCharge); // Clamp battery charge between 0 and 1

        // Handle hydrophone rotation
        hydrophoneRotation += Input.GetAxis("Horizontal") * 90f * Time.deltaTime; // Assuming horizontal axis is used for hydrophone rotation
        hydrophoneRotation = Mathf.Repeat(hydrophoneRotation, 360f); // Wrap rotation around 360 degrees

        // Handle rudder deflection
        rudderDeflection = Input.GetAxis("Horizontal") * 35f; // Assuming horizontal axis is used for rudder deflection
        rudderDeflection = Mathf.Clamp(rudderDeflection, -35f, 35f); // Clamp rudder deflection between -35 and 35 degrees

        // Handle ballast level
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ballastLevel += 0.1f; // Increase ballast level by 0.1
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ballastLevel -= 0.1f; // Decrease ballast level by 0.1
        }
        ballastLevel = Mathf.Clamp01(ballastLevel); // Clamp ballast level between 0 and 1
    }
}