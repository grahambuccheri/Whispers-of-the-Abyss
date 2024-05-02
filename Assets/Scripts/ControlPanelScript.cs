using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ControlPanelScript : MonoBehaviour, IInteractableShipObject
{
    [SerializeField] private InputHandler inputHandlerScript;
    [SerializeField] private InputAction steer;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Vector3 targetPlayerPosition;
    [SerializeField] private Vector3 offset;

    [Header("Steering Settings")]
    [SerializeField] private float wheelRotationRate; // Rate at which wheel rotates

    // WHEEL VALUE BELOW
    [SerializeField] public float wheelValue; // Clamped Value from -1 to 1
    [SerializeField] private GameObject wheel;

    [Header("Lever Settings")]
    [SerializeField] private List<int> leverInput;
    [SerializeField] private float leverInterval; // Interval between each lever press
    [SerializeField] private float secondLeverInterval; // Time between each lever press

    [SerializeField] private GameObject leverHandle;
    [SerializeField] private int leverMaxMin;
    public float leverValue;




    public void disableAttributes()
    {
        Debug.Log("Disabling Player Controls For: Control Panel");
        inputHandlerScript.playerControls.FindAction("Move").Disable();
        inputHandlerScript.characterController.enabled = false;

        // Enable the steering and lever actions
        inputHandlerScript.playerControls.FindAction("Steer").Enable();
        inputHandlerScript.playerControls.FindAction("Lever").Enable();

        // Handle player position in front of the control panel
        targetPlayerPosition = transform.position + offset;
        playerTransform.position = targetPlayerPosition;

    }

    public void interact()
    {

    }

    public void enableAttributes()
    {
        Debug.Log("Enabling Player Controls For: Control Panel");
        inputHandlerScript.playerControls.FindAction("Move").Enable();
        inputHandlerScript.characterController.enabled = true;

        // Disable the steering and lever actions
        inputHandlerScript.playerControls.FindAction("Steer").Disable();
        inputHandlerScript.playerControls.FindAction("Lever").Disable();
    }

    void Start()
    {
        steer = inputHandlerScript.steer;

        leverInput = inputHandlerScript.leverInput;

        StartCoroutine(Lever());
    }

    // Update is called once per frame
    void Update()
    {
        Steer();
    }

    private void Steer()
    {
        float wheelDirection = steer.ReadValue<float>();

        wheelValue += wheelDirection * wheelRotationRate * Time.deltaTime;

        // Wheel values from -1 to 1, inputted in submarine controls
        wheelValue = Mathf.Clamp(wheelValue, -1f, 1f);

        // Scale the z rotation by 90 to limit from -90 to 90 degrees
        wheel.transform.localEulerAngles = new Vector3(-90, 180, wheelValue * 90);
    }

    // This method is highly inspired by the inchworm project. It takes in a queue of direction (up and down, W and S resoectively) and reads it
    // One by one.
    private IEnumerator Lever()
    {
        WaitUntil leverDir = new WaitUntil(() => leverInput.Count > 0);

        while (true)
        {
            yield return leverDir;

            int leverHead = leverInput[0];
            leverInput.RemoveAt(0);

            if (leverHead == 1 && leverValue < 1)
            {
                leverValue += leverInterval;
                leverHandle.transform.localPosition += new Vector3(0, 0, leverInterval * leverMaxMin);
                yield return new WaitForSeconds(secondLeverInterval);
            }
            else if (leverHead == -1 && leverValue > -1)
            {
                leverValue -= leverInterval;
                leverHandle.transform.localPosition -= new Vector3(0, 0, leverInterval * leverMaxMin);
                yield return new WaitForSeconds(secondLeverInterval);
            }
            else
            {
                yield return null;
            }

        }
    }
}
