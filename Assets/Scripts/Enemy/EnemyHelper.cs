using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> zombiePrefabs;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxEnemyGroups = 5;
    [SerializeField] private float minEnemyDistanceFromPlayer = 5f;

    private Dictionary<GameObject, EnemyPool> pools = new();
    private HashSet<Vector3Int> printDictionary;
    private Vector3 player;

    public void InitializeSpawner(HashSet<Vector3Int> mapPositions, Vector3 playerPos)
    {
        printDictionary = mapPositions;
        player = playerPos;

        // Inicializar pools si no se han creado
        if (pools.Count == 0)
        {
            foreach (var prefab in zombiePrefabs)
            {
                pools[prefab] = new EnemyPool(prefab, initialPoolSize, transform);
            }
        }

        // Reiniciar todos los enemigos a inactivos
        foreach (var pool in pools.Values)
        {
            pool.ResetPool();
        }

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (printDictionary == null || player == null)
        {
            Debug.LogWarning("Spawner no inicializado correctamente.");
            return;
        }

        List<Vector3Int> validPositions = printDictionary
            .Where(p => Vector3.Distance(new Vector3(p.x + 0.5f, p.y, p.z + 0.5f), player) >= minEnemyDistanceFromPlayer)
            .ToList();

        for (int g = 0; g < maxEnemyGroups && validPositions.Count > 0; g++)
        {
            int groupSize = Random.Range(3, 6);
            int index = Random.Range(0, validPositions.Count);
            Vector3Int groupCenter = validPositions[index];
            validPositions.RemoveAt(index);

            for (int i = 0; i < groupSize; i++)
            {
                var prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Count)];
                GameObject enemy = pools[prefab].GetEnemy();

                Vector3 offset = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
                Vector3 tentativePos = new Vector3(groupCenter.x + 0.5f, groupCenter.y, groupCenter.z + 0.5f) + offset;

                // ✔️ Validar que la posición esté sobre el NavMesh
                if (NavMesh.SamplePosition(tentativePos, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    Debug.Log($"{gameObject.name} x {tentativePos.x} y {hit.position.y} z {tentativePos.z}");
                    enemy.transform.position = new Vector3(tentativePos.x, hit.position.y, tentativePos.z);
                    enemy.transform.rotation = Quaternion.identity;
                    enemy.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("No hay NavMesh cerca de " + tentativePos + ", enemigo no instanciado.");
                    enemy.SetActive(false);
                }
            }
        }
    }
}