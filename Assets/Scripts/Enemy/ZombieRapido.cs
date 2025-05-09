using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class ZombieRapido : Zombie
   {
    public float speedMultiplier = 1.5f; // Velocidad extra

    protected override void Awake()
    {
        base.Awake();
        agent.speed *= speedMultiplier; // Ajusta la velocidad de movimiento
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
