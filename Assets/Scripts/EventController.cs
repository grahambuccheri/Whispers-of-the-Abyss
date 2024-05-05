using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public bool onBattery;
    string randomDirection;
    string componentToDamage;

    // Arrays of components available to damage from each direction
    string[] frontComponents = {"Cameras"};
    string[] backComponents = {"Reactor", "Motor"};
    string[] leftComponents = { "Batteries" , "Reactor" , "Motor" };
    string[] rightComponents = { "Batteries" , "Reactor" , "Motor" };
    string[] attackDirections = { "Front", "Back", "Left", "Right" };
    private void Start()
    {
        
    }
    //Coroutine to play particle effects for duration
    private IEnumerator PlayEffectCo(GameObject target,float duration)
    {
        ParticleSystem system = target.GetComponent<ParticleSystem>();
        system.Play();
        yield return new WaitForSeconds(2.0f);
        system.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided With object");
        if (other.gameObject.CompareTag("MonsterNode"))
        {
            //Check to see if you are running on battery
            Debug.Log("Object was a monster");
            if (onBattery)
            {
                if (Random.value > 0.75f)
                {
                    randomDirection = attackDirections[Random.Range(0, attackDirections.Length)];

                    // Determine which ship component to damage based on the attack direction
                    componentToDamage = GetComponentToDamage(randomDirection);

                    // Apply damage to the selected component
                    DamageComponent(componentToDamage);
                }
                else
                {
                    Debug.Log("Monster Didn't hear you");
                }
            }
            else {
                randomDirection = attackDirections[Random.Range(0, attackDirections.Length)];

                // Determine which ship component to damage based on the attack direction
                componentToDamage = GetComponentToDamage(randomDirection);

                // Apply damage to the selected component
                DamageComponent(componentToDamage);
            }
           
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
        //Connect to DamageControl script
       
    DamageControl damageControl = GetComponent<DamageControl>();
        AudioManager audioManager = GetComponent<AudioManager>();
        //grab the palayer model so we can do some camera shake
        InputHandler inputHandler = null;

        GameObject playerObject = GameObject.FindWithTag("PlayerObject");

        
        void PlayEffect(GameObject target)
        {
          if(target == null)
            {
                Debug.LogWarning("Target Effect does not exist");
            }
            else
            {
                Debug.Log("Sparking");
                StartCoroutine(PlayEffectCo(target,2.0f));
            }
        }

        if (playerObject != null)
        {
             inputHandler = playerObject.GetComponent<InputHandler>();
        }

        // Calculate random damage
        int damageAmount = Random.Range(10, 50); // Random damage values

        // Reduce health of the selected component
        switch (component)
        {
            case "Batteries":
                Debug.Log("Batteries damaged by " + damageAmount + " points!");
                damageControl.shipHealth -= 10;
                // Apply damage to batteries
                damageControl.batteryHealth -= damageAmount;
                // Play sound from component location
                audioManager.PlayRandomAudio(audioManager.audioSources[0]);
                inputHandler.CameraShake();
                PlayEffect(GameObject.Find("BatterySparks"));
                break;

            case "Motor":
                Debug.Log("Motor damaged by " + damageAmount + " points!");
                damageControl.shipHealth -= 15;
                // Apply damage to motor
                damageControl.motorHealth -= damageAmount;
                //playsound
                audioManager.PlayRandomAudio(audioManager.audioSources[3]);
                inputHandler.CameraShake();
                PlayEffect(GameObject.Find("MotorSparks"));
                break;

            case "Cameras":
                Debug.Log("Cameras damaged by " + damageAmount + " points!");
                damageControl.shipHealth -= 5;
                // Apply damage to cameras
                damageControl.displayHealth -= damageAmount;
                //playsound
                audioManager.PlayRandomAudio(audioManager.audioSources[1]);
                inputHandler.CameraShake();
                PlayEffect(GameObject.Find("DisplaySparks"));
                break;

            case "Reactor":
                Debug.Log("Reactor damaged by " + damageAmount + " points!");
                damageControl.shipHealth -= 20;
                // Apply damage to reactor
                damageControl.reactorHealth -= damageAmount;
                //playsound
                audioManager.PlayRandomAudio(audioManager.audioSources[2]);
                inputHandler.CameraShake();
                PlayEffect(GameObject.Find("ReactorSparks"));
                break;

            default:
                Debug.Log("Invalid component to damage!");
                break;
        }
        
    }
}
