using Unity.Services.Analytics;
using UnityEngine;

public class ZombieLento : Zombie
{
    public float speedMultiplier = 0.5f; // Velocidad reducida

    protected override void Awake()
    {
        base.Awake();
        agent.speed *= speedMultiplier; 
    }

    protected override void UpdateBehavior()
    {
        
        if (PlayerInSight())
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                Attack();
                agent.ResetPath(); // Detiene movimiento durante ataque
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            Wander();
        }
    }
}
