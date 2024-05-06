using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageControl : MonoBehaviour
{

    public float shipHealth;
    public float batteryHealth;
    public float reactorHealth;
    public float motorHealth;
    public float displayHealth;
    public float fireValue;
    public float brokenValue;
    private Coroutine fireDamage;
    private GameObject[] objectsToHide;
    ParticleSystem system;
    [SerializeField] private SubmarineController submarineController;

    [Header("Component Objects")]
    public GameObject batteryObject;
    public GameObject reactorObject;
    public GameObject motorObject;
    public GameObject displayObject;

    void Start()
    {
        batteryHealth = 100;
        reactorHealth = 100;
        motorHealth = 100;
        displayHealth = 100;
        shipHealth = 100;
        //Values at which the components will be considered Broken or on fire.
        fireValue = 50;
        brokenValue = 40;
        objectsToHide = GameObject.FindGameObjectsWithTag("Hide");
        // Start the damage coroutine
        fireDamage = StartCoroutine(FireCo());
    }


    //Fire Damage. Procedurally remove hitpoints from the submarines main health while any component is on fire.
    private IEnumerator FireCo()
    {
        while (true)
        {
            if (checkFire(batteryHealth) || checkFire(displayHealth) || checkFire(reactorHealth) || checkFire(motorHealth))
            {
                shipHealth -= 4;
                //Debug.Log("target new health" + shipHealth);
            }
            // Apply damage to the target

            // Wait for the specified tick rate
            yield return new WaitForSeconds(2);
        }
    }
    private bool checkFire(float target)
    {
        //Debug.LogWarning("checking Health" + target);
        //enable fire and do 1 point of damage per second. 
        if (target <= fireValue)
        {
            //Start fire animation
            return true;
        }
        else
        {
            //Stop fire
            return false;
        }
    }


    private bool checkBroken(float target)
    {
        //dsiable fire, enable smoke
        if (target <= brokenValue)
        {
            //enable smoke
            //disable component
            return true;

        }
        else
        {
            //re-enable 
            return false;
        }
    }
    // Update is called once per frame


    void Update()
    {
        //checks for battery

        if (checkBroken(batteryHealth))
        {
            Debug.Log("DISABLE THROTTLE");
            submarineController.SetThrottleLock(true);
            system = GameObject.Find("BatterySparks").GetComponent<ParticleSystem>();
            if (system.isStopped)
            {
                system.Play();
            }

        }
        else
        {
            submarineController.SetThrottleLock(false);
            system = GameObject.Find("BatterySparks").GetComponent<ParticleSystem>();
            if (system.isPlaying)
            {
                system.Stop();
            }

        }
        if (checkFire(batteryHealth))
        {
            //turn on fire
            system = GameObject.Find("BatterySmoke").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            system = GameObject.Find("BatterySmoke").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
        }

        //checks for display

        if (checkBroken(displayHealth))
        {
            system = GameObject.Find("DisplaySparks").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
            foreach (var obj in objectsToHide)
            {
                if (obj != null)
                    obj.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            system = GameObject.Find("DisplaySparks").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
            foreach (var obj in objectsToHide)
            {
                if (obj != null)
                    obj.GetComponent<MeshRenderer>().enabled = true;
            }

        }
        if (checkFire(displayHealth))
        {
            system = GameObject.Find("DisplaySmoke").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            system = GameObject.Find("DisplaySmoke").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
        }

        //checks for reactor

        if (checkBroken(reactorHealth))
        {
            system = GameObject.Find("ReactorSparks").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            system = GameObject.Find("ReactorSparks").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }

        }
        if (checkFire(reactorHealth))
        {
            //turn on fire
            system = GameObject.Find("ReactorSmoke").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            //turn off fire
            system = GameObject.Find("ReactorSmoke").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
        }


        //checks for motor

        if (checkBroken(motorHealth))
        {
            submarineController.SetThrottleLock(true);

            system = GameObject.Find("MotorSparks").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            submarineController.SetThrottleLock(false);
            system = GameObject.Find("MotorSparks").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
        }
        if (checkFire(motorHealth))
        {
            system = GameObject.Find("MotorSmoke").GetComponent<ParticleSystem>();
            if (system.isStopped) { system.Play(); }
        }
        else
        {
            system = GameObject.Find("MotorSmoke").GetComponent<ParticleSystem>();
            if (system.isPlaying) { system.Stop(); }
        }

        if (shipHealth <= 0)
        {
            Debug.Log("You Died");
            SceneManager.LoadScene("Playspace", LoadSceneMode.Single);
        }

        batteryHealth = Mathf.Clamp(batteryHealth, 0, 100);
        reactorHealth = Mathf.Clamp(reactorHealth, 0, 100);
        motorHealth = Mathf.Clamp(motorHealth, 0, 100);
        displayHealth = Mathf.Clamp(displayHealth, 0, 100);
        shipHealth = Mathf.Clamp(shipHealth, 0, 100);

    }
}
