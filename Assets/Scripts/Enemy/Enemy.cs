using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IDamageable, IPoolableEnemy
{
    [Header("Stats")]
    public EnemyStatsSO stats;

    protected int currentHealth;

    [Header("Percepción")]
    public float detectionRadius = 10f;
    public float visionAngle = 90f;
    public Transform eyes;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;
    protected Animator animator;    

    protected Transform player;
    protected NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void OnEnable()
    {
        if (NavMesh.SamplePosition(transform.position, out _, 1f, NavMesh.AllAreas))
        {
            ResetEnemy();
        }
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
        // En lugar de destruir, lo desactivamos
        gameObject.SetActive(false);
    }

    public virtual void ResetEnemy()
    {
        currentHealth = stats.maxHealth;

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.enabled = true;
        agent.speed = stats.moveSpeed;
        agent.isStopped = false;

        // Restaurar transformaciones si hace falta
        transform.rotation = Quaternion.identity;
    }
}
