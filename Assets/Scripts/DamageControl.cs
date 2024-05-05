using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{

    public int shipHealth;
    public int batteryHealth;
    public int reactorHealth;
    public int motorHealth;
    public int displayHealth;
    public int fireValue;
    public int brokenValue;
    private Coroutine fireDamage;
    [SerializeField] private SubmarineController submarineController;
    void Start()
    {
    batteryHealth = 100;
    reactorHealth = 100;
    motorHealth = 100;
    displayHealth = 100;
    shipHealth = 100;
     //Values at which the components will be considered Broken or on fire.
    fireValue = 25;
    brokenValue = 0;

        // Start the damage coroutine
        fireDamage = StartCoroutine(FireCo());
}


    //Fire Damage
    private IEnumerator FireCo()
    {
        while (true)
        {
            if (checkFire(batteryHealth) || checkFire(displayHealth) || checkFire(reactorHealth) || checkFire(motorHealth))
            {
                shipHealth -= 1;
                //Debug.Log("target new health" + shipHealth);
            }
            // Apply damage to the target
            
            // Wait for the specified tick rate
            yield return new WaitForSeconds(1);
        }
    }
    private bool checkFire(int target)
    {
        //Debug.LogWarning("checking Health" + target);
        //enable fire and do 1 point of damage per second. 
        if (target != 0 && target <= fireValue)
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
    

    private bool checkBroken(int target)
    {
        //dsiable fire, enable smoke
        if(target <= 0)
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
            
        }
        else
        {
            submarineController.SetThrottleLock(false);
        }
        //checks for display

        if (checkBroken(displayHealth))
        {

        }
        else
        {

        }
        //checks for reactor

        if (checkBroken(reactorHealth))
        {
           
        }
        else
        {
                
        }
        //checks for motor

        if (checkBroken(motorHealth))
        {
            submarineController.SetThrottleLock(true);
        }
        else
        {
            submarineController.SetThrottleLock(false);
        }

        batteryHealth = Mathf.Clamp(batteryHealth, 0, 100);
        reactorHealth = Mathf.Clamp(reactorHealth, 0, 100);
        motorHealth = Mathf.Clamp(motorHealth, 0, 100);
        displayHealth = Mathf.Clamp(displayHealth, 0, 100);
        shipHealth = Mathf.Clamp(shipHealth, 0, 100);
        

    }
}
