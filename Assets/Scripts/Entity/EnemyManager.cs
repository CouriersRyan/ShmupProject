using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class in-charge of handling enemy objects such as spawning.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    // fields
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxSpawnCount = 20;

    private const float Bounds = 8.5f;
    
    // methods
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

        HUDManager.Instance.OnGameOver += StopAllCoroutines;
    }

    private void OnDestroy()
    {
        HUDManager.Instance.OnGameOver -= StopAllCoroutines;
        StopAllCoroutines();
    }

    /// <summary>
    /// Spawn random enemies from the list at a fixed interval.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemyOverTime()
    {
        int count = 0;
        while (count < maxSpawnCount)
        {
            Enemy chosenEnemy = enemies[Random.Range(0, enemies.Length)];
            PoolableObject newEnemy = GameObjectPool.Pools[chosenEnemy.poolID].Allocate();
            newEnemy.transform.position = new Vector2(Random.Range(-Bounds, Bounds), 5);
            count++;
            yield return new WaitForSeconds(spawnInterval);
        }

        // A while after the last enemy spawns, end the game.
        yield return new WaitForSeconds(5.0f);
        HUDManager.Instance.GameWin();
    }
}
