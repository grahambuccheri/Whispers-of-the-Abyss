using UnityEngine;

public class ReactorButton : MonoBehaviour, IInteractableShipObject
{
    [SerializeField] Inventory inventory;
    [SerializeField] EventController eventControllerScript;
    [SerializeField] BatteryScript batteryScript;
    bool isOn = false;

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
        Debug.Log("Reactor Button Pressed");
        Transform childTransform = transform.GetChild(0);
        GameObject childObject = childTransform.gameObject;
        inventory.Reset();
        if (isOn)
        {
            isOn = false;
            childObject.transform.localPosition += new Vector3(0, 0.6f, 0);
            eventControllerScript.onBattery = false;
            transform.GetChild(2).gameObject.GetComponent<Light>().color = new Color32(236, 208, 160, 255);
            transform.GetChild(3).gameObject.GetComponent<Light>().color = new Color32(236, 208, 160, 255);
        }
        else
        {
            isOn = true;
            childObject.transform.localPosition += new Vector3(0, -0.6f, 0);
            eventControllerScript.onBattery = true;
            transform.GetChild(2).gameObject.GetComponent<Light>().color = new Color32(213, 104, 61, 255);
            transform.GetChild(3).gameObject.GetComponent<Light>().color = new Color32(213, 104, 61, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Here is how the battery script and values work:

        // batteryValueNormalized is from 0 to 1, initially starts at 0, it drains the battery but in reverse (for simplicity reasons).

        // batteryRate is the rate at which the battery is drained.

        // batteryValue is the literal value from 0 to 100. However, we start at 100 first (this is opposite to batteryValueNormalized starting at 0 initially)

        // dialRotation is the reason why batteryValueNormalized starts at 0. It is simpler to add degrees than figuring and calculating its degrees starting at 180.
        if (eventControllerScript.onBattery && batteryScript.batteryValueNormalized <= 1)
        {

            batteryScript.batteryValueNormalized += Time.deltaTime * batteryScript.batteryRate;

            batteryScript.batteryValueNormalized = Mathf.Clamp(batteryScript.batteryValueNormalized, 0, 1);

            batteryScript.batteryValue = 100f - (batteryScript.batteryValueNormalized * 100f);

            // Rotate counter clockwise
            batteryScript.dialRotation = -batteryScript.batteryValueNormalized * 180f;
        }
        else if (!eventControllerScript.onBattery && batteryScript.batteryValueNormalized >= 0)
        {
            batteryScript.batteryValueNormalized -= Time.deltaTime * batteryScript.batteryRate;
            batteryScript.batteryValueNormalized = Mathf.Clamp(batteryScript.batteryValueNormalized, 0, 1);

            batteryScript.batteryValue = 100f - (batteryScript.batteryValueNormalized * 100f);

            // Rotate clockwise
            batteryScript.dialRotation = -batteryScript.batteryValueNormalized * 180f;
        }


    }
}
