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

    [Header("Throttle Lever Settings")]
    [SerializeField] private List<int> leverInput;
    [SerializeField] private float leverInterval; // Interval between each lever press
    [SerializeField] private float secondLeverInterval; // Time between each lever press

    [SerializeField] private GameObject leverHandle;
    [SerializeField] private int leverMaxMin;
    public float leverValue;

    [Header("Height Lever Settings")]
    [SerializeField] private List<int> heightLeverInput;
    [SerializeField] private float heightLeverInterval; // Interval between each hheight lever press
    [SerializeField] private float secondHeightLeverInterval; // Time between each height lever press

    [SerializeField] private GameObject heightLeverHandle;
    [SerializeField] private int heightLeverMaxMin;
    public float heightLeverValue;


    public void disableAttributes()
    {
        Debug.Log("Disabling Player Controls For: Control Panel");
        inputHandlerScript.playerControls.FindAction("Move").Disable();
        inputHandlerScript.characterController.enabled = false;

        // Enable the steering and lever actions
        inputHandlerScript.playerControls.FindAction("Steer").Enable();
        inputHandlerScript.playerControls.FindAction("Lever").Enable();
        inputHandlerScript.playerControls.FindAction("HeightLever").Enable();
        inputHandlerScript.playerControls.FindAction("Hydrophone").Enable();

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
        inputHandlerScript.playerControls.FindAction("HeightLever").Disable();
        inputHandlerScript.playerControls.FindAction("Hydrophone").Disable();
    }

    // Note: any hydrophone actions are dealt in the hydrophone controller script.
    void Start()
    {
        steer = inputHandlerScript.steer;

        leverInput = inputHandlerScript.leverInput;
        heightLeverInput = inputHandlerScript.heightLeverInput;

        StartCoroutine(Lever());
        StartCoroutine(HeightLever());
    }

    // Update is called once per frame
    void Update()
    {
        Steer();
    }

    private void Steer()
    {
        float wheelDirection = -steer.ReadValue<float>();
        bool homing = false;
        if (wheelDirection == 0)
        {
            homing = true;
            if (wheelValue < 0)
            {
                wheelDirection = 1;
            }
            else if (wheelValue > 0)
            {
                wheelDirection = -1;
            }

        }

        wheelValue += wheelDirection * wheelRotationRate * Time.deltaTime;
        if (homing && ((wheelDirection < 0 && wheelValue < 0) || (wheelDirection > 0 && wheelValue > 0)))
        {
            // prevent jittering from overshoot zero
            wheelValue = 0;
        }

        // Wheel values from -1 to 1, inputted in submarine controls
        wheelValue = Mathf.Clamp(wheelValue, -1f, 1f);

        // Scale the z rotation by 90 to limit from -90 to 90 degrees
        wheel.transform.localEulerAngles = new Vector3(-90, 180, -wheelValue * 90);
    }

    // This method is highly inspired by the inchworm project. It takes in a queue of direction (up and down, W and S resoectively) and reads it
    // One by one.
    // TODO add animation between phases
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

    private IEnumerator HeightLever()
    {
        WaitUntil leverDir = new WaitUntil(() => heightLeverInput.Count > 0);

        while (true)
        {
            yield return leverDir;

            int leverHead = heightLeverInput[0];
            heightLeverInput.RemoveAt(0);

            if (leverHead == 1 && heightLeverValue < 1)
            {
                heightLeverValue += heightLeverInterval;
                heightLeverHandle.transform.localPosition += new Vector3(0, 0, heightLeverInterval * heightLeverMaxMin);
                yield return new WaitForSeconds(secondHeightLeverInterval);
            }
            else if (leverHead == -1 && heightLeverValue > -1)
            {
                heightLeverValue -= heightLeverInterval;
                heightLeverHandle.transform.localPosition -= new Vector3(0, 0, heightLeverInterval * heightLeverMaxMin);
                yield return new WaitForSeconds(secondHeightLeverInterval);
            }
            else
            {
                yield return null;
            }

        }
    }
}
