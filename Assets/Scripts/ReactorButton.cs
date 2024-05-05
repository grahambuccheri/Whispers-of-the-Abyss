using UnityEngine;

public class ReactorButton : MonoBehaviour, IInteractableShipObject
{
    [SerializeField] Inventory inventory;
    [SerializeField] EventController eventControllerScript;
    [SerializeField] BatteryScript batteryScript;

    public void disableAttributes()
    {
        return;
    }

    public void enableAttributes()
    {
        return;
    }

    public void interact()
    {
        Debug.Log("Button Pressed");
        eventControllerScript.onBattery = !eventControllerScript.onBattery;

        inventory.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (eventControllerScript.onBattery == true && batteryScript.batteryValueNormalized <= 1)
        {

            batteryScript.batteryValueNormalized += Time.deltaTime * batteryScript.batteryRate;

            batteryScript.batteryValue = 100f - (batteryScript.batteryValueNormalized * 100f);

            batteryScript.dialRotation = batteryScript.batteryValueNormalized * 180f;
        }


    }
}
