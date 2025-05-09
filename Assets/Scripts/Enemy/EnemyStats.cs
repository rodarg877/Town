using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemies/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public int maxHealth = 100;
    public float moveSpeed = 2f;
    public int attackDamage = 10;
    public float attackRange = 1.5f;
}
