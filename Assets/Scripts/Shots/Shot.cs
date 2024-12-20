using System.Collections.Generic;
using Physics;
using Pool;
using UnityEngine;

/// <summary>
/// Represents a shot, something that shoots bullets for a shmup.
/// </summary>
public class Shot : MonoBehaviour, IShot
{
    // fields
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected float shotRate = 1; // shots per second
    [SerializeField] protected Affiliation affiliation = Affiliation.Enemy;
    [SerializeField] protected float bulletSpeed = 20f;

    [SerializeField] protected Transform[] firePoints;
    [SerializeField] protected bool useFirePoints;
    
    public List<ShotMod> Mods { get; set; }
    
    protected GameObjectPool bulletPool;
    [SerializeField] protected float timer;
    protected float shotInterval;
    [SerializeField] private PhysicsBody pb;

    // methods
    protected virtual void Start()
    {
        if (GameObjectPool.Pools.ContainsKey(bullet.poolID))
        {
            bulletPool = GameObjectPool.Pools[bullet.poolID];
        }
        else
        {
            bulletPool = GameObjectPool.Create(bullet, 50, 150);
        }

        timer = 0;
        shotInterval = 1f / shotRate;

        if (useFirePoints == false) firePoints = new []{pb.transform};
    }

    /// <summary>
    /// Shoot bullets at a set rate at a specified direction.
    /// </summary>
    /// <param name="dir"></param>
    public virtual void Shooting(Vector2 dir)
    {
        timer += Time.deltaTime;
        // shoot at fixed intervals
        while (timer > shotInterval)
        {
            timer -= shotInterval;

            foreach (var point in firePoints)
            {
                SpawnBullet(dir, point);
            }
        }
    }

    /// <summary>
    /// Spawns the default bullet when a direction it is facing and a transform.
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="point"></param>
    protected void SpawnBullet(Vector2 dir, Transform point)
    {
        SpawnBullet(dir, bulletSpeed, point, bulletPool);
    }

    /// <summary>
    /// Resolves all bullet allocation and set up logic, such as setting a speed, and choosing what bullet to spawn.
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="point"></param>
    /// <param name="bPool"></param>
    protected void SpawnBullet(Vector2 dir, float speed, Transform point, GameObjectPool bPool)
    {
        // get new bullet from pool and set relevant values.
        PoolableObject newBullet = bPool.Allocate();
        newBullet.transform.position = point.position;
        Bullet newBullet1 = (newBullet as Bullet);

        float theta = point.rotation.eulerAngles.z * Mathf.Deg2Rad;
        newBullet1.Direction = new Vector2(dir.x * Mathf.Cos(theta) - dir.y * Mathf.Sin(theta),
            dir.x * Mathf.Sin(theta) + dir.y * Mathf.Cos(theta));
                
        newBullet1.speed = speed;
        newBullet1.affiliation = affiliation;
        newBullet1.owner = this;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
        newBullet1.transform.rotation = rotation;
    }

    /// <summary>
    /// Called when poolable object needs to be recycled.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void OnRecycle()
    {
        
    }
}
