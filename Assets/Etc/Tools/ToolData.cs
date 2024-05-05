using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "ScriptableObjects/Tool")]
public class ToolData : ScriptableObject
{
    public string toolName; // Name

    // Variables to determine positioning relative to the player
    public Vector3 toolOffset;
    public float distanceFromCamera;
    public Vector3 toolRotation;

    // Icon
    public Sprite toolSprite;

    // Interaction distance of object
    public float interactDistance;
}
