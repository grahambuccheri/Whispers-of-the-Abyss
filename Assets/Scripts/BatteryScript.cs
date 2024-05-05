using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [Header("Battery Settings")]
    public float batteryValueNormalized;
    public float batteryValue;
    public float batteryRate;
    public float dialRotation;

    [SerializeField] private GameObject batteryDial;
    [SerializeField] private SubmarineController submarineController;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //Rotate dial based on recharging/depeleting battery
        batteryDial.transform.localEulerAngles = new Vector3(-90, 0, dialRotation);

        if (batteryValueNormalized > 0.955)
        {
            submarineController.SetThrottleLock(true);
        }
        else
        {
            submarineController.SetThrottleLock(false);
        }
    }
}
