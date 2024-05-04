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
        if (eventControllerScript.onBattery == true)
        {
            batteryScript.batteryValue -= Time.deltaTime * batteryScript.batteryRate;
        }
    }
}
