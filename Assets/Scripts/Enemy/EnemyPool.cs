using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly List<GameObject> pool = new List<GameObject>();

    public EnemyPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject instance = GameObject.Instantiate(prefab, Vector3.up * 2f, Quaternion.identity, parent);
            instance.SetActive(false);
            pool.Add(instance);
        }
    }

    public GameObject GetEnemy()
    {
        foreach (var enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // Si no hay enemigos disponibles, instanciar uno nuevo
        GameObject instance = GameObject.Instantiate(prefab, Vector3.up * 2f, Quaternion.identity, parent);
        instance.SetActive(false);
        pool.Add(instance);
        return instance;
    }

    public void ResetPool()
    {
        foreach (var enemy in pool)
        {
            enemy.SetActive(false);
        }
    }
}
