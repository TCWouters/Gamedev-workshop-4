using UnityEngine;

public class EndGame : MonoBehaviour
{
    void Update()
    {
        // Stop the timer when the player reaches the end with the key
        if (PickupKey.hasKey)
        {
            EventManager.OnTimerStop();
            enabled = false; // Disable this script after stopping the timer
        }
    }
}
