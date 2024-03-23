using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void interact();
}


[RequireComponent(typeof(CharacterController))]
public class InputHandler : MonoBehaviour
{

    private PlayerControls playerControls;
    private CharacterController characterController;

    // Different actions we have in our input system
    private InputAction movement;
    private InputAction mouse;
    private InputAction interact;
    private InputAction exitInteract;

    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private float mouseSensitivity = 10f;

    [SerializeField] private Camera mainCamera;
    private float cameraVerticalRotation = 0f;

    // Interaction and inventory variables
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private float distanceFromCamera = 1.2f;
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        playerControls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerControls.Enable(); // ENABLE PLAYER CONTROL INPUT SYSTEM

        movement = playerControls.Default.Move;
        mouse = playerControls.Default.Look;
        interact = playerControls.Default.Interact;
        exitInteract = playerControls.Default.ExitInteract;

        interact.performed += OnInteract; // Subscribe to the OnIneract method 
        exitInteract.performed += OnExitInteract; // Subscribe to the ExitIneract method 
    }

    private void OnDisable()
    {
        playerControls.Disable(); // DISABLE PLAYER CONTROL INPUT SYSTEM
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock Cursor
    }

    void Update()
    {
        // Continuously update movement and player's mouse for the camera movement
        Movement();
        Look();

        // Update the position in which we hold the object (if the player is holding one)
        HoldObject();
    }

    // HANDLING MOVEMENT METHOD
    private void Movement()
    {

        Vector2 readVec = movement.ReadValue<Vector2>();
        Vector3 readVec3 = transform.right * readVec.x + transform.forward * readVec.y;

        characterController.Move(readVec3 * Time.deltaTime * playerSpeed);
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
            // if (hitInfo.collider.gameObject.tag == "HoldableObject" && hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            // {
            //     holdableObject = hitInfo.collider.gameObject;
            // }

            // Raycast hits a holdable object
            if (hitInfo.collider.gameObject.tag == "HoldableObject")
            {
                GameObject holdableObject = hitInfo.collider.gameObject;
                inventory.item = holdableObject;

                inventory.interacting = true;
                inventory.toolFlag = true;
                inventory.itemInteractingName = holdableObject.name;

            }
            // Raycast hits a ship/interactable object
            else if (hitInfo.collider.gameObject.tag == "InteractableObject" && hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // The toolflag is false in this case, this is because we know this is not an object to be carried but instead an interactable object on the ship

                GameObject interactableObject = hitInfo.collider.gameObject;
                inventory.item = interactableObject;

                inventory.interacting = true;
                inventory.itemInteractingName = interactableObject.name;

                interactObj.interact();

            }
        }
    }

    // HANDLING EXITING INTERACTIONS
    private void OnExitInteract(InputAction.CallbackContext context)
    {
        if (inventory.interacting == true)
        {
            inventory.Reset();
        }
    }

    // HANDLING INTERACTIONS FOR HOLDING OBJECTS
    private void HoldObject()
    {
        if (inventory.item != null && inventory.toolFlag == true)
        {
            inventory.item.transform.parent = transform;
            inventory.item.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera + mainCamera.transform.right * distanceFromCamera;
            inventory.item.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
        }
    }


}
