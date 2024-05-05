using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrophoneController : MonoBehaviour
{

    public float rotationSpeed = 30f; // Rotation speed of the hydrophone
    public HydrophoneStation hydrophoneManager;
    public float rotationAmount;
    public float engineSpeedInput;

    // Start is called before the first frame update
    void Start()
    {
        hydrophoneManager = GetComponentInParent<HydrophoneStation>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for rotating left and right
        //Alex please edit this so it works for you
        float rotationInput = Input.GetAxis("Horizontal");

        // Calculate rotation amount based on input and rotation speed
        rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        // Rotate the hydrophone around its y-axis
        transform.Rotate(Vector3.up, rotationAmount);
        engineSpeedInput = Input.GetAxis("Vertical");
        hydrophoneManager.engineSpeed += engineSpeedInput * hydrophoneManager.throttleRate * Time.deltaTime;
        hydrophoneManager.engineSpeed = Mathf.Clamp01(hydrophoneManager.engineSpeed);
    }
}
