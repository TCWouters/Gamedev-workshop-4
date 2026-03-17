using UnityEngine;

public class PickupKey : MonoBehaviour
{
    public Collider vaultCollider;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            vaultCollider.enabled = false; // animation needed
            
            Destroy(gameObject);
        }
    }
}
