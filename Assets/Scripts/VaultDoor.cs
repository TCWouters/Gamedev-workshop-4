using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    public float openDistance = 5f; // Distance at which the door opens
    public float rotationSpeed = 45f; // Speed at which the door opens (degrees per second)
    public float openAngle = 90f; // Angle to rotate the door when opening
    public Vector3 rotationAxis = Vector3.up; // Axis to rotate around (Y for normal doors)
    public Vector3 pivotOffset = new Vector3(-1f, 0f, 0f); // Offset of the hinge from the door's center
    
    private Vector3 initialPosition;
    private Vector3 pivotPoint;
    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private bool isOpen = false;
    private Transform player;

    void Start()
    {
        // Find the player (usually the Main Camera or FirstPersonMovement)
        player = FindObjectOfType<FirstPersonMovement>()?.transform;
        
        initialPosition = transform.position;
        UpdatePivotPoint();
    }
    
    void UpdatePivotPoint()
    {
        pivotPoint = initialPosition + transform.TransformDirection(pivotOffset);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(initialPosition, player.position);

        if (distanceToPlayer < openDistance && !isOpen && PickupKey.hasKey)
        {
            isOpen = true;
        }
        else if ((distanceToPlayer >= openDistance || !PickupKey.hasKey) && isOpen)
        {
            isOpen = false;
        }

        // Set target angle based on open state
        targetAngle = isOpen ? openAngle : 0f;
        
        // Smoothly rotate towards target angle
        if (Mathf.Abs(currentAngle - targetAngle) > 0.01f)
        {
            float direction = targetAngle > currentAngle ? 1f : -1f;
            float rotation = direction * rotationSpeed * Time.deltaTime;
            
            if (Mathf.Abs(currentAngle + rotation - targetAngle) < 0.01f)
            {
                rotation = targetAngle - currentAngle;
            }
            
            // Rotate around the pivot point
            UpdatePivotPoint();
            transform.RotateAround(pivotPoint, rotationAxis, rotation);
            currentAngle += rotation;
        }
    }
}
