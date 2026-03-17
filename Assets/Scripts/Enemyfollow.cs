using UnityEngine;

public class Enemyfollow : MonoBehaviour
{
    public float speed = 5f;
    private Transform playerTransform;

    void Start()
    {
        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Calculate direction to player
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Move towards the player
        transform.position += direction * speed * Time.deltaTime;
    }
}
