using System;
using System.Collections;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Shot representing the boss's shot behavior.
/// </summary>
public class ShotMeteor : Shot
{
    // fields
    // secondary and tertiary bullet pools for the boss.
    [SerializeField] protected Bullet bullet0;
    [SerializeField] protected Bullet bullet1;
    [SerializeField] private bool isSecondShotBehavior;
    
    protected GameObjectPool bulletPool0;
    protected GameObjectPool bulletPool1;

    // methods
    protected override void Start()
    {
        base.Start();
        // make pools for other bullets
        if (GameObjectPool.Pools.ContainsKey(bullet0.poolID))
        {
            bulletPool0 = GameObjectPool.Pools[bullet0.poolID];
        }
        else
        {
            bulletPool0 = GameObjectPool.Create(bullet0, 50, 150);
        }
        
        // make pools for other bullets
        if (GameObjectPool.Pools.ContainsKey(bullet1.poolID))
        {
            bulletPool1 = GameObjectPool.Pools[bullet1.poolID];
        }
        else
        {
            bulletPool1 = GameObjectPool.Create(bullet1, 50, 150);
        }
    }
    
    // methods
    /// <summary>
    /// Shoot bullets everytime the method is called, but alternates between shooting behaviors.
    /// </summary>
    /// <param name="dir"></param>
    public override void Shooting(Vector2 dir)
    {
        // this shot alternatives between two behaviors each time it is called,
        // shoot a fan of bullets
        if (!isSecondShotBehavior)
        {
            FannedShooting();
        }
        // shoot swarms of bullets aimed at the player in rapid succession
        else
        {
            StartCoroutine(BarragedShooting());
        }

        // switch bullet behavior to the other time.
        isSecondShotBehavior = !isSecondShotBehavior;
    }

    /// <summary>
    /// Behavior to shooting swarms of bullets at the player in rapid succession.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BarragedShooting()
    {
        // shoot three swarms.
        for (int i = 0; i < 3; i++)
        {
            // shoot three sets of bullets with slight offset for each swarm.
            Vector2 dir = (PlayerController.Player.transform.position - transform.position).normalized;
            MeteorShoot(dir + new Vector2(-0.1f, -0.1f), secondaryCount: 3, tertiaryCount: 0);
            MeteorShoot(dir+ new Vector2(0.1f, -0.1f), secondaryCount: 3, tertiaryCount: 0);
            MeteorShoot(dir, tertiaryCount: 8);
            yield return new WaitForSeconds(shotInterval);
        }
    }

    /// <summary>
    /// Shoot 5 swarms of bullets in a fan shape.
    /// </summary>
    private void FannedShooting()
    {
        MeteorShoot(Vector2.down, 6, 15);
        MeteorShoot(Vector2.left);
        MeteorShoot(Vector2.right);
        MeteorShoot(new Vector2(1, -1).normalized, 6);
        MeteorShoot(new Vector2(-1, -1).normalized, 6);
    }

    /// <summary>
    /// Fire a single swarm of bullets at once with a large bullet at the head and secondary and tertiary bullets
    /// to follow.
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="secondaryCount"></param>
    /// <param name="tertiaryCount"></param>
    private void MeteorShoot(Vector2 dir, int secondaryCount = 4, int tertiaryCount = 10)
    {
        float normalBulletSpeed = bulletSpeed;
        // shoot main bullet aimed directly at the player.
        SpawnBullet(dir, bulletSpeed, transform, bulletPool);
        
        // shoot secondary bullets with some randomized variance to the direction.
        for (int i = 0; i < secondaryCount; i++)
        {
            Vector2 randomInfluence = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.2f;
            SpawnBullet((dir + randomInfluence), Random.Range(bulletSpeed/4, bulletSpeed), transform, bulletPool0);
        }
        
        // shoot tertiary bullets with some randomized variance to the direction.
        for (int i = 0; i < tertiaryCount; i++)
        {
            Vector2 randomInfluence = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.4f;
            SpawnBullet((dir + randomInfluence), Random.Range(bulletSpeed/4, bulletSpeed), transform, bulletPool1);
        }

        bulletSpeed = normalBulletSpeed;
    }
}
