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
    void Start()
    {
    batteryHealth = 100;
    reactorHealth = 100;
    motorHealth = 100;
    displayHealth = 100;
    shipHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        batteryHealth = Mathf.Clamp(batteryHealth, 0, 100);
        reactorHealth = Mathf.Clamp(reactorHealth, 0, 100);
        motorHealth = Mathf.Clamp(motorHealth, 0, 100);
        displayHealth = Mathf.Clamp(displayHealth, 0, 100);
        shipHealth = Mathf.Clamp(shipHealth, 0, 100);
    }
}
