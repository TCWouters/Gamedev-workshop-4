using UnityEngine;

public class PickupKey : MonoBehaviour
{
    public static bool hasKey = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {  
            hasKey = true;
            // vaultCollider.enabled = false; // animation needed
            Debug.Log("Key picked up!");
            Destroy(gameObject);
        }
    }
}
