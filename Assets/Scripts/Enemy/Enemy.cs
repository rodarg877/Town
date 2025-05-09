using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public EnemyStatsSO stats;

    protected int currentHealth;
    [Header("Percepción")]
    public float detectionRadius = 5f;
    public float visionAngle = 90f;
    public Transform eyes;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    protected Transform player;
    protected NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = stats.maxHealth;
        agent.speed = stats.moveSpeed;
    }

    protected virtual void Update()
    {
        UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}

