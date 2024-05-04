using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour, IInteractableTool
{

    [SerializeField] private ParticleSystem fireExtinguisher;
    [SerializeField] private float fireExtinguisherJuice;
    [SerializeField] private float fireExtinguisherDecreaseRate;

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
            fireExtinguisher.Play();
            fireExtinguisherJuice -= fireExtinguisherDecreaseRate * Time.deltaTime;

            Ray r = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            Debug.DrawLine(r.origin, r.direction);

            if (Physics.Raycast(r, out RaycastHit hitInfo, data.interactDistance))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
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
}
