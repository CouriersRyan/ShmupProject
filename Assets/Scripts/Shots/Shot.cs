using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class Shot : MonoBehaviour, IShot
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private float shotRate = 1; // shots per second
    [SerializeField] private Affiliation affiliation = Affiliation.Enemy;
    
    public List<ShotMod> Mods { get; set; }
    
    private GameObjectPool _bulletPool;
    [SerializeField] private float _timer;
    private float _shotInterval;

    private void Start()
    {
        if (GameObjectPool.Pools.ContainsKey(bullet.name))
        {
            _bulletPool = GameObjectPool.Pools[bullet.name];
        }
        else
        {
            _bulletPool = GameObjectPool.Create(bullet, 50, 150);
        }

        _timer = 0;
        _shotInterval = 1f / shotRate;
    }

    public virtual void Shooting(Vector2 dir)
    {
        _timer += Time.deltaTime;
        while (_timer > _shotInterval)
        {
            _timer -= _shotInterval;
            var newBullet = _bulletPool.Allocate();
            newBullet.transform.position = transform.position;
            var bullet = (newBullet as Bullet);
            bullet.dir = dir;
            bullet.affiliation = affiliation;
            bullet.owner = this;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            bullet.transform.rotation = rotation;
        }
    }
}
