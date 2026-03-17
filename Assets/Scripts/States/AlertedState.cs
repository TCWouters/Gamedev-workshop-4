// de enemy hoort wat en gaat in de richting van het geluid of naar de speler die hij gezien of gehoord heeft. Hij gaat terug naar zijn IdleState
// als hij de speler niet meer ziet of hoort.

using UnityEngine;

public class AlertedState : IEnemyState
{
    private EnemyAI enemyAI;
    private float alertDuration = 0f;
    [SerializeField] private float alertTimeout = 5f; // Alert state max duration

    public AlertedState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
        alertDuration = 0f;
    }

    public void Enter()
    {
        Debug.Log("Enemy is alerted!");
        alertDuration = 0f;
    }

    public void Execute()
    {
        alertDuration += Time.deltaTime;

        // Direct attack als speler gezien
        if (enemyAI.CanSeePlayer())
        {
            enemyAI.ChangeState(new AttackState(enemyAI));
            return;
        }

        // Back to idle als niemand meer gehoord
        if (!enemyAI.CanHearPlayer())
        {
            enemyAI.ChangeState(new IdleState(enemyAI));
            return;
        }

        // Timeout: terug naar idle als te lang alerted zonder contact
        if (alertDuration > alertTimeout)
        {
            enemyAI.ChangeState(new IdleState(enemyAI));
            return;
        }

        // Navigeer naar speler als beschikbaar
        Transform playerTransform = enemyAI.GetPlayerTransform();
        if (playerTransform != null)
        {
            enemyAI.AttackPlayer(); // Hetzelfde als patrol, maar naar speler toe
        }
    }

    public void Exit()
    {
        Debug.Log("Enemy is no longer alerted.");
    }
}
