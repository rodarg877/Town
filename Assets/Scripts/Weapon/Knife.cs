using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public WeaponAttributes weaponStats;
    public Transform attackOrigin; // Punto desde donde se mide la distancia (por ejemplo, la mano)
    public LayerMask enemyLayers;

    public override void Fire()
    {
        // No se usa en armas cuerpo a cuerpo (puede quedar vacío o lanzar un ataque si se desea)
    }

    public void PerformAttack()
    {
        // Detecta todos los colliders en un radio desde el origen
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, weaponStats.weaponDistance, enemyLayers);

        foreach (var hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(weaponStats.damage);
                Debug.Log("Golpeó a: " + hit.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null || weaponStats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, weaponStats.weaponDistance);
    }
}
