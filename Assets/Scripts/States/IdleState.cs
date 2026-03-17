//De enemy patrouilleert een gebied. Hij heeft een aantal waypoints die hij afloopt. Als hij de speler ziet of hoort gaat hij naar zijn AlertedState.
// Als hij de speler niet meer ziet of hoort gaat hij terug naar zijn IdleState.

using UnityEngine;  

public class IdleState : IEnemyState
{
    private EnemyAI enemyAI;

    public IdleState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        // Code to execute when entering the IdleState
        Debug.Log("Enemy is idle.");
    }

    public void Execute()
    {
        // Code to execute while in the IdleState
        if (enemyAI.CanSeePlayer() || enemyAI.CanHearPlayer())
        {
            enemyAI.ChangeState(new AlertedState(enemyAI));
        }
        else
        {
            // Code to handle patrolling between waypoints
            enemyAI.Patrol();
        }
    }

    public void Exit()
    {
        // Code to execute when exiting the IdleState
        Debug.Log("Enemy is no longer idle.");
    }
}