using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private float spawnInterval;

    private const float Bounds = 8.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all enemies once as well as their pools.
        foreach (var enemy in enemies)
        {
            // enemies are poolable objects, so create a pool if one isn't made.
            if (GameObjectPool.Pools.ContainsKey(enemy.poolID))
            {
            }
            else
            {
                GameObjectPool.Create(enemy, 10, 30);
            }
        }
        
        // Begin spawning enemies.
        StartCoroutine(SpawnEnemyOverTime());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemyOverTime()
    {
        while (true)
        {
            Enemy chosenEnemy = enemies[Random.Range(0, enemies.Length)];
            PoolableObject newEnemy = GameObjectPool.Pools[chosenEnemy.poolID].Allocate();
            newEnemy.transform.position = new Vector2(Random.Range(-Bounds, Bounds), 5);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
