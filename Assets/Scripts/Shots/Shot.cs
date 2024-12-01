using System.Collections.Generic;
using Pool;
using UnityEngine;

/// <summary>
/// Represents a shot, something that shoots bullets for a shmup.
/// </summary>
public class Shot : MonoBehaviour, IShot
{
    // fields
    [SerializeField] private Bullet bullet;
    [SerializeField] private float shotRate = 1; // shots per second
    [SerializeField] private Affiliation affiliation = Affiliation.Enemy;
    
    public List<ShotMod> Mods { get; set; }
    
    private GameObjectPool bulletPool;
    [SerializeField] private float timer;
    private float shotInterval;

    // methods
    private void Start()
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
    }

    /// <summary>
    /// Shoot bullets at a set rate at a specified direction.
    /// </summary>
    /// <param name="dir"></param>
    public virtual void Shooting(Vector2 dir)
    {
        timer += Time.deltaTime;
        while (timer > shotInterval)
        {
            timer -= shotInterval;
            var newBullet = bulletPool.Allocate();
            newBullet.transform.position = transform.position;
            var newBullet1 = (newBullet as Bullet);
            newBullet1.dir = dir;
            newBullet1.affiliation = affiliation;
            newBullet1.owner = this;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            newBullet1.transform.rotation = rotation;
        }
    }
}
