// de enemy hoort wat en gaat in de richting van het geluid of naar de speler die hij gezien of gehoord heeft. Hij gaat terug naar zijn IdleState
// als hij de speler niet meer ziet of hoort.

using UnityEngine;

public class AlertedState : IEnemyState
{
    private EnemyAI enemyAI;

    public AlertedState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        // Code to execute when entering the AlertedState
        Debug.Log("Enemy is alerted!");
    }

    public void Execute()
    {
        // Code to execute while in the AlertedState
        if (enemyAI.CanSeePlayer())
        {
            enemyAI.ChangeState(new AttackState(enemyAI));
        }
        else if (!enemyAI.CanHearPlayer())
        {
            enemyAI.ChangeState(new IdleState(enemyAI));
        }
    }

    public void Exit()
    {
        // Code to execute when exiting the AlertedState
        Debug.Log("Enemy is no longer alerted.");
    }
}
