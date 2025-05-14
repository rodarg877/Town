using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFactory : EnemyFactory
{
    private List<GameObject> zombiePrefabs;

    public ZombieFactory(List<GameObject> prefabs)
    {
        zombiePrefabs = prefabs;
    }

    public override GameObject CreateEnemy()
    {
        if (zombiePrefabs == null || zombiePrefabs.Count == 0)
        {
            Debug.LogWarning("No hay prefabs en la fábrica.");
            return null;
        }

        int index = Random.Range(0, zombiePrefabs.Count);
        return Object.Instantiate(zombiePrefabs[index]);
    }
}
