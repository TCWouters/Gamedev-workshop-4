// de Enemy ziet de speler en valt aan. Als hij de speler pakt is het meteen game over. de speler kan ontsnappen door weg te rennen en zich uit
// het zicht van de Enemy te bevinden. als de speler op een bepaalde afstand is verliest de enemy de speler uit het oog en gaat terug naar zijn
// AlertedState.

using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyAI enemyAI;

    public AttackState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        // Code to execute when entering the AttackState
        Debug.Log("Enemy is attacking!");
    }

    public void Execute()
    {
        // Code to execute while in the AttackState
        if (!enemyAI.CanSeePlayer())
        {
            enemyAI.ChangeState(new AlertedState(enemyAI));
        }
        else if (enemyAI.IsPlayerInRange())
        {
            // Code to handle player capture and game over
            Debug.Log("Player captured! Game Over.");
            // Implement game over logic here
        }
    }

    public void Exit()
    {
        // Code to execute when exiting the AttackState
        Debug.Log("Enemy stopped attacking.");
    }
}