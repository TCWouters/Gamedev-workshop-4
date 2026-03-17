using UnityEngine;

public class Keymovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; // Rotation speed in degrees per second
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // The axis to rotate around

    void Update()
    {
        // Rotate the key around the specified axis
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}