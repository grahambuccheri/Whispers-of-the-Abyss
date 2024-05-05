using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour, IInteractableTool
{

    [SerializeField] private ParticleSystem fireExtinguisher;
    [SerializeField] private float fireExtinguisherJuice;
    [SerializeField] private float fireExtinguisherDecreaseRate;

    [SerializeField] DamageControl damageControl;

    [SerializeField] Camera mainCamera;

    public ToolData SO;
    public ToolData data
    {
        get => SO;
        set => data = SO;
    }

    public void interact()
    {
        if (fireExtinguisherJuice > 0)
        {
            fireExtinguisher.Play(); // Play particle

            // Decrease juice
            fireExtinguisherJuice -= fireExtinguisherDecreaseRate * Time.deltaTime;

            // Create Ray
            Ray r = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            // Debug.DrawLine(r.origin, r.direction);

            // Send a raycast to a game object
            if (Physics.Raycast(r, out RaycastHit hitInfo, data.interactDistance))
            {
                // Debug.Log(hitInfo.collider.gameObject.name);
                increaseHealth(hitInfo);
            }
        }
        else
        {
            fireExtinguisherJuice = 0;
            stopInteract();
        }
    }

    public void stopInteract()
    {
        fireExtinguisher.Stop();
    }

    // This method matches the raycast hit info with whatever game object it is shooting at.

    // Specifically, we want to focus on the battery, reactor, motor, and control panel.
    public void increaseHealth(RaycastHit hitInfo)
    {
        string gameObjectName = hitInfo.collider.gameObject.name;
        Debug.Log(gameObjectName);

        if (gameObjectName.Equals(damageControl.batteryObject.name))
        {
            damageControl.batteryHealth += fireExtinguisherDecreaseRate * Time.deltaTime;
        }
        else if (gameObjectName.Equals(damageControl.reactorObject.name))
        {
            damageControl.reactorHealth += fireExtinguisherDecreaseRate * Time.deltaTime;
        }
        else if (gameObjectName.Equals(damageControl.motorObject.name))
        {
            damageControl.motorHealth += fireExtinguisherDecreaseRate * Time.deltaTime;
        }
        else if (gameObjectName.Equals(damageControl.displayObject.name))
        {
            damageControl.displayHealth += fireExtinguisherDecreaseRate * Time.deltaTime;
        }
    }
}
