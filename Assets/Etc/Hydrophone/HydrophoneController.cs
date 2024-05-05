using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HydrophoneController : MonoBehaviour
{

    public float rotationSpeed = 30f; // Rotation speed of the hydrophone
    public HydrophoneStation hydrophoneManager;
    public float rotationAmount;
    public float engineSpeedInput;

    public GameObject hydrophoneDial;
    public InputHandler inputHandlerScript;
    public InputAction hydrophone;


    // Start is called before the first frame update
    void Start()
    {
        hydrophoneManager = GetComponentInParent<HydrophoneStation>();
        hydrophone = inputHandlerScript.hydrophone;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for rotating left and right
        float rotationInput = hydrophone.ReadValue<float>();

        // Calculate rotation amount based on input and rotation speed
        rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        // Rotate the hydrophone around its y-axis
        transform.Rotate(Vector3.up, rotationAmount);

        // Rotate dial
        hydrophoneDial.transform.Rotate(0f, 0f, rotationAmount);
    }
}
