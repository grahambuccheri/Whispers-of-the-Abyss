using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;

    // Arrays of components available to damage from each direction
    string[] frontComponents = {"Cameras"};
    string[] backComponents = {"Reactor", "Motor"};
    string[] leftComponents = { "Batteries" , "Reactor" , "Motor" };
    string[] rightComponents = { "Batteries" , "Reactor" , "Motor" };

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided With object");
        if (other.gameObject.CompareTag("MonsterNode"))
        {
            Debug.Log("Object was a monster");
            string[] attackDirections = { "Front", "Back", "Left", "Right" };
            string randomDirection = attackDirections[Random.Range(0, attackDirections.Length)];

            // Determine which ship component to damage based on the attack direction
            string componentToDamage = GetComponentToDamage(randomDirection);

            // Apply damage to the selected component
            DamageComponent(componentToDamage);
        }
    }

    private string GetComponentToDamage(string direction)
    {
        string componentToDamage = "";

        // Choose a random component from the array based on the direction
        switch (direction)
        {
            case "Front":
                componentToDamage = frontComponents[Random.Range(0, frontComponents.Length)];
                break;
            case "Back":
                componentToDamage = backComponents[Random.Range(0, backComponents.Length)];
                break;
            case "Left":
                componentToDamage = leftComponents[Random.Range(0, leftComponents.Length)];
                break;
            case "Right":
                componentToDamage = rightComponents[Random.Range(0, rightComponents.Length)];
                break;
        }

        return componentToDamage;
    }

    private void DamageComponent(string component)
    {
        // Calculate random damage
        int damageAmount = Random.Range(10, 50); // Random damage values

        // Reduce health of the selected component
        switch (component)
        {
            case "Batteries":
                Debug.Log("Batteries damaged by " + damageAmount + " points!");
                // Apply damage to batteries
                // Play sound from component location
                break;
            case "Motor":
                Debug.Log("Motor damaged by " + damageAmount + " points!");
                // Apply damage to motor
                break;
            case "Cameras":
                Debug.Log("Cameras damaged by " + damageAmount + " points!");
                // Apply damage to cameras
                break;
            case "Reactor":
                Debug.Log("Reactor damaged by " + damageAmount + " points!");
                // Apply damage to reactor
                break;
            default:
                Debug.Log("Invalid component to damage!");
                break;
        }
    }
}
