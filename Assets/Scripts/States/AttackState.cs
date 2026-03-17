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
        Debug.Log("Enemy is attacking!");
    }

    public void Execute()
    {
        // Speler kan ontsnappen door uit zicht te gaan
        if (!enemyAI.CanSeePlayer() && !enemyAI.CanHearPlayer())
        {
            enemyAI.ChangeState(new IdleState(enemyAI));
            return;
        }

        // Teruggaan naar Alerted als niet meer gezien maar wel gehoord
        if (!enemyAI.CanSeePlayer() && enemyAI.CanHearPlayer())
        {
            enemyAI.ChangeState(new AlertedState(enemyAI));
            return;
        }

        // Aanvallen als speler nog gezien wordt
        if (enemyAI.CanSeePlayer())
        {
            enemyAI.AttackPlayer();

            // Check of speler gepakt is
            if (enemyAI.IsPlayerInRange())
            {
                enemyAI.CapturePlayer();
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Enemy stopped attacking.");
    }
}

