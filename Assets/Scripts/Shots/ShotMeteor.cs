using System;
using System.Collections;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotMeteor : Shot
{
    [SerializeField] protected Bullet bullet0;
    [SerializeField] protected Bullet bullet1;
    [SerializeField] private bool isSecondShotBehavior;
    
    protected GameObjectPool bulletPool0;
    protected GameObjectPool bulletPool1;

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
            //shoot a swarm of bullets
            if (!isSecondShotBehavior)
            {
                FannedShooting();
            }
            else
            {
                StartCoroutine(BarragedShooting());
            }

            isSecondShotBehavior = !isSecondShotBehavior;
    }

    private IEnumerator BarragedShooting()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 dir = (PlayerController.Player.transform.position - transform.position).normalized;
            MeteorShoot(dir + new Vector2(-0.1f, -0.1f), secondaryCount: 3, tertiaryCount: 0);
            MeteorShoot(dir+ new Vector2(0.1f, -0.1f), secondaryCount: 3, tertiaryCount: 0);
            MeteorShoot(dir, tertiaryCount: 8);
            yield return new WaitForSeconds(shotInterval);
        }
    }

    private void FannedShooting()
    {
        MeteorShoot(Vector2.down, 6, 15);
        MeteorShoot(Vector2.left);
        MeteorShoot(Vector2.right);
        MeteorShoot(new Vector2(1, -1).normalized, 6);
        MeteorShoot(new Vector2(-1, -1).normalized, 6);
    }

    private void MeteorShoot(Vector2 dir, int secondaryCount = 4, int tertiaryCount = 10)
    {
        float normalBulletSpeed = bulletSpeed;
        SpawnBullet(dir, bulletSpeed, transform, bulletPool);
        
        for (int i = 0; i < secondaryCount; i++)
        {
            Vector2 randomInfluence = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.2f;
            SpawnBullet((dir + randomInfluence), Random.Range(bulletSpeed/4, bulletSpeed), transform, bulletPool0);
        }

        //bulletSpeed = bulletSpeed / 4f;
        for (int i = 0; i < tertiaryCount; i++)
        {
            //bulletSpeed = Random.Range(-1, 1);
            Vector2 randomInfluence = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.4f;
            SpawnBullet((dir + randomInfluence), Random.Range(bulletSpeed/4, bulletSpeed), transform, bulletPool1);
        }

        bulletSpeed = normalBulletSpeed;
    }
}
