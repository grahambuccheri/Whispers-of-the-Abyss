using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



// Interface for any ship object. 
// A ship object MAY want us to freeze the user's camera or movement, hence the 
// disableAttributes and vice versa for enabling/turning attributes back on.
interface IInteractableShipObject
{
    public void interact();
    public void disableAttributes();
    public void enableAttributes();
}

// Interface for any tool object.
// Each tool has a scriptable object named ToolData where we can initialize or
// change its parameters such as its name, offset, rotation, etc. As well it
// has an interact method meant for interacting with ship objects
interface IInteractableTool
{
    public ToolData data { get; set; }
    public void interact();
    public void stopInteract();
}


[RequireComponent(typeof(CharacterController))]
public class InputHandler : MonoBehaviour
{

    public PlayerControls playerControls;
    public CharacterController characterController;

    // Different actions we have in our input system
    private InputAction movement;
    private InputAction mouse;
    private InputAction interact;
    private InputAction exitInteract;
    private InputAction click;


    public InputAction steer;
    public InputAction lever;
    public InputAction heightLever;

    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float gravity = -9.8f;

    public Camera mainCamera;
    private float cameraVerticalRotation = 0f;

    // Interaction and inventory variables
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private Inventory inventory;

    public List<int> leverInput = new List<int>();
    public List<int> heightLeverInput = new List<int>();

    private void Awake()
    {
        playerControls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerControls.Enable(); // ENABLE PLAYER CONTROL INPUT SYSTEM

        // Regular player actions
        movement = playerControls.Default.Move;
        mouse = playerControls.Default.Look;
        interact = playerControls.Default.Interact;
        exitInteract = playerControls.Default.ExitInteract;

        // Control Panel actions
        steer = playerControls.Default.Steer;
        lever = playerControls.Default.Lever;
        heightLever = playerControls.Default.HeightLever;

        // Holdable Object actions
        click = playerControls.Default.Click;


        interact.performed += OnInteract; // Subscribe to the OnIneract method 
        exitInteract.performed += OnExitInteract; // Subscribe to the ExitIneract method 

        lever.performed += OnLever;
        heightLever.performed += OnHeightLever;

        playerControls.FindAction("Lever").Disable();
        playerControls.FindAction("HeightLever").Disable();
        playerControls.FindAction("Steer").Disable();
    }

    private void OnDisable()
    {
        inventory.Reset();
        playerControls.Disable(); // DISABLE PLAYER CONTROL INPUT SYSTEM
    }

    void Start()
    {

    }

    void Update()
    {
        // Continuously update movement and player's mouse for the camera movement
        Movement();
        Look();

        // Update the position in which we hold the object (if the player is holding one)
        HoldObject();

        // Cursor, putting this in start makes the cursor disappear in locked state
        Cursor.lockState = CursorLockMode.Locked; // Lock Cursor
        Cursor.visible = true;
    }

    // HANDLING MOVEMENT METHOD
    private void Movement()
    {
        // Check if the character controller is enabled, character controller is disabled for certain ship functions like control panel to prevent moving the character
        if (characterController.enabled == true)
        {
            Vector2 readVec = movement.ReadValue<Vector2>();
            Vector3 readVec3 = transform.right * readVec.x + transform.forward * readVec.y + transform.up * gravity;

            characterController.Move(readVec3 * Time.deltaTime * playerSpeed);
        }
    }

    // HANDLING CAMERA FUNCTIONS TO LOOK AROUND
    private void Look()
    {
        Vector2 readVec = mouse.ReadValue<Vector2>();
        float mouseX = readVec.x * mouseSensitivity * Time.deltaTime;
        float mouseY = readVec.y * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        // Vertical rotation of the camera
        mainCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

        // Horizontal rotation of the camera
        transform.Rotate(Vector3.up * mouseX);
    }

    // HANDLING INTERACTIONS AND PICKING UP OBJECTS
    private void OnInteract(InputAction.CallbackContext context)
    {
        Ray r = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        // If the raycast is valid and we we are not currently interacting with anything right now
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactDistance) && inventory.interacting == false)
        {

            // Raycast hits a holdable object
            if (hitInfo.collider.gameObject.tag == "HoldableObject" && hitInfo.collider.gameObject.TryGetComponent(out IInteractableTool toolObj))
            {
                // Set inventory attributes
                GameObject holdableObject = hitInfo.collider.gameObject;
                inventory.item = holdableObject;

                inventory.interacting = true;
                inventory.toolFlag = true;

                inventory.itemInteractingName = toolObj.data.toolName;
            }
            // Raycast hits a ship/interactable object
            else if (hitInfo.collider.gameObject.tag == "InteractableObject" && hitInfo.collider.gameObject.TryGetComponent(out IInteractableShipObject interactObj))
            {
                // The toolflag is false in this case, this is because we know this is not an object to be carried but instead an interactable object on the ship

                GameObject interactableObject = hitInfo.collider.gameObject;
                inventory.item = interactableObject;

                inventory.interacting = true;
                inventory.itemInteractingName = interactableObject.name;

                // Call interface methods
                interactObj.disableAttributes();
                interactObj.interact();
            }
        }
    }

    // HANDLING EXITING INTERACTIONS
    private void OnExitInteract(InputAction.CallbackContext context)
    {
        // Check that we can exit a ship object interactable
        if (inventory.item != null)
        {

            IInteractableShipObject interactableShip = inventory.item.GetComponent(typeof(IInteractableShipObject)) as IInteractableShipObject;

            // Stop any interaction with the ship object
            if (interactableShip != null)
            {
                interactableShip.enableAttributes();
            }

            IInteractableTool interactableItem = inventory.item.GetComponent(typeof(IInteractableTool)) as IInteractableTool;

            // Stop any interaction with the ship object
            if (interactableItem != null)
            {
                interactableItem.stopInteract();
            }
        }

        if (inventory.interacting == true)
        {
            inventory.Reset();
        }


    }

    // HANDLING INTERACTIONS FOR HOLDING OBJECTS
    private void HoldObject()
    {
        // Checks if we can hold an object
        if (inventory.item != null && inventory.toolFlag == true)
        {

            // The code after this checks and detects for any clicks. If clicks
            // are detected, we can call the objects interact method due to 
            // interfaces.
            IInteractableTool interactable = inventory.item.GetComponent(typeof(IInteractableTool)) as IInteractableTool;

            if (interactable != null)
            {
                // Set the objects parent
                inventory.item.transform.parent = transform;

                // Grab the object's offset and rotation
                Vector3 offset = interactable.data.toolOffset;
                float distanceFromCamera = interactable.data.distanceFromCamera;
                Vector3 rotation = interactable.data.toolRotation;

                // Set the objects position and apply offset/rotation

                // inventory.item.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera + mainCamera.transform.right * distanceFromCamera;


                // First position the object close to the camera
                inventory.item.transform.position = mainCamera.transform.position + mainCamera.transform.forward + mainCamera.transform.right;
                // Then apply a local offset
                inventory.item.transform.localPosition += offset;

                // Set the objects rotation
                inventory.item.transform.localRotation = Quaternion.Euler(cameraVerticalRotation + rotation.x, rotation.y, rotation.z);


                float readClick = click.ReadValue<float>();
                if (readClick == 1f)
                {
                    interactable.interact();
                }
                else if (readClick == 0f)
                {
                    interactable.stopInteract();
                }
            }
        }
    }


    //Sorry Alex if this is in a weird spot, Just wanted to keep everything camera related on the player object.
    public void CameraShake()
    {

        StartCoroutine(ShakeCameraCo());
    }
    private IEnumerator ShakeCameraCo()
    {
        float shakeDuration = 0.3f; // Duration of the shake in seconds
        float shakeAmount = 0.4f; // Magnitude of the shake
        float decreaseFactor = 1.0f; // Factor by which the shake decreases each frame
        GameObject mc = GameObject.FindWithTag("MainCamera");

        Vector3 originalPosition = mc.transform.localPosition;
        float elapsedTime = 0.0f;
        //delay for sound
        yield return new WaitForSeconds(0.4f);

        while (elapsedTime < shakeDuration)
        {
            mc.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;

            elapsedTime += Time.deltaTime;
            shakeAmount *= decreaseFactor; // Decrease the shake amount each frame

            yield return null;
        }

        mc.transform.localPosition = originalPosition; // Reset the camera position back to normal
    }

    // Handles input for both levers
    private void OnLever(InputAction.CallbackContext context)
    {
        int leverDir = (int)context.ReadValue<float>();
        leverInput.Add(leverDir);
    }

    private void OnHeightLever(InputAction.CallbackContext context)
    {
        int leverDir = (int)context.ReadValue<float>();
        heightLeverInput.Add(leverDir);
    }
}
