using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public abstract class Zombie : Enemy
{
    public float wanderRadius = 5f;
    public float wanderTimer = 3f;
    public float attackRange;
    public float attackCooldown = 2f;

    protected float timer;
    protected float attackTimer;
    protected Vector3 wanderTarget;

    protected override void Awake()
    {
        base.Awake();
        attackRange = stats.attackRange;
        attackTimer = 0f;
        
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        UpdateBehavior();
    }
    // Detectar al jugador
    protected bool PlayerInSight()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - eyes.position).normalized;
        float distanceToPlayer = Vector3.Distance(eyes.position, player.position);

        if (distanceToPlayer > detectionRadius) return false;

        float angle = Vector3.Angle(eyes.forward, directionToPlayer);
        if (angle < visionAngle / 2f)
        {
            // Verifica si hay línea de visión
            if (!Physics.Raycast(eyes.position, directionToPlayer, distanceToPlayer, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    // Patrullaje
    protected void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, wanderRadius, NavMesh.AllAreas))
            {
                wanderTarget = navHit.position;
                agent.SetDestination(wanderTarget);
            }

            timer = 0;
        }
    }

    // Ataque
    protected void Attack()
    {
        if (attackTimer > 0f) return;

        // Aquí deberías reproducir animación y aplicar daño
        Debug.Log("Zombie ataca al jugador!");

        IDamageable target = player.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(stats.attackDamage); // O usa un valor de stats
        }

        attackTimer = attackCooldown;
    }
}

